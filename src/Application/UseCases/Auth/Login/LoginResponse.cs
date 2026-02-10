namespace JuliGastos.Application.UseCases.Auth.Login;

public record LoginResponse(
    long UserId,
    string Email,
    string FullName,
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiresAt,
    DateTimeOffset RefreshTokenExpiresAt
);
