namespace BudgetTracker.Models.Constants;

/// <summary>
/// Reusable set of validation messages
/// This also helps to keep the messages consistent
/// </summary>
public static class ValidationMessages
{
    public const string Required = "{0} is required.";
    public const string MaxLength = "{0} maximum length is {1} characters.";
    public const string MinMaxLength = "{0} must be between {2} and {1} characters.";
    public const string RangeValue = "{0} must be between {1} and {2}.";
    public const string AlphanumericWithSpecialChars = "Please only use alphanumeric characters and/or (- _).";
}
