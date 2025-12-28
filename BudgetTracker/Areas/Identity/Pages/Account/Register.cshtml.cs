using BudgetTracker.Data.Entities;
using BudgetTracker.Models;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.Maps;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace BudgetTracker.Areas.Identity.Pages.Account;

public class RegisterModel(
    IAccountService accountService,
    UserManager<ApplicationUser> userManager,
    ISmtpService smptService
) : PageModel
{
    private readonly ISmtpService _smptService = smptService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IAccountService _accountService = accountService;

    [BindProperty]
    public RegistrationViewModel Registration { get; set; } = null!;

    [TempData]
    public bool RegistrationComplete { get; set; } = false;

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            bool doesUserExist = await _accountService.DoesUserExistAsync(Registration.Email);

            if (!doesUserExist)
            {
                RegistrationDto registrationDto = Registration.ToDto();

                var (result, newUser) = await _accountService.RegisterNewUserAsync(registrationDto);

                if (result.Succeeded && newUser != null)
                {
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // Build the call back URL
                    string? callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new
                        {
                            area = "Identity",
                            userId = newUser.Id,
                            code = code
                        },
                        protocol: Request.Scheme
                    );

                    // Send a confirmation email
                    // TODO : Update the email to be more verbose later, look into templates
                    if(callbackUrl != null)
                    {
                        await _smptService.SendEmailAsync(Registration.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }                   
                }
            }

            // Show complete no matter what to avoid account enumeration
            RegistrationComplete = true;
        }

        return RedirectToPage();
    }
}
