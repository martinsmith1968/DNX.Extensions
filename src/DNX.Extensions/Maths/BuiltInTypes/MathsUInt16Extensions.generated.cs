#define MathsExtensionsUInt16

//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using System;
using DNX.Extensions.Conversion;

namespace DNX.Extensions.Maths.BuiltInTypes;

/// <summary>
/// Class MathUInt16Extensions.
/// </summary>
public static class MathsUInt16Extensions
{
    /// <summary>
    /// Determines whether the specified value is inclusively between min and max.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <returns><c>true</c> if the specified minimum is between min and max; otherwise, <c>false</c>.</returns>
    public static bool IsBetween(this ushort value, ushort min, ushort max)
    {
        return value.IsBetween(min, max, IsBetweenBoundsType.Inclusive);
    }

    /// <summary>
    /// Determines whether the specified value is between min and max.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <param name="boundsType">Control boundary checking.</param>
    /// <returns><c>true</c> if the specified minimum is between; otherwise, <c>false</c>.</returns>
    public static bool IsBetween(this ushort value, ushort min, ushort max, IsBetweenBoundsType boundsType)
    {
        return value.IsBetween(min, max, false, boundsType);
    }

    /// <summary>
    /// Determines whether the specified value is inclusively between the smaller of min and max and the larger of min and max.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <returns><c>true</c> if the specified minimum is between min and max; otherwise, <c>false</c>.</returns>
    public static bool IsBetweenEither(this ushort value, ushort min, ushort max)
    {
        return value.IsBetween(min, max, IsBetweenBoundsType.Inclusive);
    }

    /// <summary>
    /// Determines whether the specified value is between the smaller of min and max and the larger of min and max.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <param name="boundsType">Control boundary checking.</param>
    /// <returns><c>true</c> if [is between either] [the specified minimum]; otherwise, <c>false</c>.</returns>
    public static bool IsBetweenEither(this ushort value, ushort min, ushort max, IsBetweenBoundsType boundsType)
    {
        return value.IsBetween(min, max, true, boundsType);
    }

    /// <summary>
    /// Determines whether the specified value is between min and max with full control over bounds checking.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <param name="allowEitherOrder">if set to <c>true</c> allow min/max in either order.</param>
    /// <param name="boundsType">Control boundary checking.</param>
    /// <returns>
    ///   <c>true</c> if the specified minimum is between min and max; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBetween(this ushort value, ushort min, ushort max, bool allowEitherOrder, IsBetweenBoundsType boundsType)
    {
        var lowerBound = GetLowerBound(min, max, allowEitherOrder);
        var upperBound = GetUpperBound(min, max, allowEitherOrder);

        switch (boundsType)
        {
#if MathsExtensionsGuid
            case IsBetweenBoundsType.IncludeLowerAndUpper:
                return (value.ToBigInteger() >= lowerBound.ToBigInteger()) && (value.ToBigInteger() <= upperBound.ToBigInteger());

            case IsBetweenBoundsType.ExcludeLowerAndUpper:
                return (value.ToBigInteger() > lowerBound.ToBigInteger()) && (value.ToBigInteger() < upperBound.ToBigInteger());

            case IsBetweenBoundsType.IncludeLowerExcludeUpper:
                return (value.ToBigInteger() >= lowerBound.ToBigInteger()) && (value.ToBigInteger() < upperBound.ToBigInteger());

            case IsBetweenBoundsType.ExcludeLowerIncludeUpper:
                return (value.ToBigInteger() > lowerBound.ToBigInteger()) && (value.ToBigInteger() <= upperBound.ToBigInteger());
#else
            case IsBetweenBoundsType.IncludeLowerAndUpper:
                return (value >= lowerBound) && (value <= upperBound);

            case IsBetweenBoundsType.ExcludeLowerAndUpper:
                return (value > lowerBound) && (value < upperBound);

            case IsBetweenBoundsType.IncludeLowerExcludeUpper:
                return (value >= lowerBound) && (value < upperBound);

            case IsBetweenBoundsType.ExcludeLowerIncludeUpper:
                return (value > lowerBound) && (value <= upperBound);
#endif

            default:
                return false;
        }
    }

    /// <summary>
    /// Gets the lower bound.
    /// </summary>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <returns>ushort</returns>
    public static ushort GetLowerBound(ushort min, ushort max)
    {
        return GetLowerBound(min, max, false);
    }

    /// <summary>
    /// Gets the lower bound.
    /// </summary>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <param name="allowEitherOrder">if set to <c>true</c> allow min/max in either order</param>
    /// <returns>ushort</returns>
    public static ushort GetLowerBound(ushort min, ushort max, bool allowEitherOrder)
    {
        return allowEitherOrder
#if MathsExtensionsGuid
            ? min.ToBigInteger() < max.ToBigInteger() ? min : max
#else
            ? min < max ? min : max
#endif
            : min;
    }

    /// <summary>
    /// Gets the upper bound.
    /// </summary>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <returns>ushort</returns>
    public static ushort GetUpperBound(ushort min, ushort max)
    {
        return GetUpperBound(min, max, false);
    }

    /// <summary>
    /// Gets the upper bound.
    /// </summary>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <param name="allowEitherOrder">if set to <c>true</c> allow min/max in either order</param>
    /// <returns>ushort</returns>
    public static ushort GetUpperBound(ushort min, ushort max, bool allowEitherOrder)
    {
        return allowEitherOrder
#if MathsExtensionsGuid
            ? min.ToBigInteger() > max.ToBigInteger() ? min : max
#else
            ? min > max ? min : max
#endif
            : max;
    }
}

