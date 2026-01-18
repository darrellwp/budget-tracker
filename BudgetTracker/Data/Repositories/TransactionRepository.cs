using BudgetTracker.Data.Entities;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Data.Repositories;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <param name="context">Application database context</param>
public class TransactionRepository(IAppDbContext context, IBaseRepository<Transaction> baseRepository) : ITransactionRepository
{
    private readonly IAppDbContext _context = context;
    private readonly IBaseRepository<Transaction> _baseRepository = baseRepository;

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public Task<Transaction?> GetTransactionByIdAsync(Guid transactionId) => _baseRepository.GetByIdAsync(transactionId);

    public Task<DateOnly> GetUserFirstTransactionDateAsync(Guid userId)
    {
        return _context.Transactions
             .AsNoTracking()
             .Where(x => x.UserId == userId)
             .Select(x => x.DateOccurred)
             .DefaultIfEmpty()
             .MinAsync();
    }

    public async Task<IEnumerable<string>> GetUserLocationHistoryAsync(Guid userId)
    {
        return await _context.Transactions
            .Select(x => x.PlaceOfPurchase)
            .OfType<string>()
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToListAsync();
    }

    public async Task<int> GetUserTransactionCountAsync(Guid userId, DateOnly? startDate, DateOnly? endDate)
    {
        IQueryable<Transaction> query = _context.Transactions
            .Where(x => x.UserId == userId);

        if (startDate != null)
        {
            query = query.Where(x => x.DateOccurred >= startDate);
        }

        if (endDate != null)
        {
            query = query.Where(x => x.DateOccurred <= endDate);
        }

        return await query.CountAsync(x => x.UserId == userId);
    }

    public async Task<IEnumerable<Transaction>> GetUserTransactionsByPageAsnyc(Guid userId, TransactionSearchFilterDto filters)
    {
        IQueryable<Transaction> query = _context.Transactions
            .Include(x => x.Category)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.DateOccurred);

        if (filters.StartDate != null)
        {
            query = query.Where(x => x.DateOccurred >= filters.StartDate);
        }

        if (filters.EndDate != null)
        {
            query = query.Where(x => x.DateOccurred <= filters.EndDate);
        }

        return await query.Skip((filters.PageNumber - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<GroupedTransactionsSumDto>> GetUserTransactionsGroupedByMonthAsync(Guid userId, DateOnly startDate, DateOnly endDate)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.DateOccurred >= startDate && x.DateOccurred <= endDate)
            .GroupBy(x => new
            {
                Year = x.DateOccurred.Year,
                Month = x.DateOccurred.Month
            })
            .Select(g => new GroupedTransactionsSumDto()
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                IncomeTotal = g.Where(x => x.TransactionType == TransactionType.Income).Sum(x => x.Amount),
                ExpenseTotal = g.Where(x => x.TransactionType == TransactionType.Expense).Sum(x => x.Amount),
                OverallTotal = g.Sum(x => x.TransactionType == TransactionType.Income ? x.Amount : -x.Amount)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();
    }

    public async Task<IEnumerable<GroupedTransactionsSumDto>> GetUserTransactionsGroupedByWeekAsync(Guid userId, DateOnly startDate, DateOnly endDate)
    {

        return await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.DateOccurred >= startDate && x.DateOccurred <= endDate)
            .GroupBy(x => new
            {
                Year = x.DateOccurred.Year,
                Month = x.DateOccurred.Month,
                Week = EF.Functions.DateDiffWeek(startDate, x.DateOccurred)
            })
            .Select(g => new GroupedTransactionsSumDto()
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Week = g.Key.Week,
                IncomeTotal = g.Where(x => x.TransactionType == TransactionType.Income).Sum(x => x.Amount),
                ExpenseTotal = g.Where(x => x.TransactionType == TransactionType.Expense).Sum(x => x.Amount),
                OverallTotal = g.Sum(x => x.TransactionType == TransactionType.Income ? x.Amount : -x.Amount)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Week)
            .ToListAsync();
    }

