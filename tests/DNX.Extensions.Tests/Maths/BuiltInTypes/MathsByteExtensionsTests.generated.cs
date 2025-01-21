using DNX.Extensions.Maths;
using DNX.Extensions.Maths.BuiltInTypes;
using DNX.Extensions.Tests.Maths.BuiltInTypes.TestsDataSource;
using Shouldly;
using Xunit;

//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

namespace DNX.Extensions.Tests.Maths.BuiltInTypes;

public class MathsByteExtensionsTests
{
    [Theory]
    [MemberData(nameof(MathsByteExtensionsTestsDataSource.IsBetween_Default), MemberType = typeof(MathsByteExtensionsTestsDataSource))]
    public void IsBetween_Default(byte value, byte min, byte max, bool expectedResult)
    {
        value.IsBetween(min, max).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsByteExtensionsTestsDataSource.IsBetween_BoundsType), MemberType = typeof(MathsByteExtensionsTestsDataSource))]
    public void IsBetween_BoundsType(byte value, byte min, byte max, IsBetweenBoundsType boundsType, bool expectedResult)
    {
        value.IsBetween(min, max, boundsType).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsByteExtensionsTestsDataSource.IsBetweenEither_Default), MemberType = typeof(MathsByteExtensionsTestsDataSource))]
    public void IsBetweenEither_Default(byte value, byte min, byte max, bool expectedResult)
    {
        value.IsBetweenEither(min, max).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsByteExtensionsTestsDataSource.IsBetweenEither_BoundsType), MemberType = typeof(MathsByteExtensionsTestsDataSource))]
    public void IsBetweenEither_BoundsType(byte value, byte min, byte max, IsBetweenBoundsType boundsType, bool expectedResult)
    {
        value.IsBetweenEither(min, max, boundsType).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsByteExtensionsTestsDataSource.IsBetween), MemberType = typeof(MathsByteExtensionsTestsDataSource))]
    public void IsBetween(byte value, byte min, byte max, bool allowEitherOrder, IsBetweenBoundsType boundsType, bool expectedResult)
    {
        value.IsBetween(min, max, allowEitherOrder, boundsType).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsByteExtensionsTestsDataSource.GetLowerBound_Default), MemberType = typeof(MathsByteExtensionsTestsDataSource))]
    public void GetLowerBound_Default(byte min, byte max, byte expectedResult)
    {
        MathsByteExtensions.GetLowerBound(min, max).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsByteExtensionsTestsDataSource.GetLowerBound_AllowEitherOrder), MemberType = typeof(MathsByteExtensionsTestsDataSource))]
    public void GetLowerBound_AllowEitherOrder(byte min, byte max, bool allowEitherOrder, byte expectedResult)
    {
        MathsByteExtensions.GetLowerBound(min, max, allowEitherOrder).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsByteExtensionsTestsDataSource.GetUpperBound_Default), MemberType = typeof(MathsByteExtensionsTestsDataSource))]
    public void GetUpperBound_Default(byte min, byte max, byte expectedResult)
    {
        MathsByteExtensions.GetUpperBound(min, max).ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MathsByteExtensionsTestsDataSource.GetUpperBound_AllowEitherOrder), MemberType = typeof(MathsByteExtensionsTestsDataSource))]
    public void GetUpperBound_AllowEitherOrder(byte min, byte max, bool allowEitherOrder, byte expectedResult)
    {
        MathsByteExtensions.GetUpperBound(min, max, allowEitherOrder).ShouldBe(expectedResult);
    }
}

