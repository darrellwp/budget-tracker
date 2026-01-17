namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Part of the data points for category charts as a part of the <see cref="CategoryChartDataDto"/>
/// </summary>
public class CategoryChartDataGroupDto
{
    public string Category { get; set; } = "Uncategorized";
    public decimal Amount { get; set; }
}