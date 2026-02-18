namespace JuliGastos.Application.UseCases.FinancialCommitment.Update;

public record UpdateFinancialCommitmentResponse(
    Guid CommitmentUuid,
    string Name,
    string? Description,
    decimal Amount,
    string Frequency,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    bool IsActive,
    DateTimeOffset UpdatedAt
);
