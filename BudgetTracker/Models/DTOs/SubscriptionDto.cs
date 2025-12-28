namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Represents the subscription plan details, including tier name and usage limits for categories, transactions, and accounts.
/// </summary>
public class SubscriptionDto
{
    public string TierName { get; set; } = null!;
    public int MaxCategories { get; set; }
    public int MaxTransactions { get; set; }
    public int MaxAccounts { get; set; }
}