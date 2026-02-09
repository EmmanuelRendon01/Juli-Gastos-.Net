using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Models;
using JuliGastos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByUuidAsync(Guid uuid, long userId)
    {
        return await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Uuid == uuid && a.UserId == userId);
    }

    public async Task<List<Account>> GetAllByUserIdAsync(long userId)
    {
        return await _context.Accounts
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAndUserIdAsync(string name, long userId)
    {
        return await _context.Accounts
            .AnyAsync(a => a.Name.ToLower() == name.ToLower() && a.UserId == userId);
    }

    public async Task<Account> SaveAsync(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<Account> UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
        return account;
    }
}
