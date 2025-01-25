#define ConversionExtensionsTestsDataSource_Int64

//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using System.Numerics;
using Xunit;

namespace DNX.Extensions.Tests.Conversion.BuiltInTypes.TestsDataSource;

public class ConversionInt64ExtensionsTestsDataSource
{
        private static readonly bool IsSigned = !(
                                            "long".StartsWith("U", StringComparison.OrdinalIgnoreCase)
                                            || "long".Equals("byte", StringComparison.OrdinalIgnoreCase)
                                            );
        private static readonly bool HasDecimalPlaces = (
                                            "long".Equals("float", StringComparison.OrdinalIgnoreCase)
                                            || "long".Equals("double", StringComparison.OrdinalIgnoreCase)
                                            || "long".Equals("decimal", StringComparison.OrdinalIgnoreCase)
                                            );
        private static readonly bool UsesExponentApproximations = !(
                                            "long".Equals("float", StringComparison.OrdinalIgnoreCase)
                                            || "long".Equals("double", StringComparison.OrdinalIgnoreCase)
                                            );

        private static long CreateValue(string text)
        {
            return long.Parse(text);
        }

#if ConversionExtensionsTestsDataSource_Guid
        private static readonly Guid Min_Guid = Guid.Empty;
        private static readonly Guid Max_Guid = Guid.Parse(Guid.Empty.ToString().Replace("0", "F"));
        private static readonly Guid Guid1 = Guid.NewGuid();
        private static readonly Guid Guid2 = Guid.NewGuid();

#endif

    public static TheoryData<string, bool> IsInt64()
    {
        return new TheoryData<string, bool>()
        {
#if ConversionExtensionsTestsDataSource_Bool
            { "0", false },
            { "1", false },
            { "True", true },
            { "False", true },
            { "true", true },
            { "false", true },
            { "Yes", false },
            { "No", false },
#elif ConversionExtensionsTestsDataSource_Guid
            { Min_Guid.ToString(), true },
            { Max_Guid.ToString(), true },
            { Guid1.ToString(), true },
            { Guid2.ToString(), true },
            { Guid.Empty.ToString(), true },
            { nameof(Guid), false },
#else
            { long.MinValue.ToString(), true },
            { long.MaxValue.ToString(), true },
            { "0", true },
            { "1", true },
            { "-1", IsSigned },
            { "100", true },
            { "-100", IsSigned },
            { " 100", true },
            { "100 ", true },
            { "1.5", HasDecimalPlaces },
            { "£100", false },
#endif
        };
    }

    public static TheoryData<string, long> ToInt64()
    {
        return new TheoryData<string, long>()
        {
#if ConversionExtensionsTestsDataSource_Bool
            { "True", true },
            { "False", false },
            { "true", true },
            { "false", false },
#elif ConversionExtensionsTestsDataSource_Guid
            { Guid.Empty.ToString(), Min_Guid },
            { Guid.Empty.ToString().Replace("0", "F"), Max_Guid },
            { Guid1.ToString(), Guid1 },
            { Guid2.ToString(), Guid2 },
#else
            { long.MinValue.ToString(), long.MinValue },
            { long.MaxValue.ToString(), long.MaxValue },
            { "0", 0 },
            { "100", 100 },
            { "10", 10 },
            { "10 ", 10 },
#endif
        };
    }

    public static TheoryData<string, bool> ToInt64Throws()
    {
        return new TheoryData<string, bool>()
        {
#if ConversionExtensionsTestsDataSource_Bool
            { "0", false },
            { "1", false },
            { "false", true },
            { "true", true },
#elif ConversionExtensionsTestsDataSource_Guid
            { "0", false },
            { "1", false },
            { Guid2.ToString(), true },
#else
            { "abcdef", false },
            { "1,000", HasDecimalPlaces },
            { new BigInteger(long.MaxValue).ToString(), true },
            { (new BigInteger(long.MaxValue) + 1).ToString(), !UsesExponentApproximations },
#endif
        };
    }

    public static TheoryData<string, long, long> ToInt64WithDefault()
    {
        return new TheoryData<string, long, long>()
        {
#if ConversionExtensionsTestsDataSource_Bool
            { "abcdef", false, false },
            { "Negatory", true, true },
            { "Affirmative", false, false },
            { "false", true, false },
#elif ConversionExtensionsTestsDataSource_Guid
            { "abcdef", Guid1, Guid1 },
            { Max_Guid.ToString(), Min_Guid, Max_Guid },
#else
            { "abcdef", (long)25, 25 },
            { "50.5", (long)25, HasDecimalPlaces ? CreateValue("50.5") : (long)25 },
            { "100,000", (long)100, HasDecimalPlaces ? (long)(new BigInteger(100000)) : (long)100 },
            { "100", (long)25, 100 },
#endif
        };
    }
}

