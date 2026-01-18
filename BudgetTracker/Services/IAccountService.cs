using BudgetTracker.Data.Entities;
using BudgetTracker.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Services;

/// <summary>
/// Service to manage user accounts internal to entity framework
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Registers a new user in the application
    /// </summary>
    /// <param name="newUser"></param>
    /// <returns>Result of user creation</returns>
    Task<(IdentityResult, ApplicationUser?)> RegisterNewUserAsync(RegistrationDto newUser);

    /// <summary>
    /// Logs in a user with the given credentials
    /// </summary>
    /// <param name="user"></param>
    /// <returns>Result of login</returns>
    Task<SignInResult> LoginUserAsync(LoginDto user);

    /// <summary>
    /// Determines if a user is registered with the given email
    /// </summary>
    /// <param name="email">Email of the user</param>
    /// <returns>Is the user registered</returns>
    Task<bool> DoesUserExistAsync(string email);

}
