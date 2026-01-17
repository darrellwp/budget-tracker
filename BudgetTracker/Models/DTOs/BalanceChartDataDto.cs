namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Data point for the balance chart on the User/Index (dashboard page)
/// </summary>
public class BalanceChartDataDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
}
