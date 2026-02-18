using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.FinancialCommitment.Update;

public record UpdateFinancialCommitmentCommand(
    Guid Uuid,
    string Name,
    string? Description,
    decimal Amount,
    RecurrenceFrequency Frequency,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    bool IsActive
);
