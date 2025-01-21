using DNX.Extensions.Conversion;
using DNX.Extensions.Objects;
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
