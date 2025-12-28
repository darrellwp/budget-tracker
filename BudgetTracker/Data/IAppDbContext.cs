using BudgetTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BudgetTracker.Data;

/// <summary>
/// Represents the application's database context, providing access to the database entities and operations.
/// </summary>
/// <remarks>This interface defines the contract for interacting with the application's database, including entity
/// sets and methods for adding entities and executing SQL commands. It is typically used within a dependency injection
/// setup to manage database operations in a consistent manner.</remarks>
public interface IAppDbContext
{
    // DbSets
    DbSet<Category> Categories { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    DbSet<HttpRequestLog> HttpRequestLogs { get; set; }

    // Methods to expose from base DbContext
    EntityEntry Add(object entity);
    ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
    Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}
