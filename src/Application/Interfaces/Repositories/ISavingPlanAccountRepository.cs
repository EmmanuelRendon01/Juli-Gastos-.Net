using JuliGastos.Domain.Models;

namespace JuliGastos.Application.Interfaces.Repositories;

public interface ISavingPlanAccountRepository
{
    Task<SavingPlanAccount> AddAsync(SavingPlanAccount savingPlanAccount, CancellationToken cancellationToken = default);
    Task<List<SavingPlanAccount>> AddRangeAsync(List<SavingPlanAccount> savingPlanAccounts, CancellationToken cancellationToken = default);
    Task DeleteBySavingPlanIdAsync(long savingPlanId, CancellationToken cancellationToken = default);
    Task<List<SavingPlanAccount>> GetBySavingPlanIdAsync(long savingPlanId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long savingPlanId, long accountId, CancellationToken cancellationToken = default);
}
