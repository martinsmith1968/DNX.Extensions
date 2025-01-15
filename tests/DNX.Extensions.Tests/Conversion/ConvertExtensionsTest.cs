using DNX.Extensions.Conversion;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Conversion;

public class ConvertExtensionsTests
{
    public enum Numbers
    {
        Zero = 0,
        One,
        Two,
        Three,
        Four,
        Five
    }

    public class TestClass1 { }
    public class TestClass2 : TestClass1 { }
    public class TestClass3 { }

    public class ToStringOrDefault
    {
        [Theory]
        [InlineData(1, "1")]
        [InlineData(12.34, "12.34")]
        [InlineData(true, "True")]
        [InlineData((string)null, "")]
        public void ToStringOrDefault_without_override_can_convert_successfully(object instance, string expected)
        {
            var result = instance.ToStringOrDefault();

            result.ShouldBe(expected);
        }

        [Theory]
        [InlineData(1, "bob", "1")]
        [InlineData(12.34, "bob", "12.34")]
        [InlineData(true, "bob", "True")]
        [InlineData((string)null, "bob", "bob")]
        public void ToStringOrDefault_with_override_can_convert_successfully(object instance, string defaultValue, string expected)
        {
            var result = instance.ToStringOrDefault(defaultValue);

            result.ShouldBe(expected);
        }
    }

    public class ToBoolean
    {
        [Theory]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("TrUe", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        [InlineData("FaLsE", false)]
        [InlineData("NotABoolean", false)]
        public void ToBoolean_without_override_can_convert_successfully(string text, bool expected)
        {
            var result = text.ToBoolean();

            result.ShouldBe(expected);
        }

        [Theory]
        [InlineData("true", false, true)]
        [InlineData("TRUE", false, true)]
        [InlineData("TrUe", false, true)]
        [InlineData("false", true, false)]
        [InlineData("FALSE", true, false)]
        [InlineData("FaLsE", true, false)]
        [InlineData("NotABoolean", false, false)]
        [InlineData("NotABoolean", true, true)]
        public void ToBoolean_with_override_can_convert_successfully(string text, bool defaultValue, bool expected)
        {
            var result = text.ToBoolean(defaultValue);

            result.ShouldBe(expected);
        }
    }

    public class ToInt32
    {
        [Theory]
        [InlineData("160", 160)]
        [InlineData("0", 0)]
        [InlineData("-1", -1)]
        [InlineData("2147483647", 2147483647)]
        [InlineData("2147483648", 0)]
        [InlineData("NotAnInt32", 0)]
        public void ToInt32_without_override_can_convert_successfully(string text, int expected)
        {
            var result = text.ToInt32();

            result.ShouldBe(expected);
        }

        [Theory]
        [InlineData("160", 42, 160)]
        [InlineData("0", 57, 0)]
        [InlineData("-1", 5, -1)]
        [InlineData("2147483647", 12345, 2147483647)]
        [InlineData("2147483648", 12345, 12345)]
        [InlineData("NotAnInt32", 0, 0)]
        [InlineData("NotAnInt32", 222, 222)]
        public void ToInt32_with_override_can_convert_successfully(string text, int defaultValue, int expected)
        {
            var result = text.ToInt32(defaultValue);

            result.ShouldBe(expected);
        }
    }

    public class ToEnum
    {
        [Theory]
        [InlineData("One", Numbers.One)]
        [InlineData("FOUR", Numbers.Four)]
        [InlineData("THREE", Numbers.Three)]
        [InlineData("BOB", Numbers.Zero)]
        public void ToEnum_without_override_can_convert_Numbers_enum_successfully(string text, Numbers expected)
        {
            var result = text.ToEnum<Numbers>();

            result.ShouldBe(expected);
            ;
        }

