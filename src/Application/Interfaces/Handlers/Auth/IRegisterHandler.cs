using JuliGastos.Application.UseCases.Auth.Register;

namespace JuliGastos.Application.Interfaces.Handlers.Auth;

public interface IRegisterHandler
{
    Task<RegisterResponse> Handle(
        RegisterCommand command,
        int accessTokenExpirationMinutes = 15,
        int refreshTokenExpirationDays = 7,
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}
