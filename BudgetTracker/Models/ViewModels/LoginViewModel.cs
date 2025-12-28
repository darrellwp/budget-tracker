using BudgetTracker.Models.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models.ViewModels;

/// <summary>
/// Login view model
/// </summary>
public class LoginViewModel
{
    [DisplayName("Email")]
    [Required(ErrorMessage = ValidationMessages.Required)]
    [EmailAddress]
    public string? Email { get; set; }

    [DisplayName("Password")]
    [Required(ErrorMessage = ValidationMessages.Required)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DisplayName("Remember me?")]
    public bool RememberMe { get; set; }
}
