using BudgetTracker.Models.DTOs;
using BudgetTracker.Data.Entities;

namespace BudgetTracker.Services;

public interface IDashboardService
{
    /// <summary>
    /// Gets the combined data sets for the dashboard display
    /// </summary>
    /// <param name="userId"><see cref="ApplicationUser.UserId"/></param>
    /// <param name="start">Date to start grouping data in</param>
    /// <returns></returns>
    Task<DashboardDataDto> GetChartDataAsync(Guid userId, DateOnly? start);
}
