using JuliGastos.Application.Interfaces.Handlers.Account;
using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.Application.UseCases.Account.GetAll;

public class GetAllAccountsHandler : IGetAllAccountsHandler
{
    private readonly IAccountRepository _accountRepository;

    public GetAllAccountsHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<GetAllAccountsResponse> Handle(long userId)
    {
        var accounts = await _accountRepository.GetAllByUserIdAsync(userId);
        
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
        
        return new GetAllAccountsResponse(accountDtos, accountDtos.Count);
    }
}
