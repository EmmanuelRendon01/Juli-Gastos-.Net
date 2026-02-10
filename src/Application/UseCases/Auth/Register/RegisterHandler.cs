using JuliGastos.Application.Interfaces.Handlers.Auth;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Domain.Exceptions;
using JuliGastos.Domain.Models;

namespace JuliGastos.Application.UseCases.Auth.Register;

public class RegisterHandler : IRegisterHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public RegisterHandler(
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

    public async Task<RegisterResponse> Handle(
        RegisterCommand command,
        int accessTokenExpirationMinutes = 15,
        int refreshTokenExpirationDays = 7,
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        // 1. Validar que el email no esté registrado
        if (await _userRepository.ExistsByEmailAsync(command.Email))
        {
            throw new DuplicateEmailException($"Email {command.Email} is already registered");
        }

        // 2. Hashear la contraseña
        var passwordHash = _passwordHasher.Hash(command.Password);

        // 3. Crear la entidad User
        var user = new User
        {
            Uuid = Guid.NewGuid(),
            Email = command.Email.ToLowerInvariant(), // Normalizar email
            PasswordHash = passwordHash,
            FullName = command.FullName,
            Role = "User", // Rol por defecto
            CurrencyCode = command.CurrencyCode,
            EmergencyFundMonths = command.EmergencyFundMonths,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // 4. Guardar en la base de datos
        var savedUser = await _userRepository.SaveAsync(user);

        // 5. Generar access token y refresh token
        var accessToken = _tokenService.GenerateAccessToken(savedUser.Id, savedUser.Email, savedUser.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        var accessTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(accessTokenExpirationMinutes);
        var refreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(refreshTokenExpirationDays);

        // 6. Guardar refresh token en la base de datos
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = savedUser.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = refreshTokenExpiresAt,
            IpAddress = ipAddress ?? string.Empty,
            UserAgent = userAgent ?? string.Empty
        };

        await _refreshTokenRepository.SaveAsync(refreshTokenEntity);

        // 7. Retornar respuesta
        return new RegisterResponse(
            savedUser.Email,
            savedUser.FullName,
            accessToken,
            refreshToken,
            accessTokenExpiresAt,
            refreshTokenExpiresAt
        );
    }
}
