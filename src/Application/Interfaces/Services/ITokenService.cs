namespace JuliGastos.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(long userId, string email, string role);
    string GenerateRefreshToken();
    string GenerateToken(long userId, string email, string role);
    long? ValidateToken(string token);
}