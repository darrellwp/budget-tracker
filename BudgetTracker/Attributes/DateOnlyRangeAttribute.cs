using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BudgetTracker.Attributes;

/// <summary>
/// Test the range of a DateOnly value field
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class DateOnlyRangeAttribute(string minimum, string maximum) : ValidationAttribute, IClientModelValidator
{
    public DateOnly Minimum { get; } = DateOnly.Parse(minimum, new CultureInfo("en-US"));
    public DateOnly Maximum { get; } = DateOnly.Parse(maximum, new CultureInfo("en-US"));

    public override bool IsValid(object? value)
    {
        if (value is DateOnly dateValue)
        {
            return dateValue >= Minimum && dateValue <= Maximum;
        }
        return false;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        string errorMessage = $"{context.ModelMetadata.DisplayName} must be between {Minimum:MM/dd/yyyy} and {Maximum:MM/dd/yyyy}";

        context.Attributes.Add("data-val-daterange", errorMessage);
        context.Attributes.Add("data-val-daterange-min", Minimum.ToString("yyyy-MM-dd"));
        context.Attributes.Add("data-val-daterange-max", Maximum.ToString("yyyy-MM-dd"));
    }
}
