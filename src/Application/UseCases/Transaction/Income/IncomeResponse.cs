using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.Transaction.Income;

public record IncomeResponse
(
    Guid TransactionUuid,
    TransactionType Type,
    decimal Amount,
    DateTimeOffset Date,
    string Description
);
