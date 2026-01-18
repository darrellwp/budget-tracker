using BudgetTracker.Extensions;
using BudgetTracker.Models.Constants;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.Maps;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.Areas.User.Pages.Categories;

[Authorize]
public class EditModel(IUserService service) : PageModel
{
    private readonly IUserService _service = service;

    [BindProperty]
    public CategoryViewModel Category { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid categoryId)
    {
        CategoryModifyDto? category = await _service.GetUserCategoryAsync(categoryId, User.GetUserId());

        if (category == null)
        {
            return NotFound();
        }

        Category = category.ToViewModel();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Category == null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        // Converts the view model to a DTO and gets the currently signed in user
        Guid userId = User.GetUserId();
        CategoryModifyDto categoryDto = Category.ToModifyDto();

        // Updates an existing category
        await _service.UpdateCategoryAsync(categoryDto, userId);

        // Sets temp data for notifications
        TempData[TempDataKeys.ToastNotification] = $"The category was successfully updated.";

        return RedirectToPage("/Categories/Index", new { area = "User" });
    }
}
