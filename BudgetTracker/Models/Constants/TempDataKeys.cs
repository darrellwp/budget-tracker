namespace BudgetTracker.Models.Constants;

/// <summary>
/// Reusable temp data keys throughout the application
/// </summary>
public static class TempDataKeys
{
    /// <summary>
    /// Used to trigger a toast notification via server side to the client
    /// </summary>
    public const string ToastNotification = "ToastNotification";

    /// <summary>
    /// Used to store the return URL for redirection after an operation
    /// This keeps it server side and avoids open redirect vulnerabilities
    /// </summary>
    public const string ReturnUrl = "ReturnUrl";
}
