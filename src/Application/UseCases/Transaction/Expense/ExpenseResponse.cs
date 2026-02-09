using JuliGastos.Domain.Enums;
namespace JuliGastos.Application.UseCases.Transaction.Expense;

public class ExpenseResponse(
    TransactionType Type,
    decimal  Amount,
    DateTimeOffset Date,
    string Description
);

    
            

