using JuliGastos.Application.DTOs;

namespace JuliGastos.Application.Interfaces.Services;

public interface ISavingCalculationService
{
    /// <summary>
    /// Calcula el ahorro actual sumando los balances de las cuentas vinculadas a un plan
    /// </summary>
    Task<decimal> CalculateCurrentSavingsAsync(long savingPlanId, long userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Calcula el total de gastos comprometidos mensualizados del usuario
    /// </summary>
    Task<decimal> CalculateMonthlyCommitmentsAsync(long userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Calcula el total de ingresos recurrentes mensualizados del usuario
    /// </summary>
    Task<decimal> CalculateMonthlyRecurringIncomesAsync(long userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Calcula el promedio de gastos variables (transacciones de tipo Expense no recurrentes)
    /// </summary>
    Task<decimal> CalculateAverageVariableExpensesAsync(long userId, int months = 3, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Calcula el estado actual del ahorro (total en cuentas, disponible neto, progreso)
    /// </summary>
    Task<SavingCurrentStatus> CalculateCurrentStatusAsync(long savingPlanId, decimal targetAmount, long userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Calcula la proyección completa para un plan de ahorro
    /// </summary>
    Task<SavingProjection> CalculateProjectionAsync(long savingPlanId, decimal targetAmount, DateTimeOffset targetDate, long userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Calcula el resumen financiero general del usuario
    /// </summary>
    Task<FinancialSummary> CalculateFinancialSummaryAsync(long userId, CancellationToken cancellationToken = default);
}
