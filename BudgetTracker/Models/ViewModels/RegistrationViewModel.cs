using BudgetTracker.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models.ViewModels;

/// <summary>
/// Registration view model
/// </summary>
public class RegistrationViewModel
{
    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(50, ErrorMessage = ValidationMessages.MaxLength)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty!;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(50, ErrorMessage = ValidationMessages.MaxLength)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty!;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty!;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [StringLength(30, ErrorMessage = ValidationMessages.MinMaxLength, MinimumLength = 10)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty!;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "This does not match the password.")]
    public string ConfirmPassword { get; set; } = string.Empty!;
}
