using DNX.Extensions.DateTimes;
using FluentAssertions;
using Xunit;

namespace DNX.Extensions.Tests.DateTimes
{
    public class DateTimeExtensionsTests
    {
        private static readonly Random Randomizer = new();

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

        public static IEnumerable<object[]> ResetDateTime_Data()
        {
            return new List<object[]>()
            {
                new object[] { DateTime.UtcNow },
                new object[] { DateTime.Now },
                new object[] { DateTime.UnixEpoch },
                new object[] { DateTime.Parse("2021-11-05 20:53:44.12345") },
                new object[] { DateTime.Parse("2021-11-05 20:53:44") },
                new object[] { DateTime.Parse("2021-11-05 20:53") },
                new object[] { DateTime.Parse("2021-11-05 00:00") },
                new object[] { DateTime.Parse("2021-11-05") },
            };
        }
    }
}
