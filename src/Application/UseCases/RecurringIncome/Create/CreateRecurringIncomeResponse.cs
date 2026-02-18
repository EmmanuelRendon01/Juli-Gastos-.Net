namespace JuliGastos.Application.UseCases.RecurringIncome.Create;

public record CreateRecurringIncomeResponse(
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
