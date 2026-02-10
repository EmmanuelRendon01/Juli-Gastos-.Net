namespace JuliGastos.Application.UseCases.Auth.Register;

public record RegisterResponse(
    string Email,
    string FullName,
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiresAt,
    DateTimeOffset RefreshTokenExpiresAt
);
