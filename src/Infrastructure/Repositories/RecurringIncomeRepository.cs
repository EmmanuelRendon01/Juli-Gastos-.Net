using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Models;
using JuliGastos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.Infrastructure.Repositories;

public class RecurringIncomeRepository : IRecurringIncomeRepository
{
    private readonly ApplicationDbContext _context;

    public RecurringIncomeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RecurringIncome> SaveAsync(RecurringIncome income, CancellationToken cancellationToken = default)
    {
        await _context.RecurringIncomes.AddAsync(income, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return income;
    }

    public async Task<RecurringIncome?> GetByUuidAsync(Guid uuid, long userId, CancellationToken cancellationToken = default)
    {
        return await _context.RecurringIncomes
            .Include(ri => ri.Category)
            .FirstOrDefaultAsync(ri => ri.Uuid == uuid && ri.UserId == userId, cancellationToken);
    }

    public async Task<List<RecurringIncome>> GetAllByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.RecurringIncomes
            .Include(ri => ri.Category)
            .Where(ri => ri.UserId == userId)
            .OrderByDescending(ri => ri.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<RecurringIncome>> GetActiveByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.RecurringIncomes
            .Include(ri => ri.Category)
            .Where(ri => ri.UserId == userId && ri.IsActive)
            .OrderBy(ri => ri.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<RecurringIncome> UpdateAsync(RecurringIncome income, CancellationToken cancellationToken = default)
    {
        _context.RecurringIncomes.Update(income);
        await _context.SaveChangesAsync(cancellationToken);
        return income;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var income = await _context.RecurringIncomes
            .FirstOrDefaultAsync(ri => ri.Id == id, cancellationToken);
        
        if (income != null)
        {
            _context.RecurringIncomes.Remove(income);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
