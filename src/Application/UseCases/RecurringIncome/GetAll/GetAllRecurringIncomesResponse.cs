namespace JuliGastos.Application.UseCases.RecurringIncome.GetAll;

public record RecurringIncomeDto(
    Guid IncomeUuid,
    string Name,
    string? Description,
    decimal Amount,
    string Frequency,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    bool IsActive,
    DateTimeOffset CreatedAt
);

public record GetAllRecurringIncomesResponse(
    List<RecurringIncomeDto> RecurringIncomes,
    int TotalCount
);
