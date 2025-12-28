namespace BudgetTracker.Models.Constants;

/// <summary>
/// Set of cache keys used throughout the application
/// Constatns set so that they can be referenced in multiple places without risk of typos
/// </summary>
public static class CacheKeys
{
    /// <summary>
    /// Cache key for storing/retrieving user categories
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Cache key string</returns>
    public static string UserCategories(Guid userId) => $"UserCategories_{userId}";
}