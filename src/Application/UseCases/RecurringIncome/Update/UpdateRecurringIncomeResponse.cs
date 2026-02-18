namespace JuliGastos.Application.UseCases.RecurringIncome.Update;

public record UpdateRecurringIncomeResponse(
    Guid IncomeUuid,
    string Name,
    string? Description,
    decimal Amount,
    string Frequency,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    bool IsActive,
    DateTimeOffset UpdatedAt
);
