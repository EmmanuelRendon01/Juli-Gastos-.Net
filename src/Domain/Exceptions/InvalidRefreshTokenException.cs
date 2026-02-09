namespace JuliGastos.Domain.Exceptions;

public class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException() 
        : base("Invalid refresh token")
    {
    }
}
