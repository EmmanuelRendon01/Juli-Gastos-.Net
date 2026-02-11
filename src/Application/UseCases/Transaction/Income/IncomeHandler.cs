using JuliGastos.Application.Interfaces.Handlers.Transaction;
using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.Application.UseCases.Transaction.Income;

public class IncomeHandler : IIncomeHandler
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    
    public IncomeHandler(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task<IncomeResponse> Handle(IncomeCommand command, long userId)
    {
        var income = new Domain.Models.Transaction
        {
            Uuid = Guid.NewGuid(),
            UserId = userId,
            AccountId = command.AccountId,
            CategoryId = command.CategoryId,
            Type = Domain.Enums.TransactionType.Income,
            Amount = command.Amount,
            Date = command.Date,
            Description = command.Description,
            NecessityLevel = Domain.Enums.NecessityLevel.Income,
            IsRecurring = command.IsRecurring
        };

        var account = await _accountRepository.GetByUuidAsync(income.AccountId, userId);
        var savedIncome = await _transactionRepository.IncomeAsync(income, account, userId);
        
        account.CurrentBalance += savedIncome.Amount;
        await _accountRepository.UpdateAsync(account);

        return new IncomeResponse(
            savedIncome.Uuid,
            savedIncome.Type,
            savedIncome.Amount,
            savedIncome.Date,
            savedIncome.Description
        );
    }
}