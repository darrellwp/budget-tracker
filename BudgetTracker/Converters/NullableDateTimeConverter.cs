using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetTracker.Converters;

/// <summary>
/// Converts the nullable DateTime to UTC for database storage and back to UTC DateTime when retrieved.
/// </summary>
public class NullableDateTimeConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeConverter() : base(
    v => v.HasValue ? (v.Value.ToUniversalTime()) : v,
    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v)
    { }
}
