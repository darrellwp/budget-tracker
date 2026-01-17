namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Represents chart data for a single category, including its label and associated grouped amounts.
/// </summary>
public class CategoryChartDataDto
{
    public string Label { get; set; } = string.Empty;
    public IEnumerable<CategoryChartDataGroupDto> Amounts { get; set; }
}
