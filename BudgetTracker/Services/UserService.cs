using BudgetTracker.Data.Entities;
using BudgetTracker.Data.Repositories;
using BudgetTracker.Models.Constants;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.Maps;
using BudgetTracker.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BudgetTracker.Services;

/// <summary>
/// <inheritdoc/>
/// </summary>
public class UserService(
    IMemoryCache cache,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository,
    IOptions<ApplicationSettings> appSettings
) : IUserService
{
    private readonly IMemoryCache _cache = cache;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ApplicationSettings _appSettings = appSettings.Value;

    public async Task<bool> AddCategoryAsync(CategoryModifyDto categoryDto, Guid userId)
    {
        int userCategories = await _categoryRepository.GetCategoryCountByUserIdAsync(userId);

        if(userCategories >= _appSettings.MaxCategories)
        {
            return false;
        }

        // Map DTO to Entity
        Category category = categoryDto.ToEntity();
        category.UserId = userId;

        // Add to database
        await _categoryRepository.AddCategoryAsync(category);

        // Remove the cache
        _cache.Remove(CacheKeys.UserCategories(category.UserId));

        return true;
    }

    public async Task<bool> AddTransactionAsync(TransactionModifyDto transactionDto, Guid userId)
    {   
        // Test if the category belongs to the user; otherwise return out. Consider it malicious
        if(transactionDto.CategoryId != null)
        {
            Category? category = await _categoryRepository.GetCategoryByIdAsync(transactionDto.CategoryId.Value);

            if(category == null || category.UserId != userId)
            {
                return false;
            }
        }

        // Map DTO to Entity
        Transaction transaction = transactionDto.ToEntity();
        transaction.UserId = userId;

        // Add to database
        await _transactionRepository.AddTransactionAsync(transaction);

        return true;
    }

    public async Task<CategoryModifyDto?> GetUserCategoryAsync(Guid categoryId, Guid userId)
    {
        Category? category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

        // Returns null if the user attempts to get a category that isn't theirs
        if(category == null || category.UserId != userId)
        {
            return null;
        }

        return category.ToModifyDto();
    }

    public async Task<IEnumerable<CategoryUserListDto>> GetUserCategoryListAsync(Guid userId)
    {
        IEnumerable<CategoryUserListDto>? categories =  await _cache.GetOrCreateAsync(CacheKeys.UserCategories(userId), async entry =>
        {
            IEnumerable<Category> categoryEntities = await _categoryRepository.GetCategoriesByUserIdAsync(userId);

            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            entry.Size = categoryEntities.Count();

            return categoryEntities.Select(c => c.ToUserListDto());
        });

        return categories ?? [];
    }

    public Task<IEnumerable<string>> GetUserLocationHistoryAsync(Guid userId)
    {
        return _transactionRepository.GetUserLocationHistoryAsync(userId);
    }

    public async Task<TransactionModifyDto?> GetUserTransactionAsync(Guid transactionId, Guid userId)
    {
        Transaction? transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);

        // Returns null if the user is attempts to get a transaction that isn't theirs
        if (transaction == null || transaction.UserId != userId)
        {
            return null;
        }

        return transaction.ToModifyDto();
    }

    public async Task<TransactionUserListDto> GetUserTransactionsByPageAsync(Guid userId, TransactionSearchFilterDto filters)
    {
        int transactionCount = await _transactionRepository.GetUserTransactionCountAsync(userId, filters.StartDate, filters.EndDate);
        int maxPageCount = transactionCount == 0 ? 1 : (int)Math.Ceiling((decimal)transactionCount / (decimal)filters.PageSize);

        // Clamps the size and number to be within acceptable bounds
        // TODO: hardcode values now, update later
        filters.PageNumber = Math.Clamp(filters.PageNumber, 1, maxPageCount);
        filters.PageSize = filters.PageSize > 100 ? 10 : filters.PageSize;

        // Make query after validating the size/number are within bounds
        IEnumerable<Transaction> transactions = await _transactionRepository.GetUserTransactionsByPageAsnyc(userId, filters);

        TransactionUserListDto transactionList = new()
        {
            Transactions = transactions.Select(x => x.ToUserListDto()),
            TotalCount = transactionCount,
            PageNumber = filters.PageNumber,
            PageSize = filters.PageSize,
            MaxPage = maxPageCount
        };

        return transactionList;
    }

    public async Task<bool> RemoveCategoryAsync(Guid categoryId, Guid userId)
    {
        Category? category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

        // Ensure it exists and they are the owner of the category
        if (category == null || category.UserId != userId)
        {
            return false;
        }

        // Remove from database
        await _categoryRepository.RemoveCategoryAsync(category);

        // Remove the cache
        _cache.Remove(CacheKeys.UserCategories(category.UserId));

        return true;
    }

    public async Task RemoveTransactionAsync(Guid transactionId, Guid userId)
    {
        Transaction? transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);

        if (transaction == null || transaction.UserId != userId) return;

        await _transactionRepository.RemoveTransactionAsync(transaction);
    }

    public async Task<bool> UpdateCategoryAsync(CategoryModifyDto categoryDto, Guid userId)
    {
        Category? category = await _categoryRepository.GetCategoryByIdAsync(categoryDto.CategoryId);

        // Validate it's the correct user
        if(category == null || category.UserId != userId)
        {
            return false;
        }

        // Update the existing entity
        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;
        category.MonthlyLimit = categoryDto.MonthlyLimit;
        category.Icon = categoryDto.Icon;

        // Save the changes
        await _categoryRepository.SaveChangesAsync();

        // Remove the cache
        _cache.Remove(CacheKeys.UserCategories(category.UserId));

        return true;
    }

    public async Task<bool> UpdateTransactionAsync(TransactionModifyDto transactionDto, Guid userId)
    {
        Transaction? transaction = await _transactionRepository.GetTransactionByIdAsync(transactionDto.TransactionId);

        if(transaction == null || transaction.UserId != userId)
        {
            return false;
        }

        if (transactionDto.CategoryId != null)
        {
            Category? category = await _categoryRepository.GetCategoryByIdAsync(transactionDto.CategoryId.Value);

            if (category == null || category.UserId != userId)
            {
                return false;
            }
        }

        transaction.Description = transactionDto.Description;
        transaction.PlaceOfPurchase = transactionDto.PlaceOfPurchase;
        transaction.TransactionType = transactionDto.TransactionType;
        transaction.Amount = transactionDto.Amount;
        transaction.DateOccurred = transactionDto.DateOccurred;
        transaction.CategoryId = transactionDto.CategoryId;

        await _transactionRepository.SaveChangesAsync();

        return true;
    }
}