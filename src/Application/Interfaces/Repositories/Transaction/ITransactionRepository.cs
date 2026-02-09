using JuliGastos.Domain.Models;

namespace JuliGastos.Application.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> GetByUuidAsync(Guid uuid, long userId);
    Task<List<Transaction>> GetAllByUserIdAsync(long userId);
    Task<Transaction> ExpenseAsync(Transaction transaction, Account account, long userId);
    Task<Transaction> IncomeAsync(Transaction transaction, Account account, long userId);
    Task<Transaction> TransferAsync(Transaction transaction, Account account, long userId);

}