using JuliGastos.Application.Interfaces.Handlers.Auth;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.Auth.Logout;

public class LogoutHandler : ILogoutHandler
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LogoutHandler(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LogoutResponse> Handle(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el refresh token en la base de datos
        var token = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken);
        
        if (token == null)
        {
            throw new InvalidRefreshTokenException();
        }

        // 2. Revocar el token
        if (!token.IsRevoked)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTimeOffset.UtcNow;
            await _refreshTokenRepository.UpdateAsync(token);
        }

        // 3. Retornar respuesta
        return new LogoutResponse("Logout successful");
    }
}
