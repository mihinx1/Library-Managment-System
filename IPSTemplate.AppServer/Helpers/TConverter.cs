using System.ComponentModel;
using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace IPSTemplate.AppServer.Helpers;

public static class TConverter
{
    public static T? ChangeType<T>(object value) => (T?)ChangeType(typeof(T), value);

    public static bool TryChangeType<T>(object value, out T? result)
    {
        if (TryChangeType(typeof(T), value, out var convertedValue))
        {
            result = (T?)convertedValue;
            return true;
        }

        result = default;
        return false;
    }

    public static object? ChangeType(Type type, object value)
    {
        if (type.Equals(value.GetType()))
        {
            return value;
        }

        TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
        return typeConverter.ConvertFrom(value);
    }

    public static bool TryChangeType(Type type, object value, out object? result)
    {
        Type sourceType = value.GetType();
        if (type.Equals(sourceType))
        {
            result = value;
            return true;
        }

        TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
        if (typeConverter.CanConvertFrom(sourceType))
        {
            result = typeConverter.ConvertFrom(value);
            return true;
        }

        result = default;
        return false;
    }

    public static void RegisterTypeConverter<T, TConverter>() where TConverter : TypeConverter
    {
        TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TConverter)));
    }
}

public class CustomStringConverter : StringConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(StringValues) || base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(StringValues) || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value is StringValues stringValues
            ? stringValues.ToString()
            : base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return destinationType == typeof(StringValues)
            ? new StringValues((string?)value)
            : base.ConvertTo(context, culture, value, destinationType);
    }
}