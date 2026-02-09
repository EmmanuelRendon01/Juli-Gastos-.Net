using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.Account.Create;

public record CreateAccountCommand(
    string Name,
    AccountType Type,
    string CurrencyCode,
    decimal InitialBalance,
    decimal MonthlyMaintenanceFee = 0
);
