namespace JuliGastos.Domain.Exceptions;

public class RefreshTokenRevokedException : Exception
{
    public RefreshTokenRevokedException() 
        : base("Refresh token has been revoked")
    {
    }
}
