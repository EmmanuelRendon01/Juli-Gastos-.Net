using JuliGastos.Application.UseCases.Auth.Login;

namespace JuliGastos.Application.Interfaces.Handlers.Auth;

public interface ILoginHandler
{
    Task<LoginResponse> Handle(
        LoginCommand command, 
        int accessTokenExpirationMinutes = 15,
        int refreshTokenExpirationDays = 7,
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}
