namespace JuliGastos.Application.UseCases.Auth.Register;

public record RegisterResponse(
    long UserId,
    string Email,
    string FullName,
    string Token
);
