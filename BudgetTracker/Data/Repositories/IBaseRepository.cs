namespace BudgetTracker.Data.Repositories;

/// <summary>
/// Provides a base class for quick calls in other repository classes
/// This can be injected into other repositories to reduce code duplication
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task SaveChangesAsync();
}
