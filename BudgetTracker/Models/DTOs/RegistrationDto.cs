namespace BudgetTracker.Models.DTOs;

/// <summary>
/// Represents a user registration data transfer object
/// </summary>
public class RegistrationDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
