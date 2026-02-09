using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.Application.UseCases.Account.GetAll;

public class GetAllAccountsHandler
{
    private readonly IAccountRepository _accountRepository;

    public GetAllAccountsHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<GetAllAccountsResponse> Handle(long userId, CancellationToken cancellationToken = default)
    {
        // 1. Obtener todas las cuentas del usuario
        var accounts = await _accountRepository.GetAllByUserIdAsync(userId);

        // 2. Mapear a DTOs
        var accountDtos = accounts
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AccountDto(
                a.Uuid,
                a.Name,
                a.Type,
                a.CurrencyCode,
                a.CurrentBalance,
                a.MonthlyMaintenanceFee,
                a.IsActive,
                a.CreatedAt
            ))
            .ToList();

        // 3. Retornar respuesta
        return new GetAllAccountsResponse(accountDtos, accountDtos.Count);
    }
}
