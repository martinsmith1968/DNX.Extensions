using DNX.Extensions.Objects;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Objects;

internal class MyHashCodeClass
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class ObjectExtensionsTests
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

    [Theory]
    [InlineData(1, "Bob", 2, "Dave", false)]
    [InlineData(1, "Bob", 1, "Bob", false)]
    [InlineData(1, "Bob", null, null, true)]
    public void GetUniqueInstanceId_can_generate_Ids_based_on_reference_instance(int id1, string name1, int? id2, string name2, bool expectedResult)
    {
        var instance1 = new MyHashCodeClass() { Id = id1, Name = name1 };
        var instance2 = id2.HasValue
            ? new MyHashCodeClass() { Id = id2.Value, Name = name2 }
            : instance1;

        // Act
        var uniqueId1 = instance1.GetUniqueInstanceId();
        var uniqueId2 = instance2.GetUniqueInstanceId();

        // Assert
        (uniqueId1 == uniqueId2).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(1, "Bob", 2, "Dave", false)]
    [InlineData(1, "Bob", 2, "Bob", false)]
    [InlineData(1, "Bob", 1, "Dave", false)]
    [InlineData(1, "Bob", 1, "Bob", true)]
    [InlineData(2, "Dave", 2, "Dave", true)]
    [InlineData(1, "Bob", null, null, true)]
    public void GetUniqueInstanceId_can_generate_Ids_with_name_override(int id1, string name1, int? id2, string name2, bool expectedResult)
    {
        var instance1 = new MyHashCodeClass() { Id = id1, Name = name1 };
        var instance2 = id2.HasValue
            ? new MyHashCodeClass() { Id = id2.Value, Name = name2 }
            : instance1;

        // Act
        var uniqueId1 = instance1.GetUniqueInstanceId($"{instance1.Id}.{instance1.Name}");
        var uniqueId2 = instance2.GetUniqueInstanceId($"{instance2.Id}.{instance2.Name}");

        // Assert
        (uniqueId1 == uniqueId2).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("a", "b", "c", "a")]
    [InlineData("a", null, null, "a")]
    [InlineData(null, "b", "c", "b")]
    [InlineData(null, null, "c", "c")]
    [InlineData(null, null, null, null)]
    [InlineData("a", "", "", "a")]
    [InlineData("", "b", "c", "")]
    [InlineData("", "", "c", "")]
    [InlineData("", "", "", "")]
    public void Test_CoalesceNull(string a, string b, string c, string expectedResult)
    {
        // Act
        var result = ObjectExtensions.CoalesceNull(a, b, c);

        // Assert
       result.ShouldBe(expectedResult);
    }
}
