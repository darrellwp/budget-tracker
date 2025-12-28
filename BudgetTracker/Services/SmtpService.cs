using BudgetTracker.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BudgetTracker.Services;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <param name="smtpSettings"></param>
public class SmtpService(IOptions<SmtpSettings> smtpSettings) : ISmtpService
{
    private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

    public Task SendEmailAsync(string to, string subject, string body)
    {
        MailMessage message = new(_smtpSettings.FromAddress, to, subject, body);

        using (SmtpClient smtp = new())
        {
            if (_smtpSettings.UsePickupDirectory)
            {
                if (_smtpSettings.PickupDirectory != null)
                {
                    Directory.CreateDirectory(_smtpSettings.PickupDirectory);

                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtp.PickupDirectoryLocation = _smtpSettings.PickupDirectory;
                }       
            }
            else
            {
                smtp.Host = _smtpSettings.Host;
                smtp.Port = _smtpSettings.Port;
                smtp.Credentials = new NetworkCredential(
                    _smtpSettings.UserName,
                    _smtpSettings.Password
                );
                smtp.EnableSsl = true;
            }

            smtp.Send(message);
        }

        return Task.CompletedTask;
    }
}
