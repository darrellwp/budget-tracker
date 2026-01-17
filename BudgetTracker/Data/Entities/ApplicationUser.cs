using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Data.Entities;

/// <summary>
/// Extends the default IdentityUser to include additional properties
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set; } = [];
}
