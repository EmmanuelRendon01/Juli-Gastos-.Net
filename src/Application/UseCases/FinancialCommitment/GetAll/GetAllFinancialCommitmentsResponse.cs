namespace JuliGastos.Application.UseCases.FinancialCommitment.GetAll;

public record GetAllFinancialCommitmentsResponse(
    List<FinancialCommitmentDto> Commitments,
    int TotalCount,
    int ActiveCount
);

public record FinancialCommitmentDto(
    Guid Uuid,
    string Name,
    decimal Amount,
    string Frequency,
    long? CategoryId,
    string? CategoryName,
    string? CategoryIcon,
    DateTimeOffset? NextDueDate,
    bool IsActive,
    string? Notes,
    DateTimeOffset CreatedAt
);
