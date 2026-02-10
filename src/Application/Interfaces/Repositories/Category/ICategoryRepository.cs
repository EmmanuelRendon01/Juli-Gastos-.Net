using JuliGastos.Domain.Models;
using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(long id);
    Task<List<Category>> GetAllSystemCategoriesAsync();
    Task<List<Category>> GetAllByUserIdAsync(long userId);
    Task<List<Category>> GetAllCategoriesForUserAsync(long userId);
    Task<List<Category>> GetCategoriesByTypeAsync(TransactionType type, long? userId = null);
    Task<bool> ExistsByNameAndUserIdAsync(string name, long userId);
    Task<Category> SaveAsync(Category category);
    Task<Category> UpdateAsync(Category category);
    Task DeleteAsync(long id);
}
