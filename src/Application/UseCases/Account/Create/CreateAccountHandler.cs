using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.Account.Create;

public class CreateAccountHandler
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<CreateAccountResponse> Handle(CreateAccountCommand command, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Validar que no exista otra cuenta con el mismo nombre para el usuario
        if (await _accountRepository.ExistsByNameAndUserIdAsync(command.Name, userId))
        {
            throw new DuplicateAccountNameException(command.Name);
        }

        // 2. Crear la entidad Account
        var account = new Domain.Models.Account
        {
            Uuid = Guid.NewGuid(),
            UserId = userId,
            Name = command.Name,
            Type = command.Type,
            CurrencyCode = command.CurrencyCode.ToUpperInvariant(),
            CurrentBalance = command.InitialBalance,
            MonthlyMaintenanceFee = command.MonthlyMaintenanceFee,
            CreatedAt = DateTimeOffset.UtcNow,
            IsActive = true
        };

        // 3. Guardar en la base de datos
        var savedAccount = await _accountRepository.SaveAsync(account);

        // 4. Retornar respuesta
        return new CreateAccountResponse(
            savedAccount.Uuid,
            savedAccount.Name,
            savedAccount.Type,
            savedAccount.CurrencyCode,
            savedAccount.CurrentBalance,
            savedAccount.CreatedAt
        );
    }
}
