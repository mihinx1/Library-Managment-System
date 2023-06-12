using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.DALEF.ContextFactory;

internal class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter() : base(
        toProvider => toProvider,
        fromProvider => DateTime.SpecifyKind(fromProvider, DateTimeKind.Utc))
    {
    }
}

internal class StrictUtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    // TODO:
    // value.Kind always comes in as Unspecified. Check copying from BO to EF and other possible causes for mutation.
    // Use UtcDateTimeConverter rather than StrictUtcDateTimeConverter for now.
    private static DateTime ToProvider(DateTime value, string name) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        _ => throw new InvalidOperationException($"Column '{name}' only accepts UTC date-time values")
    };

    private static DateTime FromProvider(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => throw new InvalidOperationException($"Unsupported date-time kind: {value.Kind}")
    };

    public StrictUtcDateTimeConverter(IMutableProperty property) : base(
        toProvider => ToProvider(toProvider, property.Name),
        fromProvider => FromProvider(fromProvider))
    {
    }
}
