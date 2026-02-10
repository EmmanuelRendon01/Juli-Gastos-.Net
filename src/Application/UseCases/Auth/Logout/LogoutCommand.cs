namespace JuliGastos.Application.UseCases.Auth.Logout;

public record LogoutCommand(
    string RefreshToken
);
