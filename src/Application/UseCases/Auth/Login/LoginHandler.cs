using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.Auth.Login;

public class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken = default)
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

        // 4. Generar token JWT
        var token = _tokenService.GenerateToken(user.Id, user.Email, user.Role);

        // 5. Retornar respuesta
        return new LoginResponse(
            user.Id,
            user.Email,
            user.FullName,
            token
        );
    }
}
