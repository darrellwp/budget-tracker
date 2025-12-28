using BudgetTracker.Data.Entities;
using BudgetTracker.Models.DTOs;
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

        if(startDate != null)
        {
            query = query.Where(x => x.DateOccurred >= startDate);
        }

        if(endDate != null)
        {
            query = query.Where(x => x.DateOccurred <= endDate);
        }

        return await query.CountAsync(x => x.UserId == userId);
    }

    public async Task<IEnumerable<Transaction>> GetUserTransactionsByPageAsnyc(Guid userId, TransactionSearchFilterDto filters)
    {
        IQueryable<Transaction> query = _context.Transactions
            .Include(x => x.Category)
            .Where(x => x.UserId == userId);

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

    public async Task RemoveTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }

    public Task SaveChangesAsync() => _baseRepository.SaveChangesAsync();
}