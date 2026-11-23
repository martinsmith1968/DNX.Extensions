using System;
using System.Linq;
using DNX.Extensions.Execution;
using DNX.Extensions.Linq;

namespace DNX.Extensions.DateTimes;

/// <summary>
/// Date Time Extensions
/// </summary>
public static class DateTimeExtensions
{
    public static readonly DateTime CalendarMinValue = new(1953, 1, 1);
    public static readonly DateTime CalendarMaxValue = new(9998, 12, 31);

    /// <summary>
    /// Gets the unix epoch.
    /// </summary>
    /// <value>The unix epoch.</value>
    public static DateTime UnixEpoch
    {
        get { return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); }
    }

    /// <summary>
    /// Parses the date as UTC.
    /// </summary>
    /// <param name="dateString">The date string.</param>
    /// <returns>DateTime.</returns>
    public static DateTime ParseDateAsUtc(this string dateString)
    {
        var dateTime = DateTime.Parse(dateString);

        var dateTimeUtc = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

        return dateTimeUtc;
    }

    /// <summary>
    /// Parses the date as UTC.
    /// </summary>
    /// <param name="dateString">The date string.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>DateTime.</returns>
    public static DateTime ParseDateAsUtc(this string dateString, IFormatProvider formatProvider)
    {
        var dateTime = DateTime.Parse(dateString, formatProvider);

        var dateTimeUtc = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

        return dateTimeUtc;
    }

    /// <summary>
    /// Parses the date as UTC.
    /// </summary>
    /// <param name="dateString">The date string.</param>
    /// <param name="defaultDateTime">The default date time.</param>
    /// <param name="exceptionHandler">Invoked with the exception if parsing fails, instead of discarding it silently.</param>
    /// <returns>DateTime.</returns>
    public static DateTime ParseDateAsUtc(this string dateString, DateTime defaultDateTime, Action<Exception> exceptionHandler = null)
    {
        return RunSafely.Execute(
            () => ParseDateAsUtc(dateString),
            DateTime.SpecifyKind(defaultDateTime, DateTimeKind.Utc),
            exceptionHandler);
    }

    /// <summary>
    /// Parses the date as UTC.
    /// </summary>
    /// <param name="dateString">The date string.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <param name="defaultDateTime">The default date time.</param>
    /// <param name="exceptionHandler">Invoked with the exception if parsing fails, instead of discarding it silently.</param>
    /// <returns>System.DateTime.</returns>
    public static DateTime ParseDateAsUtc(this string dateString, IFormatProvider formatProvider, DateTime defaultDateTime, Action<Exception> exceptionHandler = null)
    {
        return RunSafely.Execute(
            () => ParseDateAsUtc(dateString, formatProvider),
            DateTime.SpecifyKind(defaultDateTime, DateTimeKind.Utc),
            exceptionHandler);
    }

    /// <summary>
    /// Returns the minimum of the given dates.
    /// </summary>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static DateTime MinOf(params DateTime[] values)
    {
        return values.HasAny()
            ? values.Min()
            : DateTime.MinValue;
    }

    /// <summary>
    /// Returns the maximum of the given dates.
    /// </summary>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static DateTime MaxOf(params DateTime[] values)
    {
        return values.HasAny()
            ? values.Max()
            : DateTime.MaxValue;
    }

    /// <summary>
    /// Gets the number of months spanned by the two dates.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="other">The other.</param>
    /// <returns></returns>
    public static int GetMonthsSpan(this DateTime dateTime, DateTime other)
    {
        var startDate = MinOf(dateTime, other);
        var endDate = MaxOf(dateTime, other);

        var startNumber = (startDate.Year * 100) + startDate.Month;
        var endNumber = (endDate.Year * 100) + endDate.Month;

        return (endNumber - startNumber) + 1;
    }

    /// <summary>
    /// Gets the Year Quarter
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns>The Quarter (1-4), or 0 on error</returns>
    public static int GetQuarter(this DateTime dateTime)
    {
        return dateTime.Month switch
        {
            1 or 2 or 3 => 1,
            4 or 5 or 6 => 2,
            7 or 8 or 9 => 3,
            _ => 4
        };
    }

    /// <summary>
    /// Sets the year.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    public static DateTime SetYear(this DateTime dateTime, int year)
    {
        return dateTime.Year == year
            ? dateTime
            : dateTime.AddYears(year - dateTime.Year);
    }

    /// <summary>
    /// Sets the month.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="month">The month.</param>
    /// <param name="maintainYear">if set to <c>true</c> [maintain year].</param>
    /// <param name="maintainDay">if set to <c>true</c> [maintain day].</param>
    /// <returns></returns>
    public static DateTime SetMonth(this DateTime dateTime, int month, bool maintainYear = true, bool maintainDay = true)
    {
        if (dateTime.Month == month)
            return dateTime;

        // A month-only change can never alter the year on its own, so maintainYear has no
        // distinct effect here - it's accepted purely for signature symmetry with SetDay.
        var year = dateTime.Year;
        var day = maintainDay
            ? Math.Min(dateTime.Day, DateTime.DaysInMonth(year, month))
            : 1;

        return new DateTime(year, month, day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, dateTime.Kind);
    }

    /// <summary>
    /// Sets the day.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="day">The day.</param>
    /// <param name="maintainYear">if set to <c>true</c> [maintain year].</param>
    /// <param name="maintainMonth">if set to <c>true</c> [maintain month].</param>
    /// <returns></returns>
    public static DateTime SetDay(this DateTime dateTime, int day, bool maintainYear = true, bool maintainMonth = true)
    {
        if (dateTime.Day == day)
            return dateTime;

        if (maintainMonth)
        {
            // Clamp rather than roll over - e.g. SetDay(31) on an April date lands on April 30,
            // not May 1.
            var clampedDay = Math.Min(day, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));

            return new DateTime(dateTime.Year, dateTime.Month, clampedDay, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, dateTime.Kind);
        }

        // Not maintaining the month: treat "day" as a day-of-month offset allowed to roll into
        // adjacent months/years.
        var year = dateTime.Year;
        var result = dateTime.AddDays(day - dateTime.Day);

        return maintainYear
            ? result.SetYear(year)
            : result;
    }

    /// <summary>
    /// Resets the hours on a DateTime
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    public static DateTime ResetHours(this DateTime dateTime)
    {
        return dateTime.Subtract(TimeSpan.FromHours(dateTime.Hour));
    }

    /// <summary>
    /// Resets the minutes on a DateTime
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    public static DateTime ResetMinutes(this DateTime dateTime)
    {
        return dateTime.Subtract(TimeSpan.FromMinutes(dateTime.Minute));
    }

    /// <summary>
    /// Resets the seconds on a DateTime
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    public static DateTime ResetSeconds(this DateTime dateTime)
    {
        return dateTime.Subtract(TimeSpan.FromSeconds(dateTime.Second));
    }

    /// <summary>
    /// Resets the milliseconds on a DateTime
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    public static DateTime ResetMilliseconds(this DateTime dateTime)
    {
        return dateTime.Subtract(TimeSpan.FromMilliseconds(dateTime.Millisecond));
    }

    /// <summary>
    /// Sets the hours on a DateTime
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="hours">The hours.</param>
    /// <returns></returns>
    public static DateTime SetHours(this DateTime dateTime, int hours)
    {
        return dateTime.ResetHours()
            .AddHours(hours);
    }

    /// <summary>
    /// Sets the minutes on a DateTime
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="minutes">The minutes.</param>
    /// <returns></returns>
    public static DateTime SetMinutes(this DateTime dateTime, int minutes)
    {
        return dateTime.ResetMinutes()
            .AddMinutes(minutes);
    }

    /// <summary>
    /// Sets the seconds on a DateTime
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="seconds">The seconds.</param>
    /// <returns></returns>
    public static DateTime SetSeconds(this DateTime dateTime, int seconds)
    {
        return dateTime.ResetSeconds()
            .AddSeconds(seconds);
    }

    /// <summary>
    /// Sets the milliseconds on a DateTime
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="milliseconds">The milliseconds.</param>
    /// <remarks>Precision on a <see cref="DateTime"/> means this only works for values up to 999</remarks>
    /// <returns></returns>
    public static DateTime SetMilliseconds(this DateTime dateTime, double milliseconds)
    {
        return dateTime.ResetMilliseconds()
            .AddMilliseconds(milliseconds);
    }
}
