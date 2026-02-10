using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.Transaction.Expense;

public record ExpenseCommand(
    Guid AccountId,
    long CategoryId,
    decimal Amount,
    DateTime Date,
    string Description,
    bool IsRecurring
    
);

