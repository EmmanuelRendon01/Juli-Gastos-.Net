using JuliGastos.Domain.Models;

namespace JuliGastos.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<List<RefreshToken>> GetActiveByUserIdAsync(long userId);
    Task<int> CountActiveByUserIdAsync(long userId);
    Task<RefreshToken> SaveAsync(RefreshToken refreshToken);
    Task<RefreshToken> UpdateAsync(RefreshToken refreshToken);
    Task RevokeAllByUserIdAsync(long userId);
    Task DeleteExpiredAsync();
    Task<RefreshToken?> GetOldestByUserIdAsync(long userId);
}
