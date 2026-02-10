using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.Account.Create;

public record CreateAccountResponse(
    Guid AccountUuid,
    string Name,
    AccountType Type,
    string CurrencyCode,
    decimal CurrentBalance,
    DateTimeOffset CreatedAt
);
