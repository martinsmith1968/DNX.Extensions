using DNX.Extensions.DateTimes;
using FluentAssertions;
using Xunit;

namespace DNX.Extensions.Tests.DateTimes
{
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
            epoch.Kind.Should().Be(DateTimeKind.Utc);
            epoch.Year.Should().Be(1970);
            epoch.Month.Should().Be(1);
            epoch.Day.Should().Be(1);
            epoch.Hour.Should().Be(0);
            epoch.Minute.Should().Be(0);
            epoch.Second.Should().Be(0);
            epoch.Millisecond.Should().Be(0);
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
            theDateTime.Kind.Should().Be(DateTimeKind.Local);
            parsedDateTime.Kind.Should().Be(DateTimeKind.Utc);
            parsedDateTime.Year.Should().Be(theDateTime.Year);
            parsedDateTime.Month.Should().Be(theDateTime.Month);
            parsedDateTime.Day.Should().Be(theDateTime.Day);
            parsedDateTime.Hour.Should().Be(theDateTime.Hour);
            parsedDateTime.Minute.Should().Be(theDateTime.Minute);
            parsedDateTime.Second.Should().Be(theDateTime.Second);
            parsedDateTime.Millisecond.Should().Be(0);
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
            parsedDateTime.Kind.Should().Be(DateTimeKind.Utc);
            parsedDateTime.Year.Should().Be(theDateTime.Year);
            parsedDateTime.Month.Should().Be(theDateTime.Month);
            parsedDateTime.Day.Should().Be(theDateTime.Day);
            parsedDateTime.Hour.Should().Be(theDateTime.Hour);
            parsedDateTime.Minute.Should().Be(theDateTime.Minute);
            parsedDateTime.Second.Should().Be(theDateTime.Second);
            parsedDateTime.Millisecond.Should().Be(0);
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
            parsedDateTime.Kind.Should().Be(DateTimeKind.Utc);
            parsedDateTime.Year.Should().Be(defaultDateTime.Year);
            parsedDateTime.Month.Should().Be(defaultDateTime.Month);
            parsedDateTime.Day.Should().Be(defaultDateTime.Day);
            parsedDateTime.Hour.Should().Be(defaultDateTime.Hour);
            parsedDateTime.Minute.Should().Be(defaultDateTime.Minute);
            parsedDateTime.Second.Should().Be(defaultDateTime.Second);
            parsedDateTime.Millisecond.Should().Be(defaultDateTime.Millisecond);
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
            theDateTime.Kind.Should().Be(DateTimeKind.Local);
            parsedDateTime.Kind.Should().Be(DateTimeKind.Utc);
            parsedDateTime.Year.Should().Be(theDateTime.Year);
            parsedDateTime.Month.Should().Be(theDateTime.Month);
            parsedDateTime.Day.Should().Be(theDateTime.Day);
            parsedDateTime.Hour.Should().Be(theDateTime.Hour);
            parsedDateTime.Minute.Should().Be(theDateTime.Minute);
            parsedDateTime.Second.Should().Be(theDateTime.Second);
            parsedDateTime.Millisecond.Should().Be(0);
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
            parsedDateTime.Kind.Should().Be(DateTimeKind.Utc);
            parsedDateTime.Year.Should().Be(theDateTime.Year);
            parsedDateTime.Month.Should().Be(theDateTime.Month);
            parsedDateTime.Day.Should().Be(theDateTime.Day);
            parsedDateTime.Hour.Should().Be(theDateTime.Hour);
            parsedDateTime.Minute.Should().Be(theDateTime.Minute);
            parsedDateTime.Second.Should().Be(theDateTime.Second);
            parsedDateTime.Millisecond.Should().Be(0);
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
            parsedDateTime.Kind.Should().Be(DateTimeKind.Utc);
            parsedDateTime.Year.Should().Be(defaultDateTime.Year);
            parsedDateTime.Month.Should().Be(defaultDateTime.Month);
            parsedDateTime.Day.Should().Be(defaultDateTime.Day);
            parsedDateTime.Hour.Should().Be(defaultDateTime.Hour);
            parsedDateTime.Minute.Should().Be(defaultDateTime.Minute);
            parsedDateTime.Second.Should().Be(defaultDateTime.Second);
            parsedDateTime.Millisecond.Should().Be(defaultDateTime.Millisecond);
        }



        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void SetYear_can_operate_as_expected(DateTime dateTime)
        {
            var year = Randomizer.Next(1, 9999);
            var month = dateTime.Month;
            var day = dateTime.Day;

            var result = dateTime.SetYear(year);

            result.Year.Should().Be(year);
            result.Month.Should().Be(month);
            result.Day.Should().Be(day);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void SetMonth_can_operate_as_expected(DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = Randomizer.Next(1, 12);
            var day = dateTime.Day;

            var result = dateTime.SetMonth(month);

            result.Year.Should().Be(year);
            result.Month.Should().Be(month);
            result.Day.Should().Be(day);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void SetDay_can_operate_as_expected(DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month;
            var day = Randomizer.Next(1, 28);

            var result = dateTime.SetDay(day);

            result.Year.Should().Be(year);
            result.Month.Should().Be(month);
            result.Day.Should().Be(day);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void ResetHours_can_operate_as_expected(DateTime dateTime)
        {
            var result = dateTime.ResetHours();

            result.Hour.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void ResetMinutes_can_operate_as_expected(DateTime dateTime)
        {
            var result = dateTime.ResetMinutes();

            result.Minute.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void ResetSeconds_can_operate_as_expected(DateTime dateTime)
        {
            var result = dateTime.ResetSeconds();

            result.Second.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void ResetMilliseconds_can_operate_as_expected(DateTime dateTime)
        {
            var result = dateTime.ResetMilliseconds();

            result.Millisecond.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void SetHours_can_operate_as_expected(DateTime dateTime)
        {
            var value = Randomizer.Next(1, 24);

            var result = dateTime.SetHours(value);

            result.Hour.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void SetMinutes_can_operate_as_expected(DateTime dateTime)
        {
            var value = Randomizer.Next(1, 60);

            var result = dateTime.SetMinutes(value);

            result.Minute.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void SetSeconds_can_operate_as_expected(DateTime dateTime)
        {
            var value = Randomizer.Next(1, 60);

            var result = dateTime.SetSeconds(value);

            result.Second.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(ResetDateTime_Data))]
        public void SetMilliseconds_can_operate_as_expected(DateTime dateTime)
        {
            var value = Randomizer.Next(1, 999);

            var result = dateTime.SetMilliseconds(value);

            result.Millisecond.Should().Be(value);
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
}
