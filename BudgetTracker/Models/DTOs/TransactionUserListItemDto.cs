using BudgetTracker.Models.Enumerations;

namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Invidiual records for the user's transactions list
/// </summary>
public class TransactionUserListItemDto
{
    public Guid TransactionId { get; set; }
    public string? Category { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateOnly DateOccurred { get; set; }
    public required string Description { get; set; }
    public string? PlaceOfPurchase { get; set; }
}