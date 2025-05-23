using DNX.Extensions.Maths;
using DNX.Extensions.Maths.BuiltInTypes;
using DNX.Extensions.Tests.Maths.BuiltInTypes.TestsDataSource;
using Shouldly;
using Xunit;

//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

namespace DNX.Extensions.Tests.Maths.BuiltInTypes;

public class MathsUInt32ExtensionsTests
{
    [Theory]
    [MemberData(nameof(MathsUInt32ExtensionsTestsDataSource.IsBetween_Default), MemberType = typeof(MathsUInt32ExtensionsTestsDataSource))]
    public void IsBetween_Default(uint value, uint min, uint max, bool expectedResult)
    {
        value.IsBetween(min, max).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsUInt32ExtensionsTestsDataSource.IsBetween_BoundsType), MemberType = typeof(MathsUInt32ExtensionsTestsDataSource))]
    public void IsBetween_BoundsType(uint value, uint min, uint max, IsBetweenBoundsType boundsType, bool expectedResult)
    {
        value.IsBetween(min, max, boundsType).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsUInt32ExtensionsTestsDataSource.IsBetweenEither_Default), MemberType = typeof(MathsUInt32ExtensionsTestsDataSource))]
    public void IsBetweenEither_Default(uint value, uint min, uint max, bool expectedResult)
    {
        value.IsBetweenEither(min, max).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsUInt32ExtensionsTestsDataSource.IsBetweenEither_BoundsType), MemberType = typeof(MathsUInt32ExtensionsTestsDataSource))]
    public void IsBetweenEither_BoundsType(uint value, uint min, uint max, IsBetweenBoundsType boundsType, bool expectedResult)
    {
        value.IsBetweenEither(min, max, boundsType).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsUInt32ExtensionsTestsDataSource.IsBetween), MemberType = typeof(MathsUInt32ExtensionsTestsDataSource))]
    public void IsBetween(uint value, uint min, uint max, bool allowEitherOrder, IsBetweenBoundsType boundsType, bool expectedResult)
    {
        value.IsBetween(min, max, allowEitherOrder, boundsType).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsUInt32ExtensionsTestsDataSource.GetLowerBound_Default), MemberType = typeof(MathsUInt32ExtensionsTestsDataSource))]
    public void GetLowerBound_Default(uint min, uint max, uint expectedResult)
    {
        MathsUInt32Extensions.GetLowerBound(min, max).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsUInt32ExtensionsTestsDataSource.GetLowerBound_AllowEitherOrder), MemberType = typeof(MathsUInt32ExtensionsTestsDataSource))]
    public void GetLowerBound_AllowEitherOrder(uint min, uint max, bool allowEitherOrder, uint expectedResult)
    {
        MathsUInt32Extensions.GetLowerBound(min, max, allowEitherOrder).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsUInt32ExtensionsTestsDataSource.GetUpperBound_Default), MemberType = typeof(MathsUInt32ExtensionsTestsDataSource))]
    public void GetUpperBound_Default(uint min, uint max, uint expectedResult)
    {
        MathsUInt32Extensions.GetUpperBound(min, max).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsUInt32ExtensionsTestsDataSource.GetUpperBound_AllowEitherOrder), MemberType = typeof(MathsUInt32ExtensionsTestsDataSource))]
    public void GetUpperBound_AllowEitherOrder(uint min, uint max, bool allowEitherOrder, uint expectedResult)
    {
        MathsUInt32Extensions.GetUpperBound(min, max, allowEitherOrder).ShouldBe(expectedResult);
    }
}

