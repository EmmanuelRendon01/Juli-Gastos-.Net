using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Enums;
using JuliGastos.Domain.Models;
using JuliGastos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(long id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Category>> GetAllSystemCategoriesAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.IsSystemDefault && c.UserId == null)
            .OrderBy(c => c.Type)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<List<Category>> GetAllByUserIdAsync(long userId)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Type)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<List<Category>> GetAllCategoriesForUserAsync(long userId)
    {
        // Returns system categories + user's custom categories
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.UserId == null || c.UserId == userId)
            .OrderBy(c => c.Type)
            .ThenBy(c => c.IsSystemDefault ? 0 : 1) // System categories first
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<List<Category>> GetCategoriesByTypeAsync(TransactionType type, long? userId = null)
    {
        var query = _context.Categories
            .AsNoTracking()
            .Where(c => c.Type == type);

        if (userId.HasValue)
        {
            // Include system categories and user's custom categories
            query = query.Where(c => c.UserId == null || c.UserId == userId.Value);
        }
        else
        {
            // Only system categories
            query = query.Where(c => c.UserId == null);
        }

        return await query
            .OrderBy(c => c.IsSystemDefault ? 0 : 1) // System categories first
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAndUserIdAsync(string name, long userId)
    {
        return await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.UserId == userId);
    }

    public async Task<Category> SaveAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task DeleteAsync(long id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
