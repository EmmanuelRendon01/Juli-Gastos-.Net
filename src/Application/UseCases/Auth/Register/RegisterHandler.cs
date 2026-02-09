using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Domain.Exceptions;
using JuliGastos.Domain.Models;

namespace JuliGastos.Application.UseCases.Auth.Register;

public class RegisterHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public RegisterHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken cancellationToken = default)
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

        // 5. Generar token JWT
        var token = _tokenService.GenerateToken(savedUser.Id, savedUser.Email, savedUser.Role);

        // 6. Retornar respuesta
        return new RegisterResponse(
            savedUser.Id,
            savedUser.Email,
            command.FullName,
            token
        );
    }
}