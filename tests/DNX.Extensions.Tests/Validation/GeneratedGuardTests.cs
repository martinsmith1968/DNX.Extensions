using System.Linq.Expressions;
using System.Reflection;
using DNX.Extensions.Maths;
using DNX.Extensions.Validation.BuiltInTypes;
using Xunit;

namespace DNX.Extensions.Tests.Validation;

public class GeneratedGuardTests
{
    private static readonly Type[] GuardedTypes =
    {
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(int),
        typeof(long),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(TimeSpan),
        typeof(Guid),
        typeof(ushort),
        typeof(uint),
        typeof(ulong)
    };

    public static IEnumerable<object[]> GuardedTypeData()
        => GuardedTypes.Select(type => new object[] { type });

    [Theory]
    [MemberData(nameof(GuardedTypeData))]
    public void Every_generated_guard_overload_accepts_valid_values(Type valueType)
    {
        foreach (var method in GetGuardMethods(valueType))
        {
            InvokeGuard(method, valueType, valid: true);
        }
    }

    [Theory]
    [MemberData(nameof(GuardedTypeData))]
    public void Every_generated_direct_value_guard_rejects_invalid_values(Type valueType)
    {
        foreach (var method in GetGuardMethods(valueType))
        {
            try
            {
                InvokeGuard(method, valueType, valid: false);
            }
            catch (ArgumentOutOfRangeException)
            {
                continue;
            }

            Assert.Fail($"Expected {method} to throw for {valueType}.");
        }
    }

    private static IEnumerable<MethodInfo> GetGuardMethods(Type valueType)
    {
        return typeof(Guard)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method =>
                method.DeclaringType == typeof(Guard) &&
                method.Name is "IsGreaterThan" or
                    "IsGreaterThanOrEqualTo" or
                    "IsLessThan" or
                    "IsLessThanOrEqualTo" or
                    "IsBetween" &&
                method.GetParameters()[0].ParameterType == typeof(Expression<>).MakeGenericType(
                    typeof(Func<>).MakeGenericType(valueType)));
    }

    private static void InvokeGuard(MethodInfo method, Type valueType, bool valid)
    {
        var parameters = method.GetParameters();
        var expression = CreateExpression(valueType);
        var arguments = new object[parameters.Length];
        arguments[0] = expression;

        var value = CreateValue(valueType, 7);
        var lower = CreateValue(valueType, 5);
        var upper = CreateValue(valueType, 10);

        switch (method.Name)
        {
            case "IsGreaterThan":
                arguments[1] = valid ? lower : upper;
                if (parameters.Length == 3)
                {
                    arguments[1] = value;
                    arguments[2] = valid ? lower : upper;
                }
                break;

            case "IsGreaterThanOrEqualTo":
                arguments[1] = valid ? value : upper;
                if (parameters.Length == 3)
                {
                    arguments[1] = value;
                    arguments[2] = valid ? value : upper;
                }
                break;

            case "IsLessThan":
                arguments[1] = valid ? upper : lower;
                if (parameters.Length == 3)
                {
                    arguments[1] = value;
                    arguments[2] = valid ? upper : lower;
                }
                break;

            case "IsLessThanOrEqualTo":
                arguments[1] = valid ? value : lower;
                if (parameters.Length == 3)
                {
                    arguments[1] = value;
                    arguments[2] = valid ? value : lower;
                }
                break;

            case "IsBetween":
                if (parameters.Length == 6)
                {
                    // Direct-value overload: exp's constant stays fixed, vary the precomputed probe instead.
                    arguments[1] = valid ? value : CreateValue(valueType, 12);
                    arguments[2] = lower;
                    arguments[3] = upper;
                    arguments[4] = true;
                    arguments[5] = IsBetweenBoundsType.Inclusive;
                }
                else
                {
                    // Convenience overloads always evaluate exp to the fixed constant (7), so move the
                    // bounds themselves to exclude it when testing the invalid case.
                    arguments[1] = valid ? lower : upper;
                    arguments[2] = valid ? upper : CreateValue(valueType, 12);

                    if (parameters.Length == 4)
                    {
                        arguments[3] = IsBetweenBoundsType.Inclusive;
                    }
                    else if (parameters.Length == 5)
                    {
                        arguments[3] = true;
                        arguments[4] = IsBetweenBoundsType.Inclusive;
                    }
                }
                break;
        }

        try
        {
            method.Invoke(null, arguments);
        }
        catch (TargetInvocationException exception) when (exception.InnerException != null)
        {
            throw exception.InnerException;
        }
    }

    private static LambdaExpression CreateExpression(Type valueType)
    {
        var delegateType = typeof(Func<>).MakeGenericType(valueType);
        var body = Expression.Constant(CreateValue(valueType, 7), valueType);

        return Expression.Lambda(delegateType, body);
    }

    private static object CreateValue(Type valueType, int value)
    {
        if (valueType == typeof(DateTime))
        {
            return new DateTime(2020, 1, 1).AddDays(value);
        }

        if (valueType == typeof(DateTimeOffset))
        {
            return new DateTimeOffset(new DateTime(2020, 1, 1).AddDays(value), TimeSpan.Zero);
        }

        if (valueType == typeof(TimeSpan))
        {
            return TimeSpan.FromDays(value);
        }

        if (valueType == typeof(Guid))
        {
            // Guid has no native ordering: Guard compares via ToBigInteger(), which treats
            // Guid.ToByteArray() as a little-endian integer, so only the last byte need vary.
            var bytes = new byte[16];
            bytes[15] = (byte)value;
            return new Guid(bytes);
        }

        return Convert.ChangeType(value, valueType);
    }
}
