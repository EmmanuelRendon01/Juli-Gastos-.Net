using JuliGastos.Application.UseCases.Account.Create;

namespace JuliGastos.Application.Interfaces.Handlers.Account;

public interface ICreateAccountHandler
{
    Task<CreateAccountResponse> Handle(CreateAccountCommand command, long userId, CancellationToken cancellationToken = default);
}
