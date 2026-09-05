using System.Globalization;

namespace DNX.Extensions.Tests.DateTimes;

internal class CustomDateTimeFormatProvider : IFormatProvider, ICustomFormatter
{
    public const string FormatString = "dd-MMM-yyyy HH:mm:ss";

    public object GetFormat(Type formatType)
    {
        return (formatType == typeof(DateTimeFormatInfo)) ? this : null;
    }

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        if (arg is DateTime dt)
        {
            // if user supplied own format use it
            return string.IsNullOrEmpty(format)
                ? dt.ToString(FormatString)
                : dt.ToString(format);
        }

        // format everything else normally
        return arg is IFormattable formattable
            ? formattable.ToString(format, formatProvider)
            : arg.ToString();
    }
}
