using BudgetTracker.Data.Entities;
using BudgetTracker.Extensions;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.Areas.User.Pages.Transactions;

[Authorize]
public class IndexModel(IUserService userService) : PageModel
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Gets the list of transactions for the given filter set
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public async Task<PartialViewResult> OnGetTransactions([FromQuery] TransactionSearchFilterDto filters)
    {
        // Gets the user transaction list
        TransactionUserListDto transactions = await _userService.GetUserTransactionsByPageAsync(User.GetUserId(), filters);

        // TempData return URL if this is hit
        TempData["ReturnUrl"] = Url.Page("/Transactions/Index", new { 
            area = "User",
            pageNumber = filters.PageNumber,
            pageSize = filters.PageSize,          
            startDate = filters.StartDate, 
            endDate = filters.EndDate 
        });

        return Partial("_TransactionTable", transactions);
    }

    /// <summary>
    /// Removes the transaction from the database if the user is the correct owner
    /// </summary>
    /// <param name="transactionId"><see cref="Transaction.TransactionId"/></param>
    /// <param name="returnUrl">Return url to keep filters</param>
    /// <returns></returns>
    public async Task OnPostRemoveAsync(Guid transactionId)
    {
        await _userService.RemoveTransactionAsync(transactionId, User.GetUserId());
    }
}