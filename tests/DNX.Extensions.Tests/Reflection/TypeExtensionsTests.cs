namespace DNX.Extensions.Tests.Reflection;

internal class MyTestClass2
{
    public int Number { get; set; }

    public MyTestClass2()
    {
        Number = 123;
    }
}

internal interface I1 { }
internal interface I1A : I1 { }
internal interface I2 { }

internal class C1 : I1 { }
internal class C1A : I1A { }
internal class C2 : I2 { }
internal class C1AI2 : I1A, I2 { }

[TestFixture]
public class TypeExtensionsTests
{
    [InlineData(typeof(object), true)]
    [InlineData(typeof(TypeExtensionsTests), true)]
    [InlineData(typeof(MyTestClass1), true)]
    [InlineData(typeof(int), false)]
    [InlineData(typeof(int?), true)]
    [InlineData(typeof(DateTime), false)]
    [InlineData(typeof(DateTime?), true)]
    public bool IsNullableTest(Type type)
    {
        return type.IsNullable();
    }

    public void IsNullableTest_Throws()
    {
        try
        {
            Type type = null;

            var result = type.IsNullable();

            Assert.Fail("Unexpected result");
        }
        catch (ArgumentNullException)
        {
        }
    }

    [InlineData(typeof(I1), true)]
    [InlineData(typeof(I1A), true)]
    [InlineData(typeof(I2), false)]
    [InlineData(typeof(C1), true)]
    [InlineData(typeof(C1A), true)]
    [InlineData(typeof(C2), false)]
    [InlineData(typeof(C1AI2), true)]
    public bool IsA_I1_Test(Type type)
    {
        return type.IsA<I1>();
    }

    [InlineData(typeof(I1), typeof(I1), true)]
    [InlineData(typeof(I1A), typeof(I1), true)]
    [InlineData(typeof(I2), typeof(I1), false)]
    [InlineData(typeof(C1), typeof(I1), true)]
    [InlineData(typeof(C1A), typeof(I1), true)]
    [InlineData(typeof(C2), typeof(I1), false)]
    [InlineData(typeof(C1AI2), typeof(I1), true)]
    [InlineData(typeof(C1AI2), typeof(I2), true)]
    [InlineData(typeof(C1AI2), typeof(I1A), true)]
    public bool IsA_Test(Type type, Type baseType)
    {
        return type.IsA(baseType);
    }

    [InlineData(typeof(object), null)]
    [InlineData(typeof(TypeExtensionsTests), null)]
    [InlineData(typeof(MyTestClass2), null)]
    [InlineData(typeof(int), 0)]
    [InlineData(typeof(int?), null)]
    [InlineData(typeof(DateTime?), null)]
    public object GetDefaultValueTest(Type type)
    {
        return type.GetDefaultValue();
    }

    [Fact]
    public void CreateDefaultInstanceTest_Generic()
    {
        // Arrange

        // Act
        var instance1 = TypeExtensions.CreateDefaultInstance<TypeExtensionsTests>();
        var instance2 = TypeExtensions.CreateDefaultInstance<MyTestClass2>();
        var instance3 = TypeExtensions.CreateDefaultInstance<int>();

        // Assert
        Assert.IsNotNull(instance1);

        Assert.IsNotNull(instance2);
        Assert.AreEqual(123, instance2.Number);

        Assert.AreEqual(0, instance3);
    }

    [Fact]
    public void CreateDefaultInstanceTest1()
    {
        // Arrange

        // Act
        var instance1 = typeof(TypeExtensionsTests).CreateDefaultInstance() as TypeExtensionsTests;
        var instance2 = typeof(MyTestClass2).CreateDefaultInstance() as MyTestClass2;

        // Assert
        Assert.IsNotNull(instance1);

        Assert.IsNotNull(instance2);
        Assert.AreEqual(123, instance2.Number);
    }
}
