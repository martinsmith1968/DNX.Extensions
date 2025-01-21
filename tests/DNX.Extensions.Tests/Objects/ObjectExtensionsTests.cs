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
        var uniqueId1 = instance1.GetUniqueInstanceId(string.Format("{0}.{1}", instance1.Id, instance1.Name));
        var uniqueId2 = instance2.GetUniqueInstanceId(string.Format("{0}.{1}", instance2.Id, instance2.Name));

        // Assert
        (uniqueId1 == uniqueId2).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("a", "b", "c", "a")]
    [InlineData("a", null, null, "a")]
    [InlineData(null, "b", "c", "b")]
    [InlineData(null, null, "c", "c")]
    [InlineData(null, null, null, null)]
    [InlineData("a", "b", "c", "a")]
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
