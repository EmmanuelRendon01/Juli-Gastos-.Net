using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.Transaction.Expense;

public record ExpenseCommand(
    long UserId,
    Guid AccountId,
    long CategoryId,
    TransactionType Type,
    decimal Amount,
    DateTime Date,
    string Description,
    NecessityLevel NecessityLevel,
    bool IsRecurring
    
);

