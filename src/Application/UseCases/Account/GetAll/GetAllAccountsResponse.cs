using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.Account.GetAll;

public record AccountDto(
    Guid Uuid,
    string Name,
    AccountType Type,
    string CurrencyCode,
    decimal CurrentBalance,
    decimal MonthlyMaintenanceFee,
    bool IsActive,
    DateTimeOffset CreatedAt
);

public record GetAllAccountsResponse(
    List<AccountDto> Accounts,
    int TotalCount
);
