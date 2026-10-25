using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using DNX.Extensions.Reflection;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Reflection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
internal sealed class ReflectionMarkerAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}

[ReflectionMarker("interface")]
internal interface IReflectionMarked
{
}

[ReflectionMarker("base")]
internal class ReflectionMarkedBase
{
    public int Number { get; set; }
}

internal sealed class ReflectionMarkedChild : ReflectionMarkedBase, IReflectionMarked
{
    public string Name { get; set; }
    private string Secret { get; set; } = "secret";
    public static string StaticValue => "static";
    public int Field = 5;
}

public class ReflectionAdditionalTests
{
    [Fact]
    public void Attribute_extensions_read_type_instance_and_member_attributes()
    {
        var typeAttributes = typeof(ReflectionMarkedChild).GetTypeAttributes<ReflectionMarkerAttribute>(true);
        var instance = new ReflectionMarkedChild();
        var instanceAttributes = instance.GetTypeAttributesFromInstance<ReflectionMarkerAttribute>(true);
        var member = typeof(ReflectionMarkedChild).GetProperty(nameof(ReflectionMarkedChild.Name));

        typeAttributes.ShouldContain(attribute => attribute.Value == "interface");
        instanceAttributes.ShouldContain(attribute => attribute.Value == "interface");
        member.GetMemberAttributes<ReflectionMarkerAttribute>(true).ShouldBeEmpty();
        typeof(ReflectionMarkedChild).HasTypeAttributes<ReflectionMarkerAttribute>(true).ShouldBeTrue();
        instance.InstanceHasTypeAttributes<ReflectionMarkerAttribute>(true).ShouldBeTrue();
    }

    [Fact]
    public void Attribute_extensions_reject_null_inputs()
    {
        Should.Throw<ArgumentNullException>(
            () => ((Type)null).GetTypeAttributes<ReflectionMarkerAttribute>(false));
        Should.Throw<ArgumentNullException>(
            () => ((object)null).GetTypeAttributesFromInstance<ReflectionMarkerAttribute>(false));
        Should.Throw<ArgumentNullException>(
            () => ((MemberInfo)null).GetMemberAttributes<ReflectionMarkerAttribute>(false));
    }

    [Fact]
    public void Expression_extensions_identify_and_name_expression_shapes()
    {
        var instance = new ReflectionMarkedChild();
        Expression<Func<int>> member = () => instance.Number;
        Expression<Func<int>> constant = () => 1;
        Expression<Func<object>> unary = () => instance.Number;
        Expression<Func<Func<int>>> lambda = () => () => 1;

        ExpressionExtensions.IsMemberExpression(member).ShouldBeTrue();
        ExpressionExtensions.IsMemberExpression(constant).ShouldBeFalse();
        ExpressionExtensions.IsUnaryExpression(unary).ShouldBeTrue();
        ExpressionExtensions.IsLambdaExpression(lambda).ShouldBeTrue();
        ExpressionExtensions.GetLambdaName(lambda).ShouldBeNull();
        ExpressionExtensions.GetMemberName(member).ShouldBe(nameof(ReflectionMarkedBase.Number));
        ExpressionExtensions.GetMemberName(constant).ShouldBeNull();
        ExpressionExtensions.GetUnaryName(unary).ShouldBe(nameof(ReflectionMarkedBase.Number));
        ExpressionExtensions.GetUnaryName(() => (object)1).ShouldBeNull();
        ExpressionExtensions.GetExpressionName(member).ShouldBe(nameof(ReflectionMarkedBase.Number));
        ExpressionExtensions.GetExpressionName(unary).ShouldBe(nameof(ReflectionMarkedBase.Number));
        ExpressionExtensions.GetExpressionName(lambda).ShouldBeNull();
        ExpressionExtensions.GetExpressionName(constant).ShouldBeNull();
    }

