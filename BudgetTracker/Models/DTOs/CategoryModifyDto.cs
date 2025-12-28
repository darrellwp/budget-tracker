namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Used to transfer data from the presentation to the data layer
/// </summary>
public class CategoryModifyDto
{
    public Guid CategoryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal? MonthlyLimit { get; set; }
    public string? Icon { get; set; }      
}
