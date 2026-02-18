namespace JuliGastos.Application.UseCases.FinancialCommitment.GetById;

public record GetFinancialCommitmentByIdResponse(
    Guid Uuid,
    string Name,
    decimal Amount,
    string Frequency,
    long? CategoryId,
    string? CategoryName,
    string? CategoryIcon,
    string? CategoryColor,
    DateTimeOffset? NextDueDate,
    bool IsActive,
    string? Notes,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
