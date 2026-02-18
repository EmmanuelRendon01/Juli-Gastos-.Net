using JuliGastos.Domain.Models;

namespace JuliGastos.Application.Interfaces.Repositories;

public interface IFinancialCommitmentRepository
{
    Task<FinancialCommitment> SaveAsync(FinancialCommitment commitment, CancellationToken cancellationToken = default);
    Task<FinancialCommitment?> GetByUuidAsync(Guid uuid, long userId, CancellationToken cancellationToken = default);
    Task<List<FinancialCommitment>> GetAllByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<List<FinancialCommitment>> GetActiveByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<FinancialCommitment> UpdateAsync(FinancialCommitment commitment, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
