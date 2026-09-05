using DNX.Extensions.DateTimes;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.DateTimes;

public class DateTimeExtensionsTests
{
    private static readonly Random Randomizer = new();

    [Fact]
    public void Unix_Epoch_returns_the_correct_value()
    {
        // Arrange

        // Act
        var epoch = DateTimeExtensions.UnixEpoch;

        // Assert
        epoch.Kind.ShouldBe(DateTimeKind.Utc);
        epoch.Year.ShouldBe(1970);
        epoch.Month.ShouldBe(1);
        epoch.Day.ShouldBe(1);
        epoch.Hour.ShouldBe(0);
        epoch.Minute.ShouldBe(0);
        epoch.Second.ShouldBe(0);
        epoch.Millisecond.ShouldBe(0);
    }

    [Fact]
    public void ParseDateAsUtc_can_parse_a_date()
    {
        // Arrange
        var theDateTime = DateTime.Now;
        var dateTimeString = theDateTime.ToLongDateString() + " " + theDateTime.ToLongTimeString();

        // Act
        var parsedDateTime = dateTimeString.ParseDateAsUtc();

        // Assert
        theDateTime.Kind.ShouldBe(DateTimeKind.Local);
        parsedDateTime.Kind.ShouldBe(DateTimeKind.Utc);
        parsedDateTime.Year.ShouldBe(theDateTime.Year);
        parsedDateTime.Month.ShouldBe(theDateTime.Month);
        parsedDateTime.Day.ShouldBe(theDateTime.Day);
        parsedDateTime.Hour.ShouldBe(theDateTime.Hour);
        parsedDateTime.Minute.ShouldBe(theDateTime.Minute);
        parsedDateTime.Second.ShouldBe(theDateTime.Second);
        parsedDateTime.Millisecond.ShouldBe(0);
    }

    [Fact]
    public void ParseDateAsUtc_with_default_value_can_parse_a_date()
    {
        // Arrange
        var defaultDateTime = DateTime.Now.Subtract(TimeSpan.FromDays(60));
        var theDateTime = DateTime.Now;
        var dateTimeString = theDateTime.ToLongDateString() + " " + theDateTime.ToLongTimeString();

        // Act
        var parsedDateTime = dateTimeString.ParseDateAsUtc(defaultDateTime);

        // Assert
        parsedDateTime.Kind.ShouldBe(DateTimeKind.Utc);
        parsedDateTime.Year.ShouldBe(theDateTime.Year);
        parsedDateTime.Month.ShouldBe(theDateTime.Month);
        parsedDateTime.Day.ShouldBe(theDateTime.Day);
        parsedDateTime.Hour.ShouldBe(theDateTime.Hour);
        parsedDateTime.Minute.ShouldBe(theDateTime.Minute);
        parsedDateTime.Second.ShouldBe(theDateTime.Second);
        parsedDateTime.Millisecond.ShouldBe(0);
    }

    [Fact]
    public void ParseDateAsUtc_with_default_value_returns_default_value_when_it_fails_to_parse_a_date()
    {
        // Arrange
        var defaultDateTime = DateTime.Now;
        var dateTimeString = "Not a datetime string";

        // Act
        var parsedDateTime = dateTimeString.ParseDateAsUtc(defaultDateTime);

        // Assert
        parsedDateTime.Kind.ShouldBe(DateTimeKind.Utc);
        parsedDateTime.Year.ShouldBe(defaultDateTime.Year);
        parsedDateTime.Month.ShouldBe(defaultDateTime.Month);
        parsedDateTime.Day.ShouldBe(defaultDateTime.Day);
        parsedDateTime.Hour.ShouldBe(defaultDateTime.Hour);
        parsedDateTime.Minute.ShouldBe(defaultDateTime.Minute);
        parsedDateTime.Second.ShouldBe(defaultDateTime.Second);
        parsedDateTime.Millisecond.ShouldBe(defaultDateTime.Millisecond);
    }

    [Fact]
    public void ParseDateAsUtc_can_parse_a_date_using_a_format_provider()
    {
        // Arrange
        var formatProvider = new CustomDateTimeFormatProvider();

        var theDateTime = DateTime.Now;
        var dateTimeString = theDateTime.ToString(CustomDateTimeFormatProvider.FormatString);

        // Act
        var parsedDateTime = dateTimeString.ParseDateAsUtc(formatProvider);

        // Assert
        theDateTime.Kind.ShouldBe(DateTimeKind.Local);
        parsedDateTime.Kind.ShouldBe(DateTimeKind.Utc);
        parsedDateTime.Year.ShouldBe(theDateTime.Year);
        parsedDateTime.Month.ShouldBe(theDateTime.Month);
        parsedDateTime.Day.ShouldBe(theDateTime.Day);
        parsedDateTime.Hour.ShouldBe(theDateTime.Hour);
        parsedDateTime.Minute.ShouldBe(theDateTime.Minute);
        parsedDateTime.Second.ShouldBe(theDateTime.Second);
        parsedDateTime.Millisecond.ShouldBe(0);
    }

