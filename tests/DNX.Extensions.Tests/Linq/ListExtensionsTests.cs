using DNX.Extensions.Exceptions;
using DNX.Extensions.Linq;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Linq;

public class ListExtensionsTests
{
    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 0, true)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 5, true)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 11, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 10, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -1, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -4, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -15, false)]
    [InlineData("", 5, false)]
    [InlineData(null, 5, false)]
    public void Test_IsValidIndex(string commaDelimitedArray, int index, bool expectedResult)
    {
        var array = string.IsNullOrEmpty(commaDelimitedArray)
            ? null
            : commaDelimitedArray.Split(",".ToCharArray());

        // Act
        var result = array.IsIndexValid(index);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 0, 0)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 5, 5)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 11, 11)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 10, 10)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -1, 9)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -4, 6)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -15, 5)]
    [InlineData("", 5, 5)]
    [InlineData(null, 5, 5)]
    public void Test_GetAbsoluteIndex(string commaDelimitedArray, int index, int expectedResult)
    {
        var array = string.IsNullOrEmpty(commaDelimitedArray)
            ? null
            : commaDelimitedArray.Split(",".ToCharArray());

        // Act
        var result = array.GetAbsoluteIndex(index);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 0, "a")]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 5, "f")]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 9, "j")]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 10, null)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -1, "j")]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -15, "f")]
    [InlineData("", 5, null)]
    [InlineData(null, 5, null)]
    public void Test_GetAt(string commaDelimitedArray, int index, string expectedResult)
    {
        var array = string.IsNullOrEmpty(commaDelimitedArray)
            ? null
            : commaDelimitedArray.Split(",".ToCharArray());

        // Act
        var result = array.GetAt(index);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 1, 5, "a,c,d,e,f,b,g,h,i,j")]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 4, 0, "e,a,b,c,d,f,g,h,i,j")]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 3, -1, "a,b,c,e,f,g,h,i,j,d")]
    public void Test_Move(string commaDelimitedArray, int oldIndex, int newIndex, string expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        // Act
        enumerable.Move(oldIndex, newIndex);

        // Assert
        string.Join(",", enumerable).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 1, 5, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 4, 0, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 3, -1, false)]
    public void Test_Move_ReadOnly(string commaDelimitedArray, int oldIndex, int newIndex, bool expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        try
        {
            // Act
            enumerable.Move(oldIndex, newIndex);

            // Assert
            expectedResult.ShouldBeTrue();
        }
        catch (ReadOnlyListException<string>)
        {
            // Assert
            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 21, 5, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 14, 0, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 1023, -1, false)]
    public void Test_Move_BadOldIndex(string commaDelimitedArray, int oldIndex, int newIndex, bool expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        try
        {
            // Act
            enumerable.Move(oldIndex, newIndex);

            // Assert
            expectedResult.ShouldBeTrue();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            if (ex.ParamName != "oldIndex")
                throw new Exception("Incorrect parameter name - " + ex.ParamName);

            // Assert
            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 5, 21, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 0, 14, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -1, 1023, false)]
    public void  Test_Move_BadNewIndex(string commaDelimitedArray, int oldIndex, int newIndex, bool expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        try
        {
            // Act
            enumerable.Move(oldIndex, newIndex);

            expectedResult.ShouldBeTrue();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            if (ex.ParamName != "newIndex")
                throw new Exception("Incorrect parameter name - " + ex.ParamName);

            // Assert
            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 1, 5, "a,f,c,d,e,b,g,h,i,j")]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 4, 0, "e,b,c,d,a,f,g,h,i,j")]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 3, -1, "a,b,c,j,e,f,g,h,i,d")]
    public void  Test_Swap(string commaDelimitedArray, int oldIndex, int newIndex, string expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        // Act
        enumerable.Swap(oldIndex, newIndex);

        // Assert
        string.Join(",", enumerable).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 1, 5, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 4, 0, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 3, -1, false)]
    public void Test_Swap_ReadOnly(string commaDelimitedArray, int oldIndex, int newIndex, bool expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        try
        {
            // Act
            enumerable.Swap(oldIndex, newIndex);

            // Assert
            expectedResult.ShouldBeTrue();
        }
        catch (ReadOnlyListException<string>)
        {
            // Assert
            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 21, 5, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 14, 0, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 1023, -1, false)]
    public void Test_Swap_BadOldIndex(string commaDelimitedArray, int oldIndex, int newIndex, bool expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        try
        {
            // Act
            enumerable.Swap(oldIndex, newIndex);

            // Assert
            expectedResult.ShouldBeTrue();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            if (ex.ParamName != "oldIndex")
                throw new Exception("Incorrect parameter name - " + ex.ParamName);

            // Assert
            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 5, 21, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", 0, 14, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", -1, 1023, false)]
    public void Test_Swap_BadNewIndex(string commaDelimitedArray, int oldIndex, int newIndex, bool expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        try
        {
            // Act
            enumerable.Swap(oldIndex, newIndex);

            // Assert
            expectedResult.ShouldBeTrue();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            if (ex.ParamName != "newIndex")
                throw new Exception("Incorrect parameter name - " + ex.ParamName);

            // Assert
            expectedResult.ShouldBeFalse();
        }
    }

    [Fact]
    public void CreateList_can_create_appropriate_lists()
    {
        // Arrange


        // Act
        var list1 = ListExtensions.CreateList(1);
        var list2 = ListExtensions.CreateList(1, 2);
        var list3 = ListExtensions.CreateList(1, 2, 3);
        var list4 = ListExtensions.CreateList(1, 2, 3, 4);
        var list5 = ListExtensions.CreateList(1, 2, 3, 4, 5);

        // Assert
        list1.ShouldNotBeNull();
        list1.Count.ShouldBe(1);
        list1[0].ShouldBe(1);

        list2.ShouldNotBeNull();
        list2.Count.ShouldBe(2);
        list2[0].ShouldBe(1);
        list2[1].ShouldBe(2);

        list3.ShouldNotBeNull();
        list3.Count.ShouldBe(3);
        list3[0].ShouldBe(1);
        list3[1].ShouldBe(2);
        list3[2].ShouldBe(3);

        list4.ShouldNotBeNull();
        list4.Count.ShouldBe(4);
        list4[0].ShouldBe(1);
        list4[1].ShouldBe(2);
        list4[2].ShouldBe(3);
        list4[3].ShouldBe(4);

        list5.ShouldNotBeNull();
        list5.Count.ShouldBe(5);
        list5[0].ShouldBe(1);
        list5[1].ShouldBe(2);
        list5[2].ShouldBe(3);
        list5[3].ShouldBe(4);
        list5[4].ShouldBe(5);
    }
}
