namespace JuliGastos.Application.UseCases.FinancialCommitment.Create;

public record CreateFinancialCommitmentResponse(
    Guid Uuid,
    string Name,
    decimal Amount,
    string Frequency,
    long? CategoryId,
    string? CategoryName,
    DateTimeOffset? NextDueDate,
    bool IsActive,
    string? Notes,
    DateTimeOffset CreatedAt
);
