using DNX.Extensions.Reflection;
using DNX.Extensions.Tests.Linq;
using Shouldly;
using Xunit;

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

public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(object), true)]
    [InlineData(typeof(TypeExtensionsTests), true)]
    [InlineData(typeof(MyTestClass1), true)]
    [InlineData(typeof(int), false)]
    [InlineData(typeof(int?), true)]
    [InlineData(typeof(DateTime), false)]
    [InlineData(typeof(DateTime?), true)]
    public void IsNullableTest(Type type, bool expectedResult)
    {
        type.IsNullable().ShouldBe(expectedResult);
    }

    [Fact]
    public void IsNullableTest_Throws()
    {
        Type type = null;

        try
        {
            var result = type.IsNullable();

            Assert.Fail("Unexpected result");
        }
        catch (ArgumentNullException ex)
        {
            ex.ShouldNotBeNull();
            ex.ParamName.ShouldBe(nameof(type));
        }
    }

    [Theory]
    [InlineData(typeof(I1), true)]
    [InlineData(typeof(I1A), true)]
    [InlineData(typeof(I2), false)]
    [InlineData(typeof(C1), true)]
    [InlineData(typeof(C1A), true)]
    [InlineData(typeof(C2), false)]
    [InlineData(typeof(C1AI2), true)]
    public void IsA_I1_Test(Type type, bool expectedResult)
    {
        type.IsA<I1>().ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(typeof(I1), typeof(I1), true)]
    [InlineData(typeof(I1A), typeof(I1), true)]
    [InlineData(typeof(I2), typeof(I1), false)]
    [InlineData(typeof(C1), typeof(I1), true)]
    [InlineData(typeof(C1A), typeof(I1), true)]
    [InlineData(typeof(C2), typeof(I1), false)]
    [InlineData(typeof(C1AI2), typeof(I1), true)]
    [InlineData(typeof(C1AI2), typeof(I2), true)]
    [InlineData(typeof(C1AI2), typeof(I1A), true)]
    public void IsA_Test(Type type, Type baseType, bool expectedResult)
    {
        type.IsA(baseType).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(typeof(object), null)]
    [InlineData(typeof(TypeExtensionsTests), null)]
    [InlineData(typeof(MyTestClass2), null)]
    [InlineData(typeof(int), 0)]
    [InlineData(typeof(int?), null)]
    [InlineData(typeof(DateTime?), null)]
    public void GetDefaultValueTest(Type type, object expectedResult)
    {
        type.GetDefaultValue().ShouldBe(expectedResult);
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
        instance1.ShouldNotBeNull();

        instance2.ShouldNotBeNull();
        instance2.Number.ShouldBe(123);

        instance3.ShouldBe(0);
    }

    [Fact]
    public void CreateDefaultInstanceTest1()
    {
        // Arrange

        // Act
        var instance1 = typeof(TypeExtensionsTests).CreateDefaultInstance() as TypeExtensionsTests;
        var instance2 = typeof(MyTestClass2).CreateDefaultInstance() as MyTestClass2;

        // Assert
        instance1.ShouldNotBeNull();

        instance2.ShouldNotBeNull();
        instance2.Number.ShouldBe(123);
    }
}
