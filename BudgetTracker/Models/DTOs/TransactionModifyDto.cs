using BudgetTracker.Models.Enumerations;

namespace BudgetTracker.Models.DTOs;

public class TransactionModifyDto
{
    public Guid TransactionId { get; set; }
    public required string Description { get; set; }
    public string? PlaceOfPurchase { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public Guid? CategoryId { get; set; }
    public DateOnly DateOccurred { get; set; }
}