    [Fact]
    public void Type_extensions_cover_nullable_assignability_and_default_creation()
    {
        typeof(string).IsA<object>().ShouldBeTrue();
        typeof(ReflectionMarkedChild).IsA<ReflectionMarkedBase>().ShouldBeTrue();
        typeof(int).GetDefaultValue().ShouldBe(0);
        typeof(string).GetDefaultValue().ShouldBeNull();
        DNX.Extensions.Reflection.TypeExtensions.CreateDefaultInstance<ReflectionMarkedChild>().ShouldNotBeNull();
        typeof(ReflectionMarkedChild).CreateDefaultInstance().ShouldBeOfType<ReflectionMarkedChild>();
    }

    [Fact]
    public void Reflection_extensions_read_members_nested_values_and_dictionaries()
    {
        var instance = new ReflectionMarkedChild { Number = 12, Name = "name" };

        instance.GetPropertyValueByName(nameof(ReflectionMarkedChild.Name), BindingFlags.Instance | BindingFlags.Public)
            .ShouldBe("name");
        instance.GetPrivatePropertyValue("Secret").ShouldBe("secret");
        instance.GetPropertyInfoByName("number").Name.ShouldBe(nameof(ReflectionMarkedBase.Number));
        instance.GetPropertyInfoByName("Missing").ShouldBeNull();
        instance.GetPropertyInfoByName("Child.Missing").ShouldBeNull();
        instance.IsPropertyNameValid("Name").ShouldBeTrue();
        instance.IsPropertyNameValid("Missing").ShouldBeFalse();
        instance.GetNestedPropertyValueByName("Name").ShouldBe("name");
        instance.GetNestedPropertyValueByName("Missing").ShouldBeNull();
        ReflectionExtensions.GetPropertiesDictionary(instance, BindingFlags.Instance | BindingFlags.Public)
            .ShouldContainKey(nameof(ReflectionMarkedChild.Name));
        instance.AsDictionary().ShouldContainKey(nameof(ReflectionMarkedChild.Name));
        ((ReflectionMarkedChild)null).AsDictionaryTyped().ShouldBeNull();

        var expando = new ExpandoObject() as IDictionary<string, object>;
        expando["Name"] = "expanded";
        expando.AsDictionary()["Name"].ShouldBe("expanded");

        var dictionary = new Dictionary<string, object> { ["Name"] = "restored", ["Number"] = 99 };
        var restored = dictionary.AsInstance<ReflectionMarkedChild>();
        restored.Name.ShouldBe("restored");
        restored.Number.ShouldBe(99);
    }

    [Fact]
    public void Reflection_extensions_handle_null_dictionary_operations_and_collections()
    {
        ReflectionExtensions.GetPropertiesDictionary(null).ShouldBeNull();
        ReflectionExtensions.AsDictionaryTyped<ReflectionMarkedChild>(null).ShouldBeNull();
        new ReflectionMarkedChild().AsDictionaryTyped().ShouldContainKey(nameof(ReflectionMarkedChild.Name));
        new ReflectionMarkedChild().PopulateFrom(null).ShouldBeOfType<ReflectionMarkedChild>();
        new ReflectionMarkedChild().PopulateFrom(new Dictionary<string, object> { ["Missing"] = 1 })
            .ShouldBeOfType<ReflectionMarkedChild>();
        ((IDictionary<string, object>)null).AsInstance<ReflectionMarkedChild>().ShouldBeNull();

        var types = new[] { typeof(ReflectionMarkedBase), typeof(ReflectionMarkedChild) };
        ReflectionExtensions.GetPropertiesForTypes(types).Count.ShouldBeGreaterThan(0);
        ReflectionExtensions.GetFieldsForType(typeof(ReflectionMarkedChild)).ShouldContain(field => field.Name == nameof(ReflectionMarkedChild.Field));
    }

    [Fact]
    public void Instance_resolution_returns_empty_for_null_and_filters_matches()
    {
        IList<ReflectionMarkedChild> nullInstances = null;
        nullInstances.ResolveInstancesOfTypeWithAttributeAndCondition<ReflectionMarkedChild, ReflectionMarkerAttribute>(
            attribute => attribute.Value == "interface").ShouldBeEmpty();
    }
}
