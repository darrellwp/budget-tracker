using BudgetTracker.Extensions;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.Maps;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.Areas.User.Pages.Categories;

[Authorize]
public class CreateModel(IUserService service) : PageModel
{
    private readonly IUserService _service = service;

    [BindProperty]
    public CategoryViewModel? Category { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Category == null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        Guid userId = User.GetUserId();
        CategoryModifyDto categoryDto = Category.ToModifyDto();

        await _service.AddCategoryAsync(categoryDto, userId);

        TempData["ToastNotification"] = "Your category was created successfully";

        return RedirectToPage("/Categories/Index", new { area = "User" });
    }
}
