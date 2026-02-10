using JuliGastos.Application.UseCases.Account.GetAll;

namespace JuliGastos.Application.Interfaces.Handlers.Account;

public interface IGetAllAccountsHandler
{
    Task<GetAllAccountsResponse> Handle(long userId);
}
