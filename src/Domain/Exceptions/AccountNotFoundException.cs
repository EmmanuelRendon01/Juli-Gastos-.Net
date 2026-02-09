namespace JuliGastos.Domain.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(Guid uuid) 
        : base($"Account with UUID {uuid} not found")
    {
    }
}
