namespace JuliGastos.Domain.Exceptions;

public class FinancialCommitmentNotFoundException : Exception
{
    public FinancialCommitmentNotFoundException(Guid uuid)
        : base($"Financial commitment with UUID '{uuid}' was not found.")
    {
    }
}
