namespace JuliGastos.Domain.Exceptions;

public class SavingPlanNotFoundException : Exception
{
    public SavingPlanNotFoundException(Guid uuid)
        : base($"Saving plan with UUID '{uuid}' was not found.")
    {
    }
}
