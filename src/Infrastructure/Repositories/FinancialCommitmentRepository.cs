using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Models;
using JuliGastos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.Infrastructure.Repositories;

public class FinancialCommitmentRepository : IFinancialCommitmentRepository
{
    private readonly ApplicationDbContext _context;

    public FinancialCommitmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinancialCommitment> SaveAsync(FinancialCommitment commitment, CancellationToken cancellationToken = default)
    {
        await _context.FinancialCommitments.AddAsync(commitment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return commitment;
    }

    public async Task<FinancialCommitment?> GetByUuidAsync(Guid uuid, long userId, CancellationToken cancellationToken = default)
    {
        return await _context.FinancialCommitments
            .Include(fc => fc.Category)
            .FirstOrDefaultAsync(fc => fc.Uuid == uuid && fc.UserId == userId, cancellationToken);
    }

    public async Task<List<FinancialCommitment>> GetAllByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.FinancialCommitments
            .Include(fc => fc.Category)
            .Where(fc => fc.UserId == userId)
            .OrderByDescending(fc => fc.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<FinancialCommitment>> GetActiveByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.FinancialCommitments
            .Include(fc => fc.Category)
            .Where(fc => fc.UserId == userId && fc.IsActive)
            .OrderBy(fc => fc.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<FinancialCommitment> UpdateAsync(FinancialCommitment commitment, CancellationToken cancellationToken = default)
    {
        _context.FinancialCommitments.Update(commitment);
        await _context.SaveChangesAsync(cancellationToken);
        return commitment;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var commitment = await _context.FinancialCommitments
            .FirstOrDefaultAsync(fc => fc.Id == id, cancellationToken);
        
        if (commitment != null)
        {
            _context.FinancialCommitments.Remove(commitment);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
