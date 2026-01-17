namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Used to represent grouped transaction total balances
/// This is an output of a grouped by query in EF
/// </summary>
public class GroupedTransactionsSumDto
{
    public int Year { get; set; }
    public int? Month { get; set; }
    public int? Week { get; set; }
    public decimal IncomeTotal { get; set; }
    public decimal ExpenseTotal { get; set; }
    public decimal OverallTotal { get; set; }
}
