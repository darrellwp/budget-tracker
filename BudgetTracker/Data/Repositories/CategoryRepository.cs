using BudgetTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Data.Repositories;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <param name="context">Database context</param>
/// <param name="baseRepository">Base shared repository</param>
public class CategoryRepository(IAppDbContext context, IBaseRepository<Category> baseRepository) : ICategoryRepository
{
    private readonly IAppDbContext _context = context;
    private readonly IBaseRepository<Category> _baseRepository = baseRepository;

    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public Task<Category?> GetCategoryByIdAsync(Guid categoryId) => _baseRepository.GetByIdAsync(categoryId);

    public async Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(Guid userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<int> GetCategoryCountByUserIdAsync(Guid userId) => await _context.Categories.CountAsync(c => c.UserId == userId);

    public async Task RemoveCategoryAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public Task SaveChangesAsync() => _baseRepository.SaveChangesAsync();
}
