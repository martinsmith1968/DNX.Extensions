#define ConversionExtensionsTestsDataSource_UInt64

//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using System.Numerics;
using Xunit;

namespace DNX.Extensions.Tests.Conversion.BuiltInTypes.TestsDataSource;

public class ConversionUInt64ExtensionsTestsDataSource
{
        private static readonly bool IsSigned = !(
                                            "ulong".StartsWith("U", StringComparison.OrdinalIgnoreCase)
                                            || "ulong".Equals("byte", StringComparison.OrdinalIgnoreCase)
                                            );
        private static readonly bool HasDecimalPlaces = (
                                            "ulong".Equals("float", StringComparison.OrdinalIgnoreCase)
                                            || "ulong".Equals("double", StringComparison.OrdinalIgnoreCase)
                                            || "ulong".Equals("decimal", StringComparison.OrdinalIgnoreCase)
                                            );
        private static readonly bool UsesExponentApproximations = !(
                                            "ulong".Equals("float", StringComparison.OrdinalIgnoreCase)
                                            || "ulong".Equals("double", StringComparison.OrdinalIgnoreCase)
                                            );

        private static ulong CreateValue(string text)
        {
            return ulong.Parse(text);
        }

#if ConversionExtensionsTestsDataSource_Guid
        private static readonly Guid Min_Guid = Guid.Empty;
        private static readonly Guid Max_Guid = Guid.Parse(Guid.Empty.ToString().Replace("0", "F"));
        private static readonly Guid Guid1 = Guid.NewGuid();
        private static readonly Guid Guid2 = Guid.NewGuid();

#endif

    public static TheoryData<string, bool> IsUInt64()
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
            { ulong.MinValue.ToString(), true },
            { ulong.MaxValue.ToString(), true },
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

    public static TheoryData<string, ulong> ToUInt64()
    {
        return new TheoryData<string, ulong>()
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
            { ulong.MinValue.ToString(), ulong.MinValue },
            { ulong.MaxValue.ToString(), ulong.MaxValue },
            { "0", 0 },
            { "100", 100 },
            { "10", 10 },
            { "10 ", 10 },
#endif
        };
    }

    public static TheoryData<string, bool> ToUInt64Throws()
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
            { new BigInteger(ulong.MaxValue).ToString(), true },
            { (new BigInteger(ulong.MaxValue) + 1).ToString(), !UsesExponentApproximations },
#endif
        };
    }

    public static TheoryData<string, ulong, ulong> ToUInt64WithDefault()
    {
        return new TheoryData<string, ulong, ulong>()
        {
#if ConversionExtensionsTestsDataSource_Bool
            { "abcdef", false, false },
            { "Negatory", true, true },
            { "Affirmative", false, false },
            { "false", true, false },
#elif ConversionExtensionsTestsDataSource_Guid
            { "", Guid2, Guid2 },
            { "abcdef", Guid1, Guid1 },
            { "0", Guid1, Guid1 },
            { Max_Guid.ToString(), Min_Guid, Max_Guid },
#else
            { "abcdef", (ulong)25, 25 },
            { "50.5", (ulong)25, HasDecimalPlaces ? CreateValue("50.5") : (ulong)25 },
            { "100,000", (ulong)100, HasDecimalPlaces ? (ulong)(new BigInteger(100000)) : (ulong)100 },
            { "100", (ulong)25, 100 },
#endif
        };
    }
}

