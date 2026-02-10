using JuliGastos.Application.Interfaces.Handlers.Transaction;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Enums;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.Transaction.Expense;

public class ExpenseHandler : IExpenseHandler
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
            Uuid = Guid.NewGuid(),
            UserId = userId,
            AccountId = command.AccountId,
            CategoryId = command.CategoryId,
            Type = TransactionType.Expense,
            Amount = command.Amount,
            Date = command.Date,
            Description = command.Description,
            NecessityLevel = NecessityLevel.Expense,
            IsRecurring = command.IsRecurring
        };

        var account = await _accountRepository.GetByUuidAsync(expense.AccountId, userId);
        var savedExpense = await _transactionRepository.ExpenseAsync(expense, account, userId);
        
        account.CurrentBalance -= savedExpense.Amount;
        await _accountRepository.UpdateAsync(account);

        return new ExpenseResponse(
            savedExpense.Uuid,
            savedExpense.Type,
            savedExpense.Amount,
            savedExpense.Date,
            savedExpense.Description
        );
    }
}

