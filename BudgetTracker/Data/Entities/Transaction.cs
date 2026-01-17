using BudgetTracker.Models.Enumerations;

namespace BudgetTracker.Data.Entities;

/// <summary>
/// Transaction for a user
/// This includes incomes and expenses
/// </summary>
public class Transaction
{
    public Guid TransactionId { get; set; }
    public Guid UserId { get; set; }
    public Guid? CategoryId { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateOnly DateOccurred { get; set; }
    public string Description { get; set; } = null!;
    public string? PlaceOfPurchase { get; set; }
    public Category? Category { get; set; }
    public ApplicationUser User { get; set; } = null!;
}
