using System.Reflection;
using DNX.Extensions.Objects;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Objects;

internal class MyHashCodeClass
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class ToDictionaryBase
{
    public string InheritedProperty { get; set; } = "base-value";
}

public class ToDictionarySimple : ToDictionaryBase
{
    public int Id { get; set; } = 1;

    public string Name { get; set; } = "bob";

    public string NullProperty { get; set; } = null;

    private string PrivateProperty { get; set; } = "private-value";

    protected string ProtectedProperty { get; set; } = "protected-value";

    public static string StaticProperty { get; set; } = "static-value";

    public string ReadPrivateProperty() => PrivateProperty;

    public string ReadProtectedProperty() => ProtectedProperty;
}

public class ToDictionaryEmpty
{
}

public class ToDictionaryWithIndexer
{
    public string Name { get; set; } = "bob";

    public string this[int index] => $"item{index}";
}

public class ToDictionaryWithWriteOnlyProperty
{
    public string Name { get; set; } = "bob";

    private string _secret;

    public string Secret
    {
        set => _secret = value;
    }

    public string ReadSecret() => _secret;
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

    public class ToDictionary
    {
        [Fact]
        public void ToDictionary_for_null_object_returns_null()
        {
            object instance = null;

            var result = instance.ToDictionary();

            result.ShouldBeNull();
        }

        [Fact]
        public void ToDictionary_with_binding_flags_for_null_object_returns_null()
        {
            object instance = null;

            var result = instance.ToDictionary(BindingFlags.Instance | BindingFlags.Public);

            result.ShouldBeNull();
        }

        [Fact]
        public void ToDictionary_can_convert_public_properties_successfully()
        {
            var instance = new ToDictionarySimple();

            var result = instance.ToDictionary();

            result.ShouldNotBeNull();
            result["Id"].ShouldBe(1);
            result["Name"].ShouldBe("bob");
            result["NullProperty"].ShouldBeNull();
        }

        [Fact]
        public void ToDictionary_includes_inherited_public_properties()
        {
            var instance = new ToDictionarySimple();

            var result = instance.ToDictionary();

            result.ShouldContainKey("InheritedProperty");
            result["InheritedProperty"].ShouldBe("base-value");
        }

        [Fact]
        public void ToDictionary_excludes_non_public_and_static_properties_by_default()
        {
            var instance = new ToDictionarySimple();

            var result = instance.ToDictionary();

            result.ShouldNotContainKey("PrivateProperty");
            result.ShouldNotContainKey("ProtectedProperty");
            result.ShouldNotContainKey("StaticProperty");
        }

        [Fact]
        public void ToDictionary_for_type_with_no_public_properties_returns_empty_dictionary()
        {
            var instance = new ToDictionaryEmpty();

            var result = instance.ToDictionary();

            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public void ToDictionary_skips_indexer_properties_instead_of_throwing()
        {
            var instance = new ToDictionaryWithIndexer();

            var result = instance.ToDictionary();

            result.ShouldNotBeNull();
            result.ShouldNotContainKey("Item");
            result["Name"].ShouldBe("bob");
        }

        [Fact]
        public void ToDictionary_skips_write_only_properties_instead_of_throwing()
        {
            var instance = new ToDictionaryWithWriteOnlyProperty();

            var result = instance.ToDictionary();

            result.ShouldNotBeNull();
            result.ShouldNotContainKey("Secret");
            result["Name"].ShouldBe("bob");
        }

        [Fact]
        public void ToDictionary_with_binding_flags_can_include_non_public_properties()
        {
            var instance = new ToDictionarySimple();

            var result = instance.ToDictionary(BindingFlags.Instance | BindingFlags.NonPublic);

            result.ShouldNotBeNull();
            result["PrivateProperty"].ShouldBe(instance.ReadPrivateProperty());
            result["ProtectedProperty"].ShouldBe(instance.ReadProtectedProperty());
            result.ShouldNotContainKey("Id");
            result.ShouldNotContainKey("Name");
        }

        [Fact]
        public void ToDictionary_with_binding_flags_can_include_static_properties()
        {
            var instance = new ToDictionarySimple();

            var result = instance.ToDictionary(BindingFlags.Static | BindingFlags.Public);

            result.ShouldNotBeNull();
            result["StaticProperty"].ShouldBe(ToDictionarySimple.StaticProperty);
            result.ShouldNotContainKey("Id");
        }

        [Fact]
        public void ToDictionary_with_binding_flags_still_skips_indexer_and_write_only_properties()
        {
            var indexed = new ToDictionaryWithIndexer();
            var writeOnly = new ToDictionaryWithWriteOnlyProperty();

            var indexedResult = indexed.ToDictionary(BindingFlags.Instance | BindingFlags.Public);
            var writeOnlyResult = writeOnly.ToDictionary(BindingFlags.Instance | BindingFlags.Public);

            indexedResult.ShouldNotContainKey("Item");
            writeOnlyResult.ShouldNotContainKey("Secret");
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

    [Fact]
    public void ChangeType_can_convert_string_to_integer_and_return_default_on_failure()
    {
        object value = "42";
        var converted = value.ChangeType<int>();
        converted.ShouldBe(42);

        value = "42";
        var typed = value.ChangeType(typeof(int));
        typed.ShouldBe(42);

        var fallback = "not-an-int".ChangeType(99);
        fallback.ShouldBe(99);

        Should.Throw<FormatException>(() => "not-an-int".ChangeType(typeof(int)));
    }

    [Fact]
    public void ChangeType_invokes_the_exceptionHandler_only_when_conversion_fails()
    {
        Exception caught = null;

        var success = "42".ChangeType(0, ex => caught = ex);
        success.ShouldBe(42);
        caught.ShouldBeNull();

        var failure = "not-an-int".ChangeType(99, ex => caught = ex);
        failure.ShouldBe(99);
        caught.ShouldNotBeNull();
    }

    [Fact]
    public void To_can_return_default_when_conversion_fails()
    {
        var good = ((object)42).To<int>();
        var bad = "abc".To<int>(99);

        good.ShouldBe(42);
        bad.ShouldBe(99);
    }

    [Fact]
    public void To_invokes_the_exceptionHandler_only_when_conversion_fails()
    {
        Exception caught = null;

        var success = ((object)42).To(0, ex => caught = ex);
        success.ShouldBe(42);
        caught.ShouldBeNull();

        var failure = "abc".To(99, ex => caught = ex);
        failure.ShouldBe(99);
        caught.ShouldNotBeNull();
    }
}
