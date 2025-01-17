namespace DNX.Extensions.Tests.Objects;

internal class MyHashCodeClass
{
    public int Id { get; set; }

    public string Name { get; set; }
}

[TestFixture]
public class ObjectExtensionsTests
{
    [InlineData(1, "Bob", 2, "Dave", false)]
    [InlineData(1, "Bob", 1, "Bob", false)]
    [InlineData(1, "Bob", null, null, true)]
    public bool GetUniqueInstanceId_can_generate_Ids_based_on_reference_instance(int id1, string name1, int? id2, string name2)
    {
        var instance1 = new MyHashCodeClass() { Id = id1, Name = name1 };
        var instance2 = id2.HasValue
            ? new MyHashCodeClass() { Id = id2.Value, Name = name2 }
            : instance1;

        var uniqueId1 = instance1.GetUniqueInstanceId();
        var uniqueId2 = instance2.GetUniqueInstanceId();

        return uniqueId1 == uniqueId2;
    }

    [InlineData(1, "Bob", 2, "Dave", false)]
    [InlineData(1, "Bob", 2, "Bob", false)]
    [InlineData(1, "Bob", 1, "Dave", false)]
    [InlineData(1, "Bob", 1, "Bob", true)]
    [InlineData(2, "Dave", 2, "Dave", true)]
    [InlineData(1, "Bob", null, null, true)]
    public bool GetUniqueInstanceId_can_generate_Ids_with_name_override(int id1, string name1, int? id2, string name2)
    {
        var instance1 = new MyHashCodeClass() { Id = id1, Name = name1 };
        var instance2 = id2.HasValue
            ? new MyHashCodeClass() { Id = id2.Value, Name = name2 }
            : instance1;

        var uniqueId1 = instance1.GetUniqueInstanceId(string.Format("{0}.{1}", instance1.Id, instance1.Name));
        var uniqueId2 = instance2.GetUniqueInstanceId(string.Format("{0}.{1}", instance2.Id, instance2.Name));

        return uniqueId1 == uniqueId2;
    }

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

    public object Test_CoalesceNull(string a, string b, string c)
    {
        var result = ObjectExtensions.CoalesceNull(a, b, c);

        return result;
    }
}
