namespace BudgetTracker.Models.DTOs;

public class DashboardDataDto
{
    public IEnumerable<BalanceChartDataDto> Balances { get; set; } = null!;
    public IEnumerable<string> CategoryLabels { get; set; } = null!;
    public IEnumerable<CategoryChartDataDto> Categories { get; set; } = null!;
}
