using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.Transaction.Expense;

public record ExpenseResponse(
    Guid TransactionUuid,
    TransactionType Type,
    decimal Amount,
    DateTimeOffset Date,
    string Description
);

    
            

