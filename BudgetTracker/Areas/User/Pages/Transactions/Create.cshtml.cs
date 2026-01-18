using BudgetTracker.Extensions;
using BudgetTracker.Models.Constants;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.Enumerations;
using BudgetTracker.Models.Maps;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetTracker.Areas.User.Pages.Transactions;

[Authorize]
public class CreateModel(IUserService userService) : PageModel
{

    private readonly IUserService _userService = userService;

    /// <summary>
    /// Gets and set the transaction to be created
    /// </summary>
    [BindProperty]
    public TransactionViewModel? Transaction { get; set; }

    /// <summary>
    /// Initialize the page
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnGetAsync()
    {
        // Initialize the page
        Transaction = new TransactionViewModel
        {
            DateOccurred = DateOnly.FromDateTime(DateTime.Now),
            TransactionType = TransactionType.Expense,
            ReturnUrl = TempData[TempDataKeys.ReturnUrl] as string
        };

        TempData.Keep(TempDataKeys.ReturnUrl);

        // Populate the options
        await PopulateTransactionOptions();

        return Page();
    }

    /// <summary>
    /// Process adding the transaction
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (Transaction == null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        Guid userId = User.GetUserId();
        TransactionModifyDto transactionDto = Transaction.ToModifyDto();

        var result = await _userService.AddTransactionAsync(transactionDto, userId);

        if (result)
        {
            var returnUrl = TempData[TempDataKeys.ReturnUrl] as string;

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage("/Transactions/Index", new { area = "User" });
        }

        TempData.Keep(TempDataKeys.ReturnUrl);

        // Rebind if there was a failure (should not occur unless bad input and no client validation)
        await PopulateTransactionOptions();

        return Page();
    }

    /// <summary>
    /// Populates the transaction options for categories and the autofill suggestions based on previous locations
    /// </summary>
    /// <returns></returns>
    private async Task PopulateTransactionOptions()
    {
        if (Transaction == null) return;

        var userCategories = await _userService.GetUserCategoryListAsync(User.GetUserId());
        var purchaseLocations = await _userService.GetUserLocationHistoryAsync(User.GetUserId());

        Transaction.CategoryOptions = userCategories.OrderBy(x => x.Name).Select(x => new SelectListItem(x.Name, x.CategoryId.ToString()));
        Transaction.PurchaseLocations = purchaseLocations.Order();
    }
}
