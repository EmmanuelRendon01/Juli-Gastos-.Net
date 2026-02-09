namespace JuliGastos.Domain.Exceptions;

public class InvalidTransferException : Exception
{
    public InvalidTransferException(string message) : base(message)
    {
    }
}