    [Fact]
    public void ParseDateAsUtc_with_default_value_returns_can_parse_a_date_using_a_format_provider()
    {
        // Arrange
        var formatProvider = new CustomDateTimeFormatProvider();

        var defaultDateTime = DateTime.Now.Subtract(TimeSpan.FromDays(60));
        var theDateTime = DateTime.Now;
        var dateTimeString = theDateTime.ToString(CustomDateTimeFormatProvider.FormatString);

        // Act
        var parsedDateTime = dateTimeString.ParseDateAsUtc(formatProvider, defaultDateTime);

        // Assert
        parsedDateTime.Kind.ShouldBe(DateTimeKind.Utc);
        parsedDateTime.Year.ShouldBe(theDateTime.Year);
        parsedDateTime.Month.ShouldBe(theDateTime.Month);
        parsedDateTime.Day.ShouldBe(theDateTime.Day);
        parsedDateTime.Hour.ShouldBe(theDateTime.Hour);
        parsedDateTime.Minute.ShouldBe(theDateTime.Minute);
        parsedDateTime.Second.ShouldBe(theDateTime.Second);
        parsedDateTime.Millisecond.ShouldBe(0);
    }

    [Fact]
    public void ParseDateAsUtc_with_default_value_returns_default_value_when_it_fails_to_parse_a_date_using_a_format_provider()
    {
        // Arrange
        var formatProvider = new CustomDateTimeFormatProvider();

        var defaultDateTime = DateTime.Now;
        var dateTimeString = "Not a datetime string";

        // Act
        var parsedDateTime = dateTimeString.ParseDateAsUtc(formatProvider, defaultDateTime);

        // Assert
        parsedDateTime.Kind.ShouldBe(DateTimeKind.Utc);
        parsedDateTime.Year.ShouldBe(defaultDateTime.Year);
        parsedDateTime.Month.ShouldBe(defaultDateTime.Month);
        parsedDateTime.Day.ShouldBe(defaultDateTime.Day);
        parsedDateTime.Hour.ShouldBe(defaultDateTime.Hour);
        parsedDateTime.Minute.ShouldBe(defaultDateTime.Minute);
        parsedDateTime.Second.ShouldBe(defaultDateTime.Second);
        parsedDateTime.Millisecond.ShouldBe(defaultDateTime.Millisecond);
    }

