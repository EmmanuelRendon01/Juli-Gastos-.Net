namespace JuliGastos.Application.UseCases.Auth.Login;

public record LoginResponse(
    long UserId,
    string Email,
    string FullName,
    string Token
);
