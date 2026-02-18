using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.RecurringIncome.Create;

public record CreateRecurringIncomeCommand(
    string Name,
    string? Description,
    decimal Amount,
    RecurrenceFrequency Frequency,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate
);