    [Theory]
    [MemberData(nameof(MinOf_Data))]
    public void MinOf_returns_the_expected_value(DateTime dt1, DateTime dt2, DateTime expected)
    {
        var result = DateTimeExtensions.MinOf(dt1, dt2);

        result.ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(MaxOf_Data))]
    public void MaxOf_returns_the_expected_value(DateTime dt1, DateTime dt2, DateTime expected)
    {
        var result = DateTimeExtensions.MaxOf(dt1, dt2);

        result.ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(GetMonthsSpan_Data))]
    public void GetMonthsSpan_calculates_as_expected(DateTime dateTime1, DateTime dateTime2, int expectedResult)
    {
        var result = dateTime1.GetMonthsSpan(dateTime2);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(GetQuarter_Data))]
    public void GetQuarter_calculates_as_expected(DateTime dateTime, int expectedResult)
    {
        var result = dateTime.GetQuarter();

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void SetYear_can_operate_as_expected(DateTime dateTime)
    {
        var year = Randomizer.Next(1, 9999);
        var month = dateTime.Month;
        var day = dateTime.Day;

        var result = dateTime.SetYear(year);

        result.Year.ShouldBe(year);
        result.Month.ShouldBe(month);
        result.Day.ShouldBe(day);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void SetMonth_can_operate_as_expected(DateTime dateTime)
    {
        var year = dateTime.Year;
        var month = Randomizer.Next(1, 12);
        var day = dateTime.Day;

        var result = dateTime.SetMonth(month);

        result.Year.ShouldBe(year);
        result.Month.ShouldBe(month);
        result.Day.ShouldBe(day);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void SetDay_can_operate_as_expected(DateTime dateTime)
    {
        var year = dateTime.Year;
        var month = dateTime.Month;
        var day = Randomizer.Next(1, 28);

        var result = dateTime.SetDay(day);

        result.Year.ShouldBe(year);
        result.Month.ShouldBe(month);
        result.Day.ShouldBe(day);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void ResetHours_can_operate_as_expected(DateTime dateTime)
    {
        var result = dateTime.ResetHours();

        result.Hour.ShouldBe(0);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void ResetMinutes_can_operate_as_expected(DateTime dateTime)
    {
        var result = dateTime.ResetMinutes();

        result.Minute.ShouldBe(0);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void ResetSeconds_can_operate_as_expected(DateTime dateTime)
    {
        var result = dateTime.ResetSeconds();

        result.Second.ShouldBe(0);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void ResetMilliseconds_can_operate_as_expected(DateTime dateTime)
    {
        var result = dateTime.ResetMilliseconds();

        result.Millisecond.ShouldBe(0);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void SetHours_can_operate_as_expected(DateTime dateTime)
    {
        var value = Randomizer.Next(1, 24);

        var result = dateTime.SetHours(value);

        result.Hour.ShouldBe(value);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void SetMinutes_can_operate_as_expected(DateTime dateTime)
    {
        var value = Randomizer.Next(1, 60);

        var result = dateTime.SetMinutes(value);

        result.Minute.ShouldBe(value);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void SetSeconds_can_operate_as_expected(DateTime dateTime)
    {
        var value = Randomizer.Next(1, 60);

        var result = dateTime.SetSeconds(value);

        result.Second.ShouldBe(value);
    }

    [Theory]
    [MemberData(nameof(ResetDateTime_Data))]
    public void SetMilliseconds_can_operate_as_expected(DateTime dateTime)
    {
        var value = Randomizer.Next(1, 999);

        var result = dateTime.SetMilliseconds(value);

        result.Millisecond.ShouldBe(value);
    }

    public static TheoryData<DateTime, DateTime, DateTime> MinOf_Data()
    {
        var data = new TheoryData<DateTime, DateTime, DateTime>();

        var today = DateTime.UtcNow.Date;

        data.Add(today, today, today);
        data.Add(today.AddHours(12), today, today);
        data.Add(today.AddHours(12), today.AddHours(14), today.AddHours(12));
        data.Add(today.AddHours(12), today.AddHours(4), today.AddHours(4));
        data.Add(today, DateTimeExtensions.CalendarMinValue, DateTimeExtensions.CalendarMinValue);
        data.Add(today, DateTimeExtensions.CalendarMaxValue, today);

        return data;
    }

    public static TheoryData<DateTime, DateTime, DateTime> MaxOf_Data()
    {
        var data = new TheoryData<DateTime, DateTime, DateTime>();

        var today = DateTime.UtcNow.Date;

        data.Add(today, today, today);
        data.Add(today.AddHours(12), today, today.AddHours(12));
        data.Add(today.AddHours(12), today.AddHours(14), today.AddHours(14));
        data.Add(today.AddHours(12), today.AddHours(4), today.AddHours(12));
        data.Add(today, DateTimeExtensions.CalendarMaxValue, DateTimeExtensions.CalendarMaxValue);
        data.Add(today, DateTimeExtensions.CalendarMinValue, today);

        return data;
    }

    public static TheoryData<DateTime, DateTime, int> GetMonthsSpan_Data()
    {
        var list = new TheoryData<DateTime, DateTime, int>
        {
            { new DateTime(2000, 01, 01), new DateTime(2000, 12, 31), 12 },
            { new DateTime(2000, 12, 31), new DateTime(2000, 01, 01), 12 },
            { new DateTime(2000, 01, 01), new DateTime(2000, 01, 31), 1 },
            { new DateTime(2000, 01, 01), new DateTime(2000, 02, 01), 2 },
            { new DateTime(2000, 01, 01), new DateTime(2000, 01, 01), 1 },
        };


        return list;
    }

    public static TheoryData<DateTime, int> GetQuarter_Data()
    {
        var thisYear = DateTime.UtcNow.Year;

        var list = new TheoryData<DateTime, int>
        {
            { new DateTime(thisYear, 01, 01), 1 },
            { new DateTime(thisYear, 02, 01), 1 },
            { new DateTime(thisYear, 03, 01), 1 },
            { new DateTime(thisYear, 04, 01), 2 },
            { new DateTime(thisYear, 04, 01), 2 },
            { new DateTime(thisYear, 05, 01), 2 },
            { new DateTime(thisYear, 07, 01), 3 },
            { new DateTime(thisYear, 08, 01), 3 },
            { new DateTime(thisYear, 09, 01), 3 },
            { new DateTime(thisYear, 10, 01), 4 },
            { new DateTime(thisYear, 11, 01), 4 },
            { new DateTime(thisYear, 12, 01), 4 },
            { new DateTime(thisYear + 1, 01, 01), 1 },
        };
        return list;
    }

    public static TheoryData<DateTime> ResetDateTime_Data()
    {
        var data = new TheoryData<DateTime>
        {
            { DateTime.UtcNow },
            { DateTime.UnixEpoch },
            { DateTime.Parse("2021-11-05 20:53:44.12345") },
            { DateTime.Parse("2021-11-05 20:53:44") },
            { DateTime.Parse("2021-11-05 20:53") },
            { DateTime.Parse("2021-11-05") },
        };

        if (DateTime.Now.Hour != DateTime.UtcNow.Hour)
        {
            data.Add(DateTime.Now);
        }

        return data;
    }
}
