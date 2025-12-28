using BudgetTracker.Attributes;
using BudgetTracker.Models.Constants;
using BudgetTracker.Models.Enumerations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BudgetTracker.Models.ViewModels;

/// <summary>
/// Used to collect and validate data from the transaction edit and create pages
/// </summary>
public class TransactionViewModel
{
    public Guid? TransactionId { get; set; }

    [Display(Name = "Transaction For")]
    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string? Description { get; set; }

    [Display(Name = "Place of purchase")]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string? PlaceOfPurchase { get; set; }

    [Display(Name = "Type")]
    [Required(ErrorMessage = ValidationMessages.Required)]
    public TransactionType TransactionType { get; set; }

    [Display(Name = "Date")]
    [Required(ErrorMessage = ValidationMessages.Required)]
    [DateOnlyRange("2000-01-01", "3000-01-01", ErrorMessage = ValidationMessages.RangeValue)]
    public DateOnly? DateOccurred { get; set; }

    [Display(Name = "Category")]
    public Guid? CategoryId { get; set; }

    [Display(Name = "Amount")]
    [Required(ErrorMessage = ValidationMessages.Required)]
    [RegularExpression(@"\$[0-9,]{1,6}\.[0-9]{2}", ErrorMessage = "Please enter a value between 0 and $99,999.99")]
    public string? AmountDisplay { get; set; }

    [BindNever]
    public decimal Amount
    {
        get
        {
            if (decimal.TryParse(AmountDisplay, NumberStyles.Currency, CultureInfo.CurrentCulture, out var result))
            {
                return result;
            }

            // Failure to parse results; bad input
            return 0;
        }
    }

    public string? ReturnUrl { get; set; }

    public IEnumerable<SelectListItem>? CategoryOptions { get; set; }

    public IEnumerable<string>? PurchaseLocations { get; set; }
}
