using BudgetTracker.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.Areas.Identity.Pages.Account;

public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<IActionResult> OnGet()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            await _signInManager.SignOutAsync();
        }

        return RedirectToPage("/Index");
    }
}
