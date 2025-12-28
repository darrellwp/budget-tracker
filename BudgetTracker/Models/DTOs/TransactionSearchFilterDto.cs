namespace BudgetTracker.Models.DTOs;

/// <summary>
/// The set of search filters for paginating user transactions
/// </summary>
public class TransactionSearchFilterDto
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}