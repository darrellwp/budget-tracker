using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Data.Repositories;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="context"></param>
public class BaseRepository<TEntity>(IAppDbContext context) : IBaseRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _set = context.Set<TEntity>();

    public async Task<TEntity?> GetByIdAsync(Guid id) => await _set.FindAsync(id);
    public async Task<TEntity?> GetByIdAsync(int id) => await _set.FindAsync(id);
    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _set.ToListAsync();
    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
