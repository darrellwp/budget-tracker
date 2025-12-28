namespace BudgetTracker.Data.Entities;

public class Category
{
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? MonthlyLimit { get; set; }
    public string? Icon { get; set; }
    public int? LastModifiedDays { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = [];
}
