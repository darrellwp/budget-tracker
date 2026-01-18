using BudgetTracker.Data.Repositories;
using BudgetTracker.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Services;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <param name="dbContext"></param>
public class DashboardService(ITransactionRepository transactionRepository) : IDashboardService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;

    public async Task<DashboardDataDto> GetChartDataAsync(Guid userId, DateOnly? start)
    {
        // Get the users start of time transactions
        start ??= await _transactionRepository.GetUserFirstTransactionDateAsync(userId);

        // The end date will always be today's date for these calls
        DateOnly end = DateOnly.FromDateTime(DateTime.Now);
        string columnFormat = "yyyy";

        DashboardDataDto chartData = new();

        // Don't bother queries if the start doesn't exist
        if (start != null)
        {
            var daysDifference = end.DayNumber - start.Value.DayNumber;
            IEnumerable<GroupedTransactionsSumDto> balanceSums;
            IEnumerable<GroupedTransactionsCategoryDto> categorySums;

            // Determine the grouping based on the date range
            if (daysDifference <= 56)
            {
                balanceSums = await _transactionRepository.GetUserTransactionsGroupedByWeekAsync(userId, start.Value, end);
                categorySums = await _transactionRepository.GetUserTransactionsGroupedByCategoryWeekAsync(userId, start.Value, end);
                columnFormat = "MMM-dd";
            }
            else if (daysDifference <= 730)
            {
                balanceSums = await _transactionRepository.GetUserTransactionsGroupedByMonthAsync(userId, start.Value, end);
                categorySums = await _transactionRepository.GetUserTransactionsGroupedByCategoryMonthAsync(userId, start.Value, end);
                columnFormat = "MMM-yy";
            }
            else
            {
                balanceSums = await _transactionRepository.GetUserTransactionsGroupedByYearAsync(userId, start.Value, end);
                categorySums = await _transactionRepository.GetUserTransactionsGroupedByCategoryYearAsync(userId, start.Value, end);
                columnFormat = "yyyy";
            }

            // Gets the initial value for the balance chart
            decimal startingValue = await _transactionRepository.GetUserTransactionSumUntilDateAsync(userId, start.Value);

            // List of data points - utilize forloop for building balance
            List<BalanceChartDataDto> balanceDataPoints = [];

            // Generate the balance data points ready for echarts
            foreach (var (value, index) in balanceSums.Select((value, index) => (value, index)))
            {
                var previous = index == 0 ? startingValue : balanceDataPoints[^1].Amount;

                // Add the data point
                balanceDataPoints.Add(new()
                {
                    Label = (value.Week != null ? start.Value.AddDays(value.Week.Value * 7) : new DateOnly(value.Year, value.Month ?? 1, 1)).ToString(columnFormat),
                    Amount = previous + value.OverallTotal,
                    Expense = value.ExpenseTotal,
                    Income = value.IncomeTotal
                });
            }

            // Build the category data points
            List<CategoryChartDataDto> categoryDataPoints = [];

            // Get a distinct list of categories since some have no data for certain time periods
            var categoryList = categorySums.Select(x => x.Category).Distinct();

            // Group by the time periods again to build out the category data points
            foreach (var groupedData in categorySums.GroupBy(x => new { x.Year, x.Month, x.Week }))
            {
                List<CategoryChartDataGroupDto> categorySetData = [];

                // Ensure all categories are represented in each time period
                foreach (var category in categoryList)
                {
                    categorySetData.Add(new CategoryChartDataGroupDto
                    {
                        Category = category,
                        Amount = groupedData.Where(x => x.Category == category).Sum(x => x.Total)
                    });
                }

                // Add the set
                categoryDataPoints.Add(new CategoryChartDataDto()
                {
                    Label = (groupedData.Key.Week != null ? start.Value.AddDays(groupedData.Key.Week.Value * 7) : new DateOnly(groupedData.Key.Year, groupedData.Key.Month ?? 1, 1)).ToString(columnFormat),
                    Amounts = categorySetData
                });
            }

            // Set the response data generated above
            chartData.CategoryLabels = categoryList;
            chartData.Balances = balanceDataPoints;
            chartData.Categories = categoryDataPoints;
        }

        return chartData;
    }
}