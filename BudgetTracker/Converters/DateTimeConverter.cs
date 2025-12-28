using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetTracker.Converters;

/// <summary>
/// Converts the DateTime to UTC for database storage and back to UTC DateTime when retrieved.
/// </summary>
public class DateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeConverter() : base(
        v => v.ToUniversalTime(),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    { }
}
