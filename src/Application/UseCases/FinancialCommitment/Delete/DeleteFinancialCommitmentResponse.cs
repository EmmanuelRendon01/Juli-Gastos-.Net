namespace JuliGastos.Application.UseCases.FinancialCommitment.Delete;

public record DeleteFinancialCommitmentResponse(
    Guid CommitmentUuid,
    string Message
);
