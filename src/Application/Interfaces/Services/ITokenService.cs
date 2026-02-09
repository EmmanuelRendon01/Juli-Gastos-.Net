namespace JuliGastos.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(long userId, string email, string role);
    long? ValidateToken(string token);
}