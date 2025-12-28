using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.ViewModels;

namespace BudgetTracker.Models.Maps;

/// <summary>
/// Maps for Login and Registration models
/// </summary>
public static class LoginMaps
{
    /// <summary>
    /// Converts a <see cref="LoginViewModel"/> to a <see cref="LoginDto"/>.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static LoginDto ToDto(this LoginViewModel viewModel)
    {
        return new LoginDto
        {
            Email = viewModel.Email ?? "",
            Password = viewModel.Password ?? "",
            RememberMe = viewModel.RememberMe
        };
    }

    /// <summary>
    /// Converts a <see cref="RegistrationViewModel"/> to a <see cref="RegistrationDto"/>."/>
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static RegistrationDto ToDto(this RegistrationViewModel viewModel)
    {
        return new RegistrationDto
        {
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Email = viewModel.Email,
            Password = viewModel.Password
        };
    }
}
