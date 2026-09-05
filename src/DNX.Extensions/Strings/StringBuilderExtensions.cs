using System.Text;

namespace DNX.Extensions.Strings;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendIfNotPresent(this StringBuilder stringBuilder, string text)
    {
        if (stringBuilder != null && !stringBuilder.ToString().EndsWith(text))
        {
            stringBuilder.Append(text);
        }

        return stringBuilder;
    }

    public static StringBuilder AppendSeparator(this StringBuilder stringBuilder, string text = " ")
    {
        if (stringBuilder is { Length: > 0 } && !stringBuilder.ToString().EndsWith(text))
        {
            stringBuilder.Append(text);
        }

        return stringBuilder;
    }
}
