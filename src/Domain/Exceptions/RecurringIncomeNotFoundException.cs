namespace JuliGastos.Domain.Exceptions;

public class RecurringIncomeNotFoundException : Exception
{
    public RecurringIncomeNotFoundException(Guid uuid)
        : base($"Recurring income with UUID '{uuid}' was not found.")
    {
    }
}
