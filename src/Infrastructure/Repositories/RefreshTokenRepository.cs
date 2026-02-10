using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Models;
using JuliGastos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task<List<RefreshToken>> GetActiveByUserIdAsync(long userId)
    {
        return await _context.RefreshTokens
            .Where(rt => rt.UserId == userId 
                && !rt.IsRevoked 
                && rt.ExpiresAt > DateTimeOffset.UtcNow)
            .OrderByDescending(rt => rt.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> CountActiveByUserIdAsync(long userId)
    {
        return await _context.RefreshTokens
            .CountAsync(rt => rt.UserId == userId 
                && !rt.IsRevoked 
                && rt.ExpiresAt > DateTimeOffset.UtcNow);
    }

    public async Task<RefreshToken> SaveAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken> UpdateAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task RevokeAllByUserIdAsync(long userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();
        
        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTimeOffset.UtcNow;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpiredAsync()
    {
        var expiredTokens = await _context.RefreshTokens
            .Where(rt => rt.ExpiresAt < DateTimeOffset.UtcNow)
            .ToListAsync();
        
        _context.RefreshTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetOldestByUserIdAsync(long userId)
    {
        return await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .OrderBy(rt => rt.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
