using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.Transaction.Expense;

public class ExpenseHandler
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    
    public ExpenseHandler(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task<ExpenseResponse> Handle(ExpenseCommand command, long userId)
    {
        var expense = new Domain.Models.Transaction
        {
            UserId = userId,
            AccountId = command.AccountId,
            CategoryId = command.CategoryId,
            Type = command.Type,
            Amount =  command.Amount,
            Date = command.Date,
            Description = command.Description,
            NecessityLevel = command.NecessityLevel,
            IsRecurring = command.IsRecurring
        };

        var account = await _accountRepository.GetByUuidAsync(expense.AccountId, userId);
        var savedExpense = await _transactionRepository.ExpenseAsync(expense, account, userId);
        
        account.CurrentBalance -= savedExpense.Amount;

        return new ExpenseResponse(
            savedExpense.Type,
            savedExpense.Amount,
            savedExpense.Date,
            savedExpense.Description
        );


    }
}

