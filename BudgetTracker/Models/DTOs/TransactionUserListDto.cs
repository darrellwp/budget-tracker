namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Object representing the paged table containing a list of user transactions
/// Options for the currently applied filters are also included
/// </summary>
public class TransactionUserListDto
{
    public required IEnumerable<TransactionUserListItemDto> Transactions { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int MaxPage { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
