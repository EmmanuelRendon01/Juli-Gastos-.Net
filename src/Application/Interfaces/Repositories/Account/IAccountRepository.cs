using JuliGastos.Domain.Models;

namespace JuliGastos.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByUuidAsync(Guid uuid, long userId);
    Task<List<Account>> GetAllByUserIdAsync(long userId);
    Task<bool> ExistsByNameAndUserIdAsync(string name, long userId);
    Task<Account> SaveAsync(Account account);
    Task<Account> UpdateAsync(Account account);
}