        [Theory]
        [InlineData("One", Numbers.Five, Numbers.One)]
        [InlineData("FOUR", Numbers.Five, Numbers.Four)]
        [InlineData("THREE", Numbers.Five, Numbers.Three)]
        [InlineData("BOB", Numbers.Five, Numbers.Five)]
        public void ToEnum_with_override_can_convert_Numbers_enum_successfully(string text, Numbers defaultValue, Numbers expected)
        {
            var result = text.ToEnum<Numbers>(defaultValue);

            result.ShouldBe(expected);
            ;
        }
    }

    public class ToGuid
    {
        [Theory]
        [InlineData("034B1998-E8C7-4DC0-B0EF-E4D606166756", "034B1998-E8C7-4DC0-B0EF-E4D606166756")]
        [InlineData("034B1998-E8C7-4DC0-B0EF-E4D606166756", "034b1998-e8c7-4dc0-b0ef-e4d606166756")]
        [InlineData("bob", "00000000-0000-0000-0000-000000000000")]
        public void ToGuid_without_default_can_convert_successfully(string text, string expected)
        {
            var result = text.ToGuid();

            result.ToString().ShouldBe(expected, StringCompareShould.IgnoreCase);
        }

        [Theory]
        [InlineData("034B1998-E8C7-4DC0-B0EF-E4D606166756", "00000000-0000-0000-0000-000000000000", "034B1998-E8C7-4DC0-B0EF-E4D606166756")]
        [InlineData("034B1998-E8C7-4DC0-B0EF-E4D606166756", "00000000-0000-0000-0000-000000000000", "034b1998-e8c7-4dc0-b0ef-e4d606166756")]
        [InlineData("034B1998-E8C7-4DC0-B0EF-E4D606166756", "E223D5C2-61C5-4FD9-AABE-F8761B4EDCA6", "034b1998-e8c7-4dc0-b0ef-e4d606166756")]
        [InlineData("bob", "E223D5C2-61C5-4FD9-AABE-F8761B4EDCA6", "E223D5C2-61C5-4FD9-AABE-F8761B4EDCA6")]
        public void ToGuid_with_default_can_convert_successfully(string text, string defaultValue, string expected)
        {
            var result = text.ToGuid(Guid.Parse(defaultValue));

            result.ToString().ShouldBe(expected, StringCompareShould.IgnoreCase);
        }
    }

    public class To
    {
        [Fact]
        public void To_without_default_for_null_class_instance_can_convert_object_successfully()
        {
            var instance = (TestClass2)null;
            var result = instance.To<TestClass1>();
            result.ShouldBeNull();
        }

        [Fact]
        public void To_without_default_for_related_class_can_convert_object_successfully()
        {
            var instance = new TestClass2();
            var result = instance.To<TestClass1>();
            result.ShouldNotBeNull();
            result.ShouldBe(instance);
        }

        [Fact]
        public void To_without_default_for_unrelated_class_can_convert_object_successfully()
        {
            var instance = new TestClass3();
            var result = instance.To<TestClass1>();
            result.ShouldBeNull();
        }

        [Fact]
        public void To_with_default_for_null_class_instance_can_convert_object_successfully()
        {
            var instance = (TestClass2)null;
            var defaultValue = new TestClass1();
            var result = instance.To<TestClass1>(defaultValue);
            result.ShouldNotBeNull();
            result.ShouldBe(defaultValue);
        }

        [Fact]
        public void To_with_default_for_related_class_can_convert_object_successfully()
        {
            var defaultValue = new TestClass1();
            var instance = new TestClass2();
            var result = instance.To<TestClass1>(defaultValue);
            result.ShouldNotBeNull();
            result.ShouldBe(instance);
        }

        [Fact]
        public void To_with_default_for_unrelated_class_can_convert_object_successfully()
        {
            var defaultValue = new TestClass1();
            var instance = new TestClass3();
            var result = instance.To<TestClass1>(defaultValue);
            result.ShouldNotBeNull();
            result.ShouldBe(defaultValue);
        }
    }
}
