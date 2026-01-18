using BudgetTracker.Extensions;
using BudgetTracker.Models.Constants;
using BudgetTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Areas.User.Pages;

[Authorize]
public class IndexModel(IDashboardService dashboardService) : PageModel
{
    private readonly IDashboardService _dashboardService = dashboardService;

    /// <summary>
    /// Retrieves the data for the user dashboard display
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnGetDashboard(string view)
    {
        // Declare the start date
        DateOnly? startDate;

        // Default view is year to date
        view ??= ChartDisplayColumnType.YearToDate;

        // Determine the start date based on the view
        if (view == ChartDisplayColumnType.YearToDate)
        {
            startDate = new DateOnly(DateTime.Now.Year, 1, 1);
        }
        else if (view == ChartDisplayColumnType.Year)
        {
            startDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-1));
        }
        else
        {
            startDate = null;
        }

        var result = await _dashboardService.GetChartDataAsync(User.GetUserId(), startDate);

        // Create and send dashboard view model
        return new JsonResult(result);
    }
}
