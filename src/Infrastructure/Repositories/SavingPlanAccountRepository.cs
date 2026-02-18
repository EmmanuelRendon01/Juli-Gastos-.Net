using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Models;
using JuliGastos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.Infrastructure.Repositories;

public class SavingPlanAccountRepository : ISavingPlanAccountRepository
{
    private readonly ApplicationDbContext _context;

    public SavingPlanAccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SavingPlanAccount> AddAsync(SavingPlanAccount savingPlanAccount, CancellationToken cancellationToken = default)
    {
        await _context.SavingPlanAccounts.AddAsync(savingPlanAccount, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return savingPlanAccount;
    }

    public async Task<List<SavingPlanAccount>> AddRangeAsync(List<SavingPlanAccount> savingPlanAccounts, CancellationToken cancellationToken = default)
    {
        await _context.SavingPlanAccounts.AddRangeAsync(savingPlanAccounts, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return savingPlanAccounts;
    }

    public async Task DeleteBySavingPlanIdAsync(long savingPlanId, CancellationToken cancellationToken = default)
    {
        var accounts = await _context.SavingPlanAccounts
            .Where(spa => spa.SavingPlanId == savingPlanId)
            .ToListAsync(cancellationToken);
        
        _context.SavingPlanAccounts.RemoveRange(accounts);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<SavingPlanAccount>> GetBySavingPlanIdAsync(long savingPlanId, CancellationToken cancellationToken = default)
    {
        return await _context.SavingPlanAccounts
            .Include(spa => spa.Account)
            .Where(spa => spa.SavingPlanId == savingPlanId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(long savingPlanId, long accountId, CancellationToken cancellationToken = default)
    {
        return await _context.SavingPlanAccounts
            .AnyAsync(spa => spa.SavingPlanId == savingPlanId && spa.AccountId == accountId, cancellationToken);
    }
}
