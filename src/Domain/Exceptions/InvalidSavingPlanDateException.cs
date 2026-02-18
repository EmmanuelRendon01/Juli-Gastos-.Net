namespace JuliGastos.Domain.Exceptions;

public class InvalidSavingPlanDateException : Exception
{
    public InvalidSavingPlanDateException()
        : base("Target date must be in the future.")
    {
    }
    
    public InvalidSavingPlanDateException(string message)
        : base(message)
    {
    }
}
