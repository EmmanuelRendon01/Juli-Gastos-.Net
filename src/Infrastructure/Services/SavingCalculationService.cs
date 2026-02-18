using JuliGastos.Application.DTOs;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.Infrastructure.Services;

public class SavingCalculationService : ISavingCalculationService
{
    private readonly ISavingPlanAccountRepository _savingPlanAccountRepository;
    private readonly IFinancialCommitmentRepository _financialCommitmentRepository;
    private readonly IRecurringIncomeRepository _recurringIncomeRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    public SavingCalculationService(
        ISavingPlanAccountRepository savingPlanAccountRepository,
        IFinancialCommitmentRepository financialCommitmentRepository,
        IRecurringIncomeRepository recurringIncomeRepository,
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        _savingPlanAccountRepository = savingPlanAccountRepository;
        _financialCommitmentRepository = financialCommitmentRepository;
        _recurringIncomeRepository = recurringIncomeRepository;
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task<decimal> CalculateCurrentSavingsAsync(long savingPlanId, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Obtener las cuentas vinculadas al plan
        var savingPlanAccounts = await _savingPlanAccountRepository.GetBySavingPlanIdAsync(savingPlanId, cancellationToken);
        
        // 2. Sumar los balances de todas las cuentas vinculadas
        var totalSavings = savingPlanAccounts
            .Where(spa => spa.Account != null)
            .Sum(spa => spa.Account!.CurrentBalance);
        
        return totalSavings;
    }

    public async Task<decimal> CalculateMonthlyCommitmentsAsync(long userId, CancellationToken cancellationToken = default)
    {
        // 1. Obtener todos los compromisos financieros activos del usuario
        var commitments = await _financialCommitmentRepository.GetActiveByUserIdAsync(userId, cancellationToken);
        
        // 2. Convertir cada compromiso a monto mensual y sumar
        var monthlyTotal = commitments.Sum(c => ConvertToMonthly(c.Amount, c.Frequency));
        
        return monthlyTotal;
    }

    public async Task<decimal> CalculateMonthlyRecurringIncomesAsync(long userId, CancellationToken cancellationToken = default)
    {
        // 1. Obtener todos los ingresos recurrentes activos del usuario
        var incomes = await _recurringIncomeRepository.GetActiveByUserIdAsync(userId, cancellationToken);
        
        // 2. Convertir cada ingreso a monto mensual y sumar
        var monthlyTotal = incomes.Sum(i => ConvertToMonthly(i.Amount, i.Frequency));
        
        return monthlyTotal;
    }

    public async Task<decimal> CalculateAverageVariableExpensesAsync(long userId, int months = 3, CancellationToken cancellationToken = default)
    {
        // 1. Calcular fecha de inicio (N meses atrás)
        var startDate = DateTimeOffset.UtcNow.AddMonths(-months);
        
        // 2. Obtener todas las transacciones de tipo Expense del usuario en el período
        var transactions = await _transactionRepository.GetAllByUserIdAsync(userId);
        
        // 3. Filtrar transacciones de gasto en el rango de fechas
        var expenses = transactions
            .Where(t => t.Type == TransactionType.Expense && t.Date >= startDate)
            .ToList();
        
        // 4. Calcular el total de gastos
        var totalExpenses = expenses.Sum(t => t.Amount);
        
        // 5. Calcular el promedio mensual
        var averageMonthly = months > 0 ? totalExpenses / months : 0;
        
        return averageMonthly;
    }

    public async Task<SavingCurrentStatus> CalculateCurrentStatusAsync(long savingPlanId, decimal targetAmount, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Calcular total en cuentas vinculadas
        var totalInAccounts = await CalculateCurrentSavingsAsync(savingPlanId, userId, cancellationToken);
        
        // 2. Calcular compromisos mensuales
        var monthlyCommitments = await CalculateMonthlyCommitmentsAsync(userId, cancellationToken);
        
        // 3. Calcular gastos variables promedio
        var averageVariableExpenses = await CalculateAverageVariableExpensesAsync(userId, 3, cancellationToken);
        
        // 4. Calcular disponible neto (considerando solo los compromisos del mes)
        var availableNow = totalInAccounts - monthlyCommitments;
        
        // 5. Calcular progreso en porcentaje
        var progressPercentage = targetAmount > 0 ? (totalInAccounts / targetAmount) * 100 : 0;
        
        return new SavingCurrentStatus(
            TotalInAccounts: totalInAccounts,
            MonthlyCommitments: monthlyCommitments,
            AverageVariableExpenses: averageVariableExpenses,
            AvailableNow: availableNow,
            ProgressPercentage: Math.Round(progressPercentage, 2)
        );
    }

    public async Task<SavingProjection> CalculateProjectionAsync(long savingPlanId, decimal targetAmount, DateTimeOffset targetDate, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Calcular ahorro actual
        var currentSavings = await CalculateCurrentSavingsAsync(savingPlanId, userId, cancellationToken);
        
        // 2. Calcular ingresos mensuales recurrentes
        var monthlyRecurringIncomes = await CalculateMonthlyRecurringIncomesAsync(userId, cancellationToken);
        
        // 3. Calcular gastos mensuales (fijos + variables)
        var monthlyCommitments = await CalculateMonthlyCommitmentsAsync(userId, cancellationToken);
        var averageVariableExpenses = await CalculateAverageVariableExpensesAsync(userId, 3, cancellationToken);
        var monthlyTotalExpenses = monthlyCommitments + averageVariableExpenses;
        
        // 4. Calcular capacidad de ahorro mensual
        var monthlySavingCapacity = monthlyRecurringIncomes - monthlyTotalExpenses;
        
        // 5. Calcular meses hasta la fecha objetivo
        var monthsToTarget = CalculateMonthsDifference(DateTimeOffset.UtcNow, targetDate);
        
        // 6. Calcular monto proyectado
        var projectedAmount = currentSavings + (monthlySavingCapacity * monthsToTarget);
        
        // 7. Determinar si alcanzará la meta
        var willReachTarget = projectedAmount >= targetAmount;
        
        // 8. Calcular excedente/faltante
        var surplus = projectedAmount - targetAmount;
        
        // 9. Calcular fecha estimada de completitud (si tiene capacidad positiva)
        DateTimeOffset? estimatedCompletionDate = null;
        if (monthlySavingCapacity > 0 && targetAmount > currentSavings)
        {
            var amountNeeded = targetAmount - currentSavings;
            var monthsNeeded = (int)Math.Ceiling((double)(amountNeeded / monthlySavingCapacity));
            estimatedCompletionDate = DateTimeOffset.UtcNow.AddMonths(monthsNeeded);
        }
        else if (currentSavings >= targetAmount)
        {
            // Ya alcanzó la meta
            estimatedCompletionDate = DateTimeOffset.UtcNow;
        }
        
        return new SavingProjection(
            MonthlyRecurringIncomes: monthlyRecurringIncomes,
            MonthlyTotalExpenses: monthlyTotalExpenses,
            MonthlySavingCapacity: monthlySavingCapacity,
            MonthsToTarget: monthsToTarget,
            ProjectedAmount: projectedAmount,
            WillReachTarget: willReachTarget,
            Surplus: surplus,
            EstimatedCompletionDate: estimatedCompletionDate
        );
    }

    public async Task<FinancialSummary> CalculateFinancialSummaryAsync(long userId, CancellationToken cancellationToken = default)
    {
        // 1. Calcular total en todas las cuentas del usuario
        var allAccounts = await _accountRepository.GetAllByUserIdAsync(userId);
        var totalInAllAccounts = allAccounts
            .Where(a => a.IsActive)
            .Sum(a => a.CurrentBalance);
        
        // 2. Calcular compromisos mensuales
        var monthlyCommitments = await CalculateMonthlyCommitmentsAsync(userId, cancellationToken);
        
        // 3. Calcular ingresos recurrentes mensuales
        var monthlyRecurringIncomes = await CalculateMonthlyRecurringIncomesAsync(userId, cancellationToken);
        
        // 4. Calcular gastos variables promedio
        var averageVariableExpenses = await CalculateAverageVariableExpensesAsync(userId, 3, cancellationToken);
        
        // 5. Calcular flujo de caja neto mensual
        var monthlyNetCashFlow = monthlyRecurringIncomes - monthlyCommitments - averageVariableExpenses;
        
        return new FinancialSummary(
            TotalInAllAccounts: totalInAllAccounts,
            MonthlyCommitments: monthlyCommitments,
            MonthlyRecurringIncomes: monthlyRecurringIncomes,
            AverageVariableExpenses: averageVariableExpenses,
            MonthlyNetCashFlow: monthlyNetCashFlow
        );
    }

    // Métodos auxiliares privados

    private decimal ConvertToMonthly(decimal amount, RecurrenceFrequency frequency)
    {
        return frequency switch
        {
            RecurrenceFrequency.Weekly => amount * 4.33m,      // 52 semanas / 12 meses
            RecurrenceFrequency.Biweekly => amount * 2.17m,    // 26 quincenas / 12 meses
            RecurrenceFrequency.Monthly => amount,
            RecurrenceFrequency.Quarterly => amount / 3m,
            RecurrenceFrequency.Yearly => amount / 12m,
            _ => 0m
        };
    }

    private int CalculateMonthsDifference(DateTimeOffset start, DateTimeOffset end)
    {
        var months = ((end.Year - start.Year) * 12) + end.Month - start.Month;
        return Math.Max(0, months); // No permitir meses negativos
    }
}
