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
using JuliGastos.Application.UseCases.FinancialCommitment.Create;
using JuliGastos.Application.UseCases.FinancialCommitment.Delete;
using JuliGastos.Application.UseCases.FinancialCommitment.GetAll;
using JuliGastos.Application.UseCases.FinancialCommitment.GetById;
using JuliGastos.Application.UseCases.FinancialCommitment.Update;
using JuliGastos.Application.UseCases.RecurringIncome.Create;
using JuliGastos.Application.UseCases.RecurringIncome.Delete;
using JuliGastos.Application.UseCases.RecurringIncome.GetAll;
using JuliGastos.Application.UseCases.RecurringIncome.GetById;
using JuliGastos.Application.UseCases.RecurringIncome.Update;
using JuliGastos.Application.UseCases.SavingPlan.Create;
using JuliGastos.Application.UseCases.SavingPlan.Delete;
using JuliGastos.Application.UseCases.SavingPlan.GetAll;
using JuliGastos.Application.UseCases.SavingPlan.GetById;
using JuliGastos.Application.UseCases.SavingPlan.GetDashboard;
using JuliGastos.Application.UseCases.SavingPlan.GetProjection;
using JuliGastos.Application.UseCases.SavingPlan.Update;
using JuliGastos.Application.UseCases.SavingPlan.UpdateAccounts;
using JuliGastos.Application.UseCases.SavingPlan.UpdateStatus;
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

        // Financial Commitment Handlers - Fixed expenses management
        services.AddScoped<CreateFinancialCommitmentHandler>();
        services.AddScoped<GetAllFinancialCommitmentsHandler>();
        services.AddScoped<GetFinancialCommitmentByIdHandler>();
        services.AddScoped<UpdateFinancialCommitmentHandler>();
        services.AddScoped<DeleteFinancialCommitmentHandler>();

        // Recurring Income Handlers - Regular income sources
        services.AddScoped<CreateRecurringIncomeHandler>();
        services.AddScoped<GetAllRecurringIncomesHandler>();
        services.AddScoped<GetRecurringIncomeByIdHandler>();
        services.AddScoped<UpdateRecurringIncomeHandler>();
        services.AddScoped<DeleteRecurringIncomeHandler>();

        // Saving Plan Handlers - Savings goals and projections
        services.AddScoped<CreateSavingPlanHandler>();
        services.AddScoped<GetAllSavingPlansHandler>();
        services.AddScoped<GetSavingPlanByIdHandler>();
        services.AddScoped<UpdateSavingPlanHandler>();
        services.AddScoped<DeleteSavingPlanHandler>();
        services.AddScoped<UpdateSavingPlanAccountsHandler>();
        services.AddScoped<GetSavingPlanProjectionHandler>();
        services.AddScoped<GetSavingPlansDashboardHandler>();
        services.AddScoped<UpdateSavingPlanStatusHandler>();

        return services;
    }
}
