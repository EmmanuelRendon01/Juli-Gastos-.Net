using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Models;
using JuliGastos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.Infrastructure.Repositories;

public class SavingPlanRepository : ISavingPlanRepository
{
    private readonly ApplicationDbContext _context;

    public SavingPlanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SavingPlan> SaveAsync(SavingPlan savingPlan, CancellationToken cancellationToken = default)
    {
        await _context.SavingPlans.AddAsync(savingPlan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return savingPlan;
    }

    public async Task<SavingPlan?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.SavingPlans
            .FirstOrDefaultAsync(sp => sp.Id == id, cancellationToken);
    }

    public async Task<SavingPlan?> GetByUuidAsync(Guid uuid, long userId, CancellationToken cancellationToken = default)
    {
        return await _context.SavingPlans
            .FirstOrDefaultAsync(sp => sp.Uuid == uuid && sp.UserId == userId, cancellationToken);
    }

    public async Task<SavingPlan?> GetByUuidWithAccountsAsync(Guid uuid, long userId, CancellationToken cancellationToken = default)
    {
        return await _context.SavingPlans
            .Include(sp => sp.SavingPlanAccounts)
                .ThenInclude(spa => spa.Account)
            .FirstOrDefaultAsync(sp => sp.Uuid == uuid && sp.UserId == userId, cancellationToken);
    }

    public async Task<List<SavingPlan>> GetAllByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.SavingPlans
            .Where(sp => sp.UserId == userId)
            .OrderByDescending(sp => sp.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SavingPlan>> GetAllByUserIdWithAccountsAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.SavingPlans
            .Include(sp => sp.SavingPlanAccounts)
                .ThenInclude(spa => spa.Account)
            .Where(sp => sp.UserId == userId)
            .OrderByDescending(sp => sp.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<SavingPlan> UpdateAsync(SavingPlan savingPlan, CancellationToken cancellationToken = default)
    {
        _context.SavingPlans.Update(savingPlan);
        await _context.SaveChangesAsync(cancellationToken);
        return savingPlan;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var savingPlan = await GetByIdAsync(id, cancellationToken);
        if (savingPlan != null)
        {
            _context.SavingPlans.Remove(savingPlan);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsByUuidAsync(Guid uuid, long userId, CancellationToken cancellationToken = default)
    {
        return await _context.SavingPlans
            .AnyAsync(sp => sp.Uuid == uuid && sp.UserId == userId, cancellationToken);
    }
}
