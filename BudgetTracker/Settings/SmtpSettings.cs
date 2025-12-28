namespace BudgetTracker.Settings;

public class SmtpSettings
{
    public const string SECTION_NAME = "Smtp";
    public bool UsePickupDirectory { get; set; }
    public string? PickupDirectory { get; set; }
    public required string FromAddress { get; set; }
    public required string Host { get; set; }
    public int Port { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}
