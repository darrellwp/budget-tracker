using BudgetTracker.Models.DTOs;
using BudgetTracker.Data.Entities;

namespace BudgetTracker.Services;

/// <summary>
/// Contains most of the application logic for <see cref="Category"/>, <see cref="Charge"/>, and <see cref="Income"/>.
/// This can be broken down further later if needed, but for now it serves as a general service interface.
/// TODO: Potentially add result object for returns; most of the calls are too simple to care here though bool or void is fine
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Attempts to add a new category for the user if they are not over their limit
    /// </summary>
    /// <param name="categoryDto">Category modification DTO</param>
    /// <returns>Whether the add was successful</returns>
    Task<bool> AddCategoryAsync(CategoryModifyDto categoryDto, Guid userId);

    /// <summary>
    /// Add a new transaction to the users account
    /// </summary>
    /// <param name="transactionDto"></param>
    /// <param name="useId"></param>
    Task<bool> AddTransactionAsync(TransactionModifyDto transactionDto, Guid userId);

    /// <summary>
    /// Gets the category that matches the ID of the record and the user ID
    /// </summary>
    /// <param name="categoryId"><see cref="Category.CategoryId"/></param>
    /// <param name="userId">User's GUID</param>
    /// <returns>Single matching record if found</returns>
    Task<CategoryModifyDto?> GetUserCategoryAsync(Guid categoryId, Guid userId);

    /// <summary>
    /// Gets the list of categories for a given user and converts them to DTOs
    /// </summary>
    /// <param name="userId">User's GUID</param>
    /// <returns><see cref="IEnumerable{T}"/> of category DTOs</returns>
    Task<IEnumerable<CategoryUserListDto>> GetUserCategoryListAsync(Guid userId);

    /// <summary>
    /// Returns a list of user place of purchase history for suggestions
    /// </summary>
    /// <param name="userId">User's GUID</param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="Transaction.PlaceOfPurchase"/></returns>
    Task<IEnumerable<string>> GetUserLocationHistoryAsync(Guid userId);

    /// <summary>
    /// Gets the transaction that matches the ID of the record and by user ID
    /// </summary>
    /// <param name="transactionId"><see cref="Transaction.TransactionId"/></param>
    /// <param name="userId">User's GUID</param>
    /// <returns><see cref="TransactionModifyDto"/></returns>
    Task<TransactionModifyDto?> GetUserTransactionAsync(Guid transactionId, Guid userId);

    /// <summary>
    /// Gets the users transactions by a page amount
    /// </summary>
    /// <param name="pageNumber">The page to take</param>
    /// <param name="userId">The user's ID</param>
    /// <returns>A set of transactions based on the user's ID</returns>
    Task<TransactionUserListDto> GetUserTransactionsByPageAsync(Guid userId, TransactionSearchFilterDto filters);

    /// <summary>
    /// Attempts to remove a category for a user after verifying ownership
    /// </summary>
    /// <param name="categoryId"><see cref="Category.CategoryId"> to remove</param>
    /// <param name="userId">The user attempting to remove</param>
    /// <returns>Whether or not the remove was successful</returns>
    Task<bool> RemoveCategoryAsync(Guid categoryId, Guid userId);

    /// <summary>
    /// Attempts to remove a transaction for a user after verifing ownership
    /// </summary>
    /// <param name="transactionId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task RemoveTransactionAsync(Guid transactionId, Guid userId);


    /// <summary>
    /// Updates an existing category after verifying ownership
    /// </summary>
    /// <param name="categoryDto">Category modification DTO</param>
    /// <param name="userId">User processing the change</param>
    /// <returns>Whether or not the update was successful</returns>
    Task<bool> UpdateCategoryAsync(CategoryModifyDto categoryDto, Guid userId);

    /// <summary>
    /// Updates an existing transaction after verifying ownership
    /// </summary>
    /// <param name="transactionDto">Modification DTO for transactions</param>
    /// <param name="userId">User processing the change</param>
    /// <returns></returns>
    Task<bool> UpdateTransactionAsync(TransactionModifyDto transactionDto, Guid userId);
}
