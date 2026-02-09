using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Models;
using JuliGastos.Infrastructure.Persistence;
namespace JuliGastos.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;
    
    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Transaction> GetByUuidAsync(Guid uuid, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Transaction>> GetAllByUserIdAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Transaction> ExpenseAsync(Transaction transaction, Account account, long userId)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public Task<Transaction> IncomeAsync(Transaction transaction, Account account, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction> TransferAsync(Transaction transaction, Account account, long userId)
    {
        throw new NotImplementedException();
    }
}
