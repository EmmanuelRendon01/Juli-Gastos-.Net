using JuliGastos.Domain.Models;

namespace JuliGastos.Application.Interfaces.Repositories;

public interface ISavingPlanRepository
{
    Task<SavingPlan> SaveAsync(SavingPlan savingPlan, CancellationToken cancellationToken = default);
    Task<SavingPlan?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<SavingPlan?> GetByUuidAsync(Guid uuid, long userId, CancellationToken cancellationToken = default);
    Task<SavingPlan?> GetByUuidWithAccountsAsync(Guid uuid, long userId, CancellationToken cancellationToken = default);
    Task<List<SavingPlan>> GetAllByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<List<SavingPlan>> GetAllByUserIdWithAccountsAsync(long userId, CancellationToken cancellationToken = default);
    Task<SavingPlan> UpdateAsync(SavingPlan savingPlan, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUuidAsync(Guid uuid, long userId, CancellationToken cancellationToken = default);
}
