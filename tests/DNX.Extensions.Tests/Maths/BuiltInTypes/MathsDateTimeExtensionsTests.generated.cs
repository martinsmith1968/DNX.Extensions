// Code generated by a Template
using System;
using DNX.Helpers.Maths;
using DNX.Helpers.Maths.BuiltInTypes;
using NUnit.Framework;
using Test.DNX.Helpers.Maths.BuiltInTypes.TestsDataSource;

namespace Test.DNX.Helpers.Maths.BuiltInTypes
{
    [TestFixture]
    public class MathsDateTimeExtensionsTests
    {
        [TestCaseSource(typeof(MathsDateTimeExtensionsTestsSource), "IsBetween_Default")]
        public bool IsBetween_Default(DateTime value, DateTime min, DateTime max)
        {
            return value.IsBetween(min, max);
        }

        [TestCaseSource(typeof(MathsDateTimeExtensionsTestsSource), "IsBetween_BoundsType")]
        public bool IsBetween_BoundsType(DateTime value, DateTime min, DateTime max, IsBetweenBoundsType boundsType)
        {
            return value.IsBetween(min, max, boundsType);
        }

        [TestCaseSource(typeof(MathsDateTimeExtensionsTestsSource), "IsBetweenEither_Default")]
        public bool IsBetweenEither_Default(DateTime value, DateTime min, DateTime max)
        {
            return value.IsBetweenEither(min, max);
        }

        [TestCaseSource(typeof(MathsDateTimeExtensionsTestsSource), "IsBetweenEither_BoundsType")]
        public bool IsBetweenEither_BoundsType(DateTime value, DateTime min, DateTime max, IsBetweenBoundsType boundsType)
        {
            return value.IsBetweenEither(min, max, boundsType);
        }

        [TestCaseSource(typeof(MathsDateTimeExtensionsTestsSource), "IsBetween")]
        public bool IsBetween(DateTime value, DateTime min, DateTime max, bool allowEitherOrder, IsBetweenBoundsType boundsType)
        {
            return value.IsBetween(min, max, allowEitherOrder, boundsType);
        }

        [TestCaseSource(typeof(MathsDateTimeExtensionsTestsSource), "GetLowerBound")]
        public DateTime GetLowerBound(DateTime min, DateTime max, bool allowEitherOrder)
        {
            return MathsDateTimeExtensions.GetLowerBound(min, max, allowEitherOrder);
        }

        [TestCaseSource(typeof(MathsDateTimeExtensionsTestsSource), "GetUpperBound")]
        public DateTime GetUpperBound(DateTime min, DateTime max, bool allowEitherOrder)
        {
            return MathsDateTimeExtensions.GetUpperBound(min, max, allowEitherOrder);
        }
    }
}
