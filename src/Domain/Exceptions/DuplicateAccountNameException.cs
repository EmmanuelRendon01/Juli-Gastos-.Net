namespace JuliGastos.Domain.Exceptions;

public class DuplicateAccountNameException : Exception
{
    public DuplicateAccountNameException(string name) 
        : base($"An account with name '{name}' already exists")
    {
    }
}
