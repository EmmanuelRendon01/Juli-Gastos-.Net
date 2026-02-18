namespace JuliGastos.Application.DTOs;

public record SavingCurrentStatus(
    decimal TotalInAccounts,
    decimal MonthlyCommitments,
    decimal AverageVariableExpenses,
    decimal AvailableNow,
    decimal ProgressPercentage
);

public record SavingProjection(
    decimal MonthlyRecurringIncomes,
    decimal MonthlyTotalExpenses,
    decimal MonthlySavingCapacity,
    int MonthsToTarget,
    decimal ProjectedAmount,
    bool WillReachTarget,
    decimal Surplus,
    DateTimeOffset? EstimatedCompletionDate
);

public record LinkedAccountDto(
    Guid Uuid,
    string Name,
    string Type,
    decimal CurrentBalance,
    string CurrencyCode
);

public record FinancialSummary(
    decimal TotalInAllAccounts,
    decimal MonthlyCommitments,
    decimal MonthlyRecurringIncomes,
    decimal AverageVariableExpenses,
    decimal MonthlyNetCashFlow
);
