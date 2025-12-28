namespace BudgetTracker.Settings;

public class ApplicationSettings
{
    public const string SECTION_NAME = "Application";
    public required string Name { get; set; }
    public int MaxCategories { get; set; }
}
