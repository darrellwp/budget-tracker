using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.Maps;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.Areas.Identity.Pages.Account;

/// <summary>
/// Processes login requests to the application
/// </summary>
/// <param name="accountService"></param>
public class LoginModel(IAccountService accountService) : PageModel
{

    private readonly IAccountService _accountService = accountService;

    [BindProperty]
    public LoginViewModel UserLogin { get; set; } = null!;

    public string? ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; } = "";


    public IActionResult OnGet(string? returnUrl = null)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Index", new { area = "User" });
        }

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ReturnUrl = returnUrl;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            LoginDto userLogin = UserLogin.ToDto();

            // This can lock out the user on failed attempts
            var result = await _accountService.LoginUserAsync(userLogin);

            if (result.Succeeded)
            {
                // TODO : Redirect to dashboard once created
                return RedirectToPage("/Index", new { area = "User" });
            }
        }

        // Set generic error
        ErrorMessage = "There was an issue with your login attempt. Please try again.";

        // If we got this far, something failed, redisplay form with error
        return RedirectToPage();
    }
}
