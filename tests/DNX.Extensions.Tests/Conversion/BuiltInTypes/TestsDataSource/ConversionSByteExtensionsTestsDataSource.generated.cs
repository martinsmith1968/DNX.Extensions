#define ConversionExtensionsTestsDataSource_SByte

//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using System.Numerics;
using Xunit;

namespace DNX.Extensions.Tests.Conversion.BuiltInTypes.TestsDataSource;

public class ConversionSByteExtensionsTestsDataSource
{
        private static readonly bool IsSigned = !(
                                            "sbyte".StartsWith("U", StringComparison.OrdinalIgnoreCase)
                                            || "sbyte".Equals("byte", StringComparison.OrdinalIgnoreCase)
                                            );
        private static readonly bool HasDecimalPlaces = (
                                            "sbyte".Equals("float", StringComparison.OrdinalIgnoreCase)
                                            || "sbyte".Equals("double", StringComparison.OrdinalIgnoreCase)
                                            || "sbyte".Equals("decimal", StringComparison.OrdinalIgnoreCase)
                                            );
        private static readonly bool UsesExponentApproximations = !(
                                            "sbyte".Equals("float", StringComparison.OrdinalIgnoreCase)
                                            || "sbyte".Equals("double", StringComparison.OrdinalIgnoreCase)
                                            );

        private static sbyte CreateValue(string text)
        {
            return sbyte.Parse(text);
        }

#if ConversionExtensionsTestsDataSource_Guid
        private static readonly Guid Min_Guid = Guid.Empty;
        private static readonly Guid Max_Guid = Guid.Parse(Guid.Empty.ToString().Replace("0", "F"));
        private static readonly Guid Guid1 = Guid.NewGuid();
        private static readonly Guid Guid2 = Guid.NewGuid();

#endif

    public static TheoryData<string, bool> IsSByte()
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
            { sbyte.MinValue.ToString(), true },
            { sbyte.MaxValue.ToString(), true },
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

    public static TheoryData<string, sbyte> ToSByte()
    {
        return new TheoryData<string, sbyte>()
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
            { sbyte.MinValue.ToString(), sbyte.MinValue },
            { sbyte.MaxValue.ToString(), sbyte.MaxValue },
            { "0", 0 },
            { "100", 100 },
            { "10", 10 },
            { "10 ", 10 },
#endif
        };
    }

    public static TheoryData<string, bool> ToSByteThrows()
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
            { new BigInteger(sbyte.MaxValue).ToString(), true },
            { (new BigInteger(sbyte.MaxValue) + 1).ToString(), !UsesExponentApproximations },
#endif
        };
    }

    public static TheoryData<string, sbyte, sbyte> ToSByteWithDefault()
    {
        return new TheoryData<string, sbyte, sbyte>()
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
            { "abcdef", (sbyte)25, 25 },
            { "50.5", (sbyte)25, HasDecimalPlaces ? CreateValue("50.5") : (sbyte)25 },
            { "100,000", (sbyte)100, HasDecimalPlaces ? (sbyte)(new BigInteger(100000)) : (sbyte)100 },
            { "100", (sbyte)25, 100 },
#endif
        };
    }
}

