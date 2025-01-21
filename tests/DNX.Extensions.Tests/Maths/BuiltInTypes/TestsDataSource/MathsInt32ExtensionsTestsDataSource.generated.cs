#define MathsExtensionsTestsDataSource_Int32

//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using System.Numerics;
using DNX.Extensions.Conversion;
using DNX.Extensions.Maths;
using Xunit;

namespace DNX.Extensions.Tests.Maths.BuiltInTypes.TestsDataSource;

public class MathsInt32ExtensionsTestsDataSource
{
#if MathsExtensionsTestsDataSource_DateTime
    private static int CreateDataValue(int value)
#elif MathsExtensionsTestsDataSource_Guid
    private static int CreateDataValue(int value)
#else
    private static int CreateDataValue(int value)
#endif
    {
#if MathsExtensionsTestsDataSource_DateTime
        var epoch = new DateTime(2017, 01, 01);

        return epoch.AddDays(Convert.ToInt32(value));
#elif MathsExtensionsTestsDataSource_Guid
        return value.ToGuid();
#else
        return value;
#endif
    }

    public static TheoryData<int, int, int, bool> IsBetween_Default()
    {
        return new TheoryData<int, int, int, bool>()
        {
            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), false },
        };
    }

    public static TheoryData<int, int, int, IsBetweenBoundsType, bool> IsBetween_BoundsType()
    {
        return new TheoryData<int, int, int, IsBetweenBoundsType, bool>()
        {
            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), (IsBetweenBoundsType)int.MaxValue, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), (IsBetweenBoundsType)int.MaxValue, false },
        };
    }

    public static TheoryData<int, int, int, bool> IsBetweenEither_Default()
    {
        return new TheoryData<int, int, int, bool>()
        {
            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), false },
        };
    }

    public static TheoryData<int, int, int, IsBetweenBoundsType, bool> IsBetweenEither_BoundsType()
    {
        return new TheoryData<int, int, int, IsBetweenBoundsType, bool>()
        {
            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Inclusive, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.Exclusive, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
        };
    }

    public static TheoryData<int, int, int, bool, IsBetweenBoundsType, bool> IsBetween()
    {
        return new TheoryData<int, int, int, bool, IsBetweenBoundsType, bool>()
        {
            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Inclusive, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.Exclusive, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), false, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Inclusive, true },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Inclusive, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Inclusive, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Exclusive, true },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Exclusive, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.Exclusive, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, true },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, false },

            { CreateDataValue(5), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(2), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(9), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(1), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(5), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(10), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, true },
            { CreateDataValue(20), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(0), CreateDataValue(1), CreateDataValue(10), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
            { CreateDataValue(0), CreateDataValue(10), CreateDataValue(1), true, IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, false },
        };
    }

    public static TheoryData<int, int, int> GetLowerBound_Default()
    {
        return new TheoryData<int, int, int>()
        {
            { CreateDataValue(5), CreateDataValue(5), CreateDataValue(5) },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(1) },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(10) },
            { CreateDataValue(10), CreateDataValue(20), CreateDataValue(10) },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(20) },
        };
    }

    public static TheoryData<int, int, bool, int> GetLowerBound_AllowEitherOrder()
    {
        return new TheoryData<int, int, bool, int>()
        {
            { CreateDataValue(1), CreateDataValue(10), true, CreateDataValue(1) },
            { CreateDataValue(1), CreateDataValue(10), false, CreateDataValue(1) },
            { CreateDataValue(10), CreateDataValue(1), true, CreateDataValue(1) },
            { CreateDataValue(10), CreateDataValue(1), false, CreateDataValue(10) },
            { CreateDataValue(10), CreateDataValue(10), true, CreateDataValue(10) },
            { CreateDataValue(10), CreateDataValue(10), false, CreateDataValue(10) },
        };
    }

    public static TheoryData<int, int, int> GetUpperBound_Default()
    {
        return new TheoryData<int, int, int>()
        {
            { CreateDataValue(5), CreateDataValue(5), CreateDataValue(5) },
            { CreateDataValue(1), CreateDataValue(10), CreateDataValue(10) },
            { CreateDataValue(10), CreateDataValue(1), CreateDataValue(1) },
            { CreateDataValue(10), CreateDataValue(20), CreateDataValue(20) },
            { CreateDataValue(20), CreateDataValue(10), CreateDataValue(10) },
        };
    }

    public static TheoryData<int, int, bool, int> GetUpperBound_AllowEitherOrder()
    {
        return new TheoryData<int, int, bool, int>()
        {
            { CreateDataValue(1), CreateDataValue(10), true, CreateDataValue(10) },
            { CreateDataValue(1), CreateDataValue(10), false, CreateDataValue(10) },
            { CreateDataValue(10), CreateDataValue(1), true, CreateDataValue(10) },
            { CreateDataValue(10), CreateDataValue(1), false, CreateDataValue(1) },
            { CreateDataValue(10), CreateDataValue(10), true, CreateDataValue(10) },
            { CreateDataValue(10), CreateDataValue(10), false, CreateDataValue(10) },
        };
    }
}

