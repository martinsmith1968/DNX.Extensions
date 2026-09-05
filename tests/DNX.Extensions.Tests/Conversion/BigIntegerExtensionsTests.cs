using System.Numerics;
using DNX.Extensions.Conversion;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Conversion;

public class BigIntegerExtensionsTests
{
    [Theory]
    [MemberData(nameof(ToGuid_Data))]
    public void ToGuid_should_convert_as_expected(BigInteger value, Guid expectedResult)
    {
        // Act
        var result = value.ToGuid();

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ToBigInteger_Data))]
    public void ToBigInteger_should_convert_as_expected(Guid value, BigInteger expectedResult)
    {
        // Act
        var result = value.ToBigInteger();

        // Assert
        result.ShouldBe(expectedResult);
    }

    public static TheoryData<BigInteger, Guid> ToGuid_Data()
    {
        var data = new TheoryData<BigInteger, Guid>();

        StaticData()
            .ToList()
            .ForEach(x => { data.Add(x.Item1, x.Item2); });

        return data;
    }

    public static TheoryData<Guid, BigInteger> ToBigInteger_Data()
    {
        var data = new TheoryData<Guid, BigInteger>();

        StaticData()
            .ToList()
            .ForEach(x => { data.Add(x.Item2, x.Item1);});

        return data;
    }

    private static IEnumerable<Tuple<BigInteger, Guid>> StaticData()
    {
        return
        [
            new Tuple<BigInteger, Guid> ( new BigInteger(0), Guid.Empty ),
            new Tuple<BigInteger, Guid> ( new BigInteger(1), Guid.Parse("00000001-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(10), Guid.Parse("0000000A-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(16), Guid.Parse("00000010-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(256), Guid.Parse("00000100-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(1024), Guid.Parse("00000400-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(4096), Guid.Parse("00001000-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(8192), Guid.Parse("00002000-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(16384), Guid.Parse("00004000-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(short.MaxValue), Guid.Parse("00007fff-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(ushort.MaxValue), Guid.Parse("0000ffff-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(int.MaxValue), Guid.Parse("7fffffff-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(uint.MaxValue), Guid.Parse("ffffffff-0000-0000-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(long.MaxValue), Guid.Parse("ffffffff-ffff-7fff-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(ulong.MaxValue), Guid.Parse("ffffffff-ffff-ffff-0000-000000000000") ),
            new Tuple<BigInteger, Guid> ( new BigInteger(decimal.MaxValue), Guid.Parse("ffffffff-ffff-ffff-ffff-ffff00000000") ),
            //new Tuple<BigInteger, Guid> ( new BigInteger(float.MaxValue), Guid.Parse("00000000-0000-0000-0000-000000ffffff") ),
            //new Tuple<BigInteger, Guid> ( new BigInteger(double.MaxValue), Guid.Parse("00000000-0000-0000-0000-000000000000") ),
        ];
    }
}
