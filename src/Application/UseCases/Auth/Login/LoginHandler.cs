using JuliGastos.Application.Interfaces.Handlers.Auth;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Domain.Exceptions;
using JuliGastos.Domain.Models;

namespace JuliGastos.Application.UseCases.Auth.Login;

public class LoginHandler : ILoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand command, 
        int accessTokenExpirationMinutes = 15,
        int refreshTokenExpirationDays = 7,
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        // 1. Buscar usuario por email (normalizado)
        var user = await _userRepository.GetByEmailAsync(command.Email.ToLowerInvariant());

        // 2. Validar que el usuario exista
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        // 3. Verificar la contraseña
        if (!_passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        // 4. Generar access token y refresh token
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        var accessTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(accessTokenExpirationMinutes);
        var refreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(refreshTokenExpirationDays);

        // 5. Verificar límite de refresh tokens activos (máximo 5)
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

        // 6. Guardar refresh token en la base de datos
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = refreshTokenExpiresAt,
            IpAddress = ipAddress ?? string.Empty,
            UserAgent = userAgent ?? string.Empty
        };

        await _refreshTokenRepository.SaveAsync(refreshTokenEntity);

        // 7. Retornar respuesta
        return new LoginResponse(
            user.Id,
            user.Email,
            user.FullName,
            accessToken,
            refreshToken,
            accessTokenExpiresAt,
            refreshTokenExpiresAt
        );
    }
}
