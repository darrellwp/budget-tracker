using BudgetTracker.Extensions;
using BudgetTracker.Models.Constants;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.Maps;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetTracker.Areas.User.Pages.Transactions;

public class EditModel(IUserService userService) : PageModel
{
    private readonly IUserService _userService = userService;

    [BindProperty]
    public TransactionViewModel Transaction { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid transactionId)
    {
        TransactionModifyDto? transaction = await _userService.GetUserTransactionAsync(transactionId, User.GetUserId());

        if (transaction == null)
        {
            return NotFound();
        }

        Transaction = transaction.ToViewModel();
        Transaction.ReturnUrl = TempData[TempDataKeys.ReturnUrl] as string;

        TempData.Keep(TempDataKeys.ReturnUrl);

        // Populate the options for the view model
        await PopulateTransactionOptions();

        return Page();
    }

    /// <summary>
    /// Updates the currently opened transaction
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (Transaction == null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        // Converts the view model to a DTO and gets the currently signed in user
        Guid userId = User.GetUserId();
        TransactionModifyDto transactionDto = Transaction.ToModifyDto();

        // Updates an existing category
        await _userService.UpdateTransactionAsync(transactionDto, userId);

        // Sets temp data for notifications
        TempData[TempDataKeys.ToastNotification] = $"The category was successfully updated.";

        // Just in case they have query parameters set
        var returnUrl = TempData[TempDataKeys.ReturnUrl] as string;

        if (!string.IsNullOrWhiteSpace(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToPage("/Transactions/Index", new { area = "User" });
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
