namespace JuliGastos.Domain.Exceptions;

public class AccountNotOwnedByUserException : Exception
{
    public AccountNotOwnedByUserException(long accountId, long userId)
        : base($"Account with ID '{accountId}' is not owned by user '{userId}'.")
    {
    }
}
