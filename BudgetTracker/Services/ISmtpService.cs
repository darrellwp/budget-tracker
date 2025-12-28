namespace BudgetTracker.Services;

/// <summary>
/// Service to handle SMTP actions such as sending account confirmation, password resets, and notifications.
/// </summary>
public interface ISmtpService
{
    /// <summary>
    /// Sends a generic email
    /// </summary>
    /// <param name="to">Address to sent to</param>
    /// <param name="subject">Primary subject</param>
    /// <param name="body">Body of the email</param>
    /// <returns></returns>
    Task SendEmailAsync(string to, string subject, string body);
}

