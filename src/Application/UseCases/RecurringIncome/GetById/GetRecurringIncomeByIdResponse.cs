namespace JuliGastos.Application.UseCases.RecurringIncome.GetById;

public record GetRecurringIncomeByIdResponse(
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
