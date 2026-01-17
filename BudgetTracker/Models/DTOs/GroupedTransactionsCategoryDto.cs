namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Used to represent grouped transaction totals by category
/// This is an output of a grouped by query in EF
/// </summary>
public class GroupedTransactionsCategoryDto
{   
    public int Year { get; set; }
    public int? Month { get; set; }
    public int? Week { get; set; }
    public string Category { get; set; } = "Uncategorized";
    public decimal Total { get; set; }
}
