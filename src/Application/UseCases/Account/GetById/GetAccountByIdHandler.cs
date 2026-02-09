using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.UseCases.Account.GetAll;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.Account.GetById;

public class GetAccountByIdHandler
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountByIdHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> Handle(Guid accountUuid, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar cuenta por UUID y validar que pertenece al usuario
        var account = await _accountRepository.GetByUuidAsync(accountUuid, userId);

        // 2. Si no existe o no es del usuario, lanzar excepción
        if (account == null)
        {
            throw new AccountNotFoundException(accountUuid);
        }

        // 3. Mapear a DTO y retornar
        return new AccountDto(
            account.Uuid,
            account.Name,
            account.Type,
            account.CurrencyCode,
            account.CurrentBalance,
            account.MonthlyMaintenanceFee,
            account.IsActive,
            account.CreatedAt
        );
    }
}
