using BudgetTracker.Models.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BudgetTracker.Models.ViewModels;

/// <summary>
/// Used to collect and validate data from the edit and create views for catgories
/// </summary>
public class CategoryViewModel
{
    public Guid? CategoryId { get; set; }

    [DisplayName("Category")]
    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    [RegularExpression(@"[A-Za-z0-9\s\-_]+", ErrorMessage = "Please only use alphanumeric characters and/or (- _).")]
    public required string Name { get; set; }

    [DisplayName(nameof(Description))]
    [MaxLength(500, ErrorMessage = ValidationMessages.MaxLength)]
    public string? Description { get; set; }

    [DisplayName(nameof(Icon))]
    [RegularExpression(@"[A-Za-z-]*")]
    [MaxLength(50, ErrorMessage = ValidationMessages.MaxLength)]
    public string? Icon { get; set; }

    [DisplayName("Monthly Limit")]
    [RegularExpression(@"\$[0-9,]{1,6}\.[0-9]{2}", ErrorMessage = "Please enter a value between 0 and $99,999.99")]
    public string? MonthlyLimitDisplay { get; set; }

    [BindNever]
    public decimal? MonthlyLimit
    {
        get
        {
            if (decimal.TryParse(MonthlyLimitDisplay, NumberStyles.Currency, CultureInfo.CurrentCulture, out var result))
            {
                // Just set to null by default if 0 is entered
                if (result == 0)
                {
                    return null;
                }

                return result;
            }

            // Failure to parse results in a null budget; bad input
            return null;
        }
    }
}
