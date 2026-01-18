using BudgetTracker.Data.Entities;
using BudgetTracker.Models.DTOs;

namespace BudgetTracker.Data.Repositories;

/// <summary>
/// Repository representing CRUD interactions for the <see cref="Transaction"/> table
/// TODO: Look into options for reducing the grouping calls; for now they work
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
    /// Gets the date of the first transaction the user has ever had
    /// </summary>
    /// <param name="userId">User ID to get the date for</param>
    /// <returns>Date of first transaction</returns>
    Task<DateOnly> GetUserFirstTransactionDateAsync(Guid userId);

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
    /// Retrieves a set of user transactions grouped by month within the provided date range
    /// The groupings are based on the total balance of the users transactions
    /// </summary>
    /// <param name="userId"><see cref="ApplicationUser.UserId"/></param>
    /// <param name="startDate">Start of the date range to group</param>
    /// <param name="endDate">End of the date range to group</param>
    /// <returns>Grouped set of transactions with summed balances</returns>
    Task<IEnumerable<GroupedTransactionsSumDto>> GetUserTransactionsGroupedByMonthAsync(Guid userId, DateOnly startDate, DateOnly endDate);

    /// <summary>
    /// Retrieves a set of user transactions grouped by week within the provided date range
    /// The groupings are based on the total balance of the users transactions
    /// </summary>
    /// <param name="userId"><see cref="ApplicationUser.UserId"/></param>
    /// <param name="startDate">Start of the date range to group</param>
    /// <param name="endDate">End of the date range to group</param>
    /// <returns>Grouped set of transactions with summed balances</returns>
    Task<IEnumerable<GroupedTransactionsSumDto>> GetUserTransactionsGroupedByWeekAsync(Guid userId, DateOnly startDate, DateOnly endDate);

    /// <summary>
    /// Retrieves a set of user transactions grouped by year within the provided date range
    /// The groupings are based on the total balance of the users transactions
    /// </summary>
    /// <param name="userId"><see cref="ApplicationUser.UserId"/></param>
    /// <param name="startDate">Start of the date range to group</param>
    /// <param name="endDate">End of the date range to group</param>
    /// <returns>Grouped set of transactions with summed balances</returns>
    Task<IEnumerable<GroupedTransactionsSumDto>> GetUserTransactionsGroupedByYearAsync(Guid userId, DateOnly startDate, DateOnly endDate);

    /// <summary>
    /// Retrieves a set of user transactions grouped by month and the category within the provided date range
    /// The groupings are based on the total balance of the users transactions
    /// </summary>
    /// <param name="userId"><see cref="ApplicationUser.UserId"/></param>
    /// <param name="startDate">Start of the date range to group</param>
    /// <param name="endDate">End of the date range to group</param>
    /// <returns>Grouped set of transactions with summed balances</returns>
    Task<IEnumerable<GroupedTransactionsCategoryDto>> GetUserTransactionsGroupedByCategoryMonthAsync(Guid userId, DateOnly startDate, DateOnly endDate);

    /// <summary>
    /// Retrieves a set of user transactions grouped by week and category within the provided date range
    /// The groupings are based on the total balance of the users transactions
    /// </summary>
    /// <param name="userId"><see cref="ApplicationUser.UserId"/></param>
    /// <param name="startDate">Start of the date range to group</param>
    /// <param name="endDate">End of the date range to group</param>
    /// <returns>Grouped set of transactions with summed balances</returns>
    Task<IEnumerable<GroupedTransactionsCategoryDto>> GetUserTransactionsGroupedByCategoryWeekAsync(Guid userId, DateOnly startDate, DateOnly endDate);

    /// <summary>
    /// Retrieves a set of user transactions grouped by year and category within the provided date range
    /// The groupings are based on the total balance of the users transactions
    /// </summary>
    /// <param name="userId"><see cref="ApplicationUser.UserId"/></param>
    /// <param name="startDate">Start of the date range to group</param>
    /// <param name="endDate">End of the date range to group</param>
    /// <returns>Grouped set of transactions with summed balances</returns>
    Task<IEnumerable<GroupedTransactionsCategoryDto>> GetUserTransactionsGroupedByCategoryYearAsync(Guid userId, DateOnly startDate, DateOnly endDate);

    /// <summary>
    /// Gets a set of user <see cref="Transaction"/> records based on the page number and count
    /// </summary>
    /// <param name="pageNumber">The page to grab</param>
    /// <param name="pageCount">The size of the page</param>
    /// <param name="userId">User ID of the records</param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="Transaction"/></returns>
    Task<IEnumerable<Transaction>> GetUserTransactionsByPageAsnyc(Guid userId, TransactionSearchFilterDto filters);

    /// <summary>
    /// Sums all of the transactions up until the provided date
    /// </summary>
    /// <param name="untilDate">Cut off date</param>
    /// <returns>Summed transactions</returns>
    Task<decimal> GetUserTransactionSumUntilDateAsync(Guid userId, DateOnly untilDate);

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