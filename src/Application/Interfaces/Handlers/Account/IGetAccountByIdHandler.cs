using JuliGastos.Application.UseCases.Account.GetAll;

namespace JuliGastos.Application.Interfaces.Handlers.Account;

public interface IGetAccountByIdHandler
{
    Task<AccountDto> Handle(Guid accountUuid, long userId, CancellationToken cancellationToken = default);
}
