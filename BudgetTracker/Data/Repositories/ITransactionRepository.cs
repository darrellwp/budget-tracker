using BudgetTracker.Data.Entities;
using BudgetTracker.Models.DTOs;

namespace BudgetTracker.Data.Repositories;

/// <summary>
/// Repository representing CRUD interactions for the <see cref="Transaction"/> table
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Adds a new transaction to the database
    /// </summary>
    /// <param name="transaction"><see cref="Transaction"/> to add</param>
    Task AddTransactionAsync(Transaction transaction);

    /// <summary>
    /// Gets a transaction by the provided <see cref="Transaction.TransactionId"/>
    /// </summary>
    /// <param name="transactionId"><see cref="Transaction.TransactionId"/></param>
    /// <returns><see cref="Transaction"/> record</returns>
    Task<Transaction?> GetTransactionByIdAsync(Guid transactionId);

    /// <summary>
    /// Gets a list of purchase locations for the user
    /// Used for autofilling a suggestion list
    /// </summary>
    /// <param name="userId"></param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="Transaction.PlaceOfPurchase"/></returns>
    Task<IEnumerable<string>> GetUserLocationHistoryAsync(Guid userId);

    /// <summary>
    /// Gets the count of user <see cref="Transaction"/> records
    /// </summary>
    /// <param name="userId">User ID to get the count for</param>
    /// <returns>Count</returns>
    Task<int> GetUserTransactionCountAsync(Guid userId, DateOnly? startDate, DateOnly? endDate);

    /// <summary>
    /// Gets a set of user <see cref="Transaction"/> records based on the page number and count
    /// </summary>
    /// <param name="pageNumber">The page to grab</param>
    /// <param name="pageCount">The size of the page</param>
    /// <param name="userId">User ID of the records</param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="Transaction"/></returns>
    Task<IEnumerable<Transaction>> GetUserTransactionsByPageAsnyc(Guid userId, TransactionSearchFilterDto filters);

    /// <summary>
    /// Removes the given transaction record from the database
    /// </summary>
    /// <param name="transaction"><see cref="Transaction"/></param>
    /// <returns></returns>
    Task RemoveTransactionAsync(Transaction transaction);

    /// <summary>
    /// Quick save change call
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
}