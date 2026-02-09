namespace JuliGastos.Domain.Exceptions;

public class AccountsOwnershipException : Exception
{
    public AccountsOwnershipException(string message) : base(message)
    {
    }
}
