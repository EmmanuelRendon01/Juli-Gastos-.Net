using JuliGastos.Application.UseCases.Auth.Logout;

namespace JuliGastos.Application.Interfaces.Handlers.Auth;

public interface ILogoutHandler
{
    Task<LogoutResponse> Handle(LogoutCommand command, CancellationToken cancellationToken = default);
}
