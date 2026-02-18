using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.RecurringIncome.Update;

public record UpdateRecurringIncomeCommand(
    Guid Uuid,
    string Name,
    string? Description,
    decimal Amount,
    RecurrenceFrequency Frequency,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    bool IsActive
);
