using JuliGastos.Application.UseCases.Transaction.Expense;

namespace JuliGastos.Application.Interfaces.Handlers.Transaction;

public interface IExpenseHandler
{
    Task<ExpenseResponse> Handle(ExpenseCommand command, long userId);
}
