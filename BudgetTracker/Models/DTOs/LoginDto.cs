namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Login data transfer object
/// </summary>
public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool RememberMe { get; set; }
}
