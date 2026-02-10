using JuliGastos.Application.Interfaces.Handlers.Auth;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Domain.Exceptions;
using JuliGastos.Domain.Models;

namespace JuliGastos.Application.UseCases.Auth.Refresh;

public class RefreshHandler : IRefreshHandler
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RefreshHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<RefreshResponse> Handle(
        RefreshCommand command,
        int accessTokenExpirationMinutes = 15,
        int refreshTokenExpirationDays = 7,
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        // 1. Buscar el refresh token en la base de datos
        var existingToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken);
        
        if (existingToken == null)
        {
            throw new InvalidRefreshTokenException();
        }

        // 2. Validar que no esté expirado
        if (existingToken.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            throw new RefreshTokenExpiredException();
        }

        // 3. Validar que no esté revocado
        if (existingToken.IsRevoked)
        {
            // ROTACIÓN: Si el token ya fue usado, revocar todos los tokens del usuario (posible ataque)
            if (!string.IsNullOrEmpty(existingToken.ReplacedByToken))
            {
                await _refreshTokenRepository.RevokeAllByUserIdAsync(existingToken.UserId);
            }
            throw new RefreshTokenRevokedException();
        }

        // 4. Obtener el usuario asociado
        var user = await _userRepository.GetByIdAsync(existingToken.UserId);
        
        if (user == null)
        {
            throw new InvalidRefreshTokenException();
        }

        // 5. Generar nuevos tokens
        var newAccessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.Role);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        
        var accessTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(accessTokenExpirationMinutes);
        var refreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(refreshTokenExpirationDays);

        // 6. ROTACIÓN: Revocar el token anterior y marcarlo como reemplazado
        existingToken.IsRevoked = true;
        existingToken.RevokedAt = DateTimeOffset.UtcNow;
        existingToken.ReplacedByToken = newRefreshToken;
        await _refreshTokenRepository.UpdateAsync(existingToken);

        // 7. Verificar límite de refresh tokens activos (máximo 5)
        var activeTokensCount = await _refreshTokenRepository.CountActiveByUserIdAsync(user.Id);
        if (activeTokensCount >= 5)
        {
            // Revocar el refresh token más antiguo
            var oldestToken = await _refreshTokenRepository.GetOldestByUserIdAsync(user.Id);
            if (oldestToken != null)
            {
                oldestToken.IsRevoked = true;
                oldestToken.RevokedAt = DateTimeOffset.UtcNow;
                await _refreshTokenRepository.UpdateAsync(oldestToken);
            }
        }

        // 8. Guardar el nuevo refresh token
        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            UserId = user.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = refreshTokenExpiresAt,
            IpAddress = ipAddress ?? string.Empty,
            UserAgent = userAgent ?? string.Empty
        };

        await _refreshTokenRepository.SaveAsync(newRefreshTokenEntity);

        // 9. Retornar respuesta con nuevos tokens
        return new RefreshResponse(
            newAccessToken,
            newRefreshToken,
            accessTokenExpiresAt,
            refreshTokenExpiresAt
        );
    }
}
