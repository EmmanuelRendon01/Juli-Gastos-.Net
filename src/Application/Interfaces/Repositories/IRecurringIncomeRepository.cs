using JuliGastos.Domain.Models;

namespace JuliGastos.Application.Interfaces.Repositories;

public interface IRecurringIncomeRepository
{
    Task<RecurringIncome> SaveAsync(RecurringIncome income, CancellationToken cancellationToken = default);
    Task<RecurringIncome?> GetByUuidAsync(Guid uuid, long userId, CancellationToken cancellationToken = default);
    Task<List<RecurringIncome>> GetAllByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<List<RecurringIncome>> GetActiveByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<RecurringIncome> UpdateAsync(RecurringIncome income, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
