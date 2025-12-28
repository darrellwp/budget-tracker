namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Displays on a list for the user to see their categories
/// </summary>
public class CategoryUserListDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? MonthlyLimit { get; set; }
    public string? Icon { get; set; }
    public int LastModifiedDays { get; set; }
}
