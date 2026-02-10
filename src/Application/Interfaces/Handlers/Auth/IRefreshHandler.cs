using JuliGastos.Application.UseCases.Auth.Refresh;

namespace JuliGastos.Application.Interfaces.Handlers.Auth;

public interface IRefreshHandler
{
    Task<RefreshResponse> Handle(
        RefreshCommand command,
        int accessTokenExpirationMinutes = 15,
        int refreshTokenExpirationDays = 7,
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}