    public async Task<IEnumerable<GroupedTransactionsSumDto>> GetUserTransactionsGroupedByYearAsync(Guid userId, DateOnly startDate, DateOnly endDate)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.DateOccurred >= startDate && x.DateOccurred <= endDate)
            .GroupBy(x => new
            {
                Year = x.DateOccurred.Year
            })
            .Select(g => new GroupedTransactionsSumDto()
            {
                Year = g.Key.Year,
                IncomeTotal = g.Where(x => x.TransactionType == TransactionType.Income).Sum(x => x.Amount),
                ExpenseTotal = g.Where(x => x.TransactionType == TransactionType.Expense).Sum(x => x.Amount),
                OverallTotal = g.Sum(x => x.TransactionType == TransactionType.Income ? x.Amount : -x.Amount)
            })
            .OrderBy(x => x.Year)
            .ToListAsync();
    }

    public async Task<IEnumerable<GroupedTransactionsCategoryDto>> GetUserTransactionsGroupedByCategoryMonthAsync(Guid userId, DateOnly startDate, DateOnly endDate)
    {
        return await _context.Transactions
           .AsNoTracking()
           .Where(x => x.UserId == userId && x.DateOccurred >= startDate && x.DateOccurred <= endDate && x.TransactionType != TransactionType.Income)
           .GroupBy(x => new
           {
               Year = x.DateOccurred.Year,
               Month = x.DateOccurred.Month,
               Category = x.Category != null ? x.Category.Name : "Uncategorized"
           })
           .Select(g => new GroupedTransactionsCategoryDto()
           {
               Year = g.Key.Year,
               Month = g.Key.Month,
               Category = g.Key.Category,
               Total = g.Sum(x => x.Amount)
           })
           .OrderBy(x => x.Year)
           .ThenBy(x => x.Month)
           .ToListAsync();
    }

    public async Task<IEnumerable<GroupedTransactionsCategoryDto>> GetUserTransactionsGroupedByCategoryWeekAsync(Guid userId, DateOnly startDate, DateOnly endDate)
    {
        return await _context.Transactions
           .AsNoTracking()
           .Where(x => x.UserId == userId && x.DateOccurred >= startDate && x.DateOccurred <= endDate && x.TransactionType != TransactionType.Income)
           .GroupBy(x => new
           {
               Year = x.DateOccurred.Year,
               Month = x.DateOccurred.Month,
               Week = EF.Functions.DateDiffWeek(startDate, x.DateOccurred),
               Category = x.Category != null ? x.Category.Name : "Uncategorized"
           })
           .Select(g => new GroupedTransactionsCategoryDto()
           {
               Year = g.Key.Year,
               Month = g.Key.Month,
               Week = g.Key.Week,
               Category = g.Key.Category,
               Total = g.Sum(x => x.Amount)
           })
           .OrderBy(x => x.Year)
           .ThenBy(x => x.Month)
           .ThenBy(x => x.Week)
           .ToListAsync();
    }

    public async Task<IEnumerable<GroupedTransactionsCategoryDto>> GetUserTransactionsGroupedByCategoryYearAsync(Guid userId, DateOnly startDate, DateOnly endDate)
    {
        return await _context.Transactions
           .AsNoTracking()
           .Where(x => x.UserId == userId && x.DateOccurred >= startDate && x.DateOccurred <= endDate && x.TransactionType != TransactionType.Income)
           .GroupBy(x => new
           {
               Year = x.DateOccurred.Year,
               Category = x.Category != null ? x.Category.Name : "Uncategorized"
           })
           .Select(g => new GroupedTransactionsCategoryDto()
           {
               Year = g.Key.Year,
               Category = g.Key.Category,
               Total = g.Sum(x => x.Amount)
           })
           .OrderBy(x => x.Year)
           .ToListAsync();
    }

    public async Task<decimal> GetUserTransactionSumUntilDateAsync(Guid userId, DateOnly untilDate)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.DateOccurred < untilDate)
            .SumAsync(x => x.TransactionType == TransactionType.Income ? x.Amount : -x.Amount);
    }

    public async Task RemoveTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }

    public Task SaveChangesAsync() => _baseRepository.SaveChangesAsync();
}