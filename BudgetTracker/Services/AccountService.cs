using BudgetTracker.Data.Entities;
using BudgetTracker.Models.Constants;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.Enumerations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BudgetTracker.Services
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class AccountService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
    ) : IAccountService
    {

        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        public async Task<(IdentityResult, ApplicationUser?)> RegisterNewUserAsync(RegistrationDto newUser)
        {
            ApplicationUser user = new()
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName
            };

            // Utilize the identity stores for username and email
            await _userManager.SetUserNameAsync(user, newUser.Email);
            await _userManager.SetEmailAsync(user, newUser.Email);

            // Create the user
            var result = await _userManager.CreateAsync(user, newUser.Password);

            ApplicationUser? createdUser = await _userManager.FindByEmailAsync(newUser.Email);

            return (result, createdUser);
        }

        public async Task<bool> DoesUserExistAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<SignInResult> LoginUserAsync(LoginDto user)
        {
            ApplicationUser? userAccount = await _userManager.FindByEmailAsync(user.Email);

            if (userAccount == null)
            {
                // User does not exist
                return SignInResult.Failed;
            }

            SignInResult checkPassword = await _signInManager.CheckPasswordSignInAsync(userAccount, user.Password, true);

            if (!checkPassword.Succeeded)
            {
                return checkPassword;
            }

            List<Claim> claims = [
                new Claim(CustomClaims.FirstName, userAccount.FirstName ?? ""),
                new Claim(CustomClaims.LastName, userAccount.LastName ?? "")
            ];

            // Attempt to sign in; lock on failure
            await _signInManager.SignInWithClaimsAsync(userAccount, user.RememberMe, claims);

            return SignInResult.Success;
        }
    }
}
