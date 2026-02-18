using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.FinancialCommitment.Create;

public record CreateFinancialCommitmentCommand(
    string Name,
    decimal Amount,
    RecurrenceFrequency Frequency,
    long? CategoryId = null,
    DateTimeOffset? NextDueDate = null,
    string? Notes = null
);
