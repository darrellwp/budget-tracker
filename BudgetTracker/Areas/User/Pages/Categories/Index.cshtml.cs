using BudgetTracker.Extensions;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.Areas.User.Pages.Categories;

/// <summary>
/// User's category management page.
/// Displays a list of categories and allows the removal and entry to the edit/create page.
/// </summary>
/// <param name="service">Application service</param>
[Authorize]
public class IndexModel(IUserService userService) : PageModel
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// List of user's categories
    /// </summary>
    public IEnumerable<CategoryUserListDto> Categories { get; set; } = [];

    /// <summary>
    /// Sets the list for the user to view
    /// </summary>
    /// <returns></returns>
    public async Task OnGetAsync()
    {
        Categories = await _userService.GetUserCategoryListAsync(User.GetUserId());
    }

    /// <summary>
    /// Allows for the removal of a category, service verifies ownership
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostRemoveCategoryAsync(Guid categoryId)
    {
        // Attempt to remove the category
        bool removeSuccess = await _userService.RemoveCategoryAsync(categoryId, User.GetUserId());

        // If a failure occurs, stay on the page
        if (!removeSuccess)
        {
            return Page();
        }

        return RedirectToPage();
    }
}
