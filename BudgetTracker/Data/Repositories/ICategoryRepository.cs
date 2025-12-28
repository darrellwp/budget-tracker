using BudgetTracker.Data.Entities;

namespace BudgetTracker.Data.Repositories;

/// <summary>
/// Repository for handling <see cref="Category"/> operations on the database layer
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Adds a new category to the database
    /// </summary>
    /// <param name="category"><see cref="Category"/> to add</param>
    Task AddCategoryAsync(Category category);

    /// <summary>
    /// Gets a single category by the unique GUID
    /// </summary>
    /// <param name="categoryId"><see cref="Category.CategoryId"/></param>
    /// <returns>Matching <see cref="Category"/></returns>
    Task<Category?> GetCategoryByIdAsync(Guid categoryId);

    /// <summary>
    /// Gets all the caetegories for a given user
    /// </summary>
    /// <param name="userId">User's GUID</param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="Category"/></returns>
    Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(Guid userId);

    /// <summary>
    /// Gets the count of categories the user has created in their account
    /// </summary>
    /// <param name="userId">User's GUID</param>
    /// <returns>Amount of categories</returns>
    Task<int> GetCategoryCountByUserIdAsync(Guid userId);

    /// <summary>
    /// Removes a category from the database
    /// History is stored via temporal tables
    /// </summary>
    /// <param name="category"><see cref="Category"/> object to remove</param>
    Task RemoveCategoryAsync(Category category);

    /// <summary>
    /// Quick save change call
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
}
