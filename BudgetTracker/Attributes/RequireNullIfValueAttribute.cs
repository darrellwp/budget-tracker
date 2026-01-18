using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BudgetTracker.Attributes;

/// <summary>
/// Validates that a property is null if another property has a specific value
/// </summary>
/// <param name="propertyName">The propety to compare to</param>
/// <param name="value">The value to match</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class RequireNullIfValueAttribute(string propertyName, object value) : ValidationAttribute
{
    public string PropertyName { get; set; } = propertyName;
    public object Value { get; set; } = value;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        PropertyInfo property = validationContext.ObjectType.GetProperty(PropertyName) ?? throw new ArgumentNullException($"{nameof(PropertyName)} must be a valid property on {validationContext.ObjectType.Name}");

        var propertyValue = property.GetValue(validationContext.ObjectInstance);

        if (propertyValue != null && propertyValue.Equals(Value) && value != null)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
