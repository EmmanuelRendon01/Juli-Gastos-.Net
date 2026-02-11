using JuliGastos.Application.Interfaces.Handlers.Account;
using JuliGastos.Application.Interfaces.Handlers.Auth;
using JuliGastos.Application.Interfaces.Handlers.Transaction;
using JuliGastos.Application.UseCases.Account.Create;
using JuliGastos.Application.UseCases.Account.GetAll;
using JuliGastos.Application.UseCases.Account.GetById;
using JuliGastos.Application.UseCases.Auth.Login;
using JuliGastos.Application.UseCases.Auth.Logout;
using JuliGastos.Application.UseCases.Auth.Refresh;
using JuliGastos.Application.UseCases.Auth.Register;
using JuliGastos.Application.UseCases.Transaction.Expense;
using JuliGastos.Application.UseCases.Transaction.Income;

namespace JuliGastos.API.Extensions;

/// <summary>
/// Extension methods for configuring Application layer services
/// Includes: Use Case Handlers (Auth, Account, Transaction)
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Adds all Application layer use case handlers organized by feature
    /// </summary>
    public static IServiceCollection AddApplicationHandlers(this IServiceCollection services)
    {
        // Auth Handlers - User authentication and authorization
        services.AddScoped<IRegisterHandler, RegisterHandler>();
        services.AddScoped<ILoginHandler, LoginHandler>();
        services.AddScoped<IRefreshHandler, RefreshHandler>();
        services.AddScoped<ILogoutHandler, LogoutHandler>();

        // Account Handlers - Financial accounts management
        services.AddScoped<ICreateAccountHandler, CreateAccountHandler>();
        services.AddScoped<IGetAllAccountsHandler, GetAllAccountsHandler>();
        services.AddScoped<IGetAccountByIdHandler, GetAccountByIdHandler>();

        // Transaction Handlers - Income and expense tracking
        services.AddScoped<IExpenseHandler, ExpenseHandler>();
        services.AddScoped<IIncomeHandler, IncomeHandler>();

        return services;
    }
}
