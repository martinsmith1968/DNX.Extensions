using System;
using System.ComponentModel;

namespace DNX.Extensions.Enums;

/// <summary>
/// Enum Extensions
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the attribute.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="en">The en.</param>
    /// <returns></returns>
    public static T GetAttribute<T>(this Enum en)
    {
        var type = en.GetType();

        var memInfo = type.GetMember(en.ToString());

        if (memInfo.Length > 0)
        {
            var attrs = memInfo[0].GetCustomAttributes(typeof(T), false);

            if (attrs.Length > 0)
            {
                return (T)attrs[0];
            }
        }

        return default;
    }

    /// <summary>
    /// Retrieve the description on the enum, e.g.
    /// [Description("Bright Pink")]
    /// BrightPink = 2,
    /// Then when you pass in the enum, it will retrieve the description
    /// </summary>
    /// <param name="en">The Enumeration</param>
    /// <returns>A string representing the friendly name</returns>
    public static string GetDescription(this Enum en)
    {
        var attr = en.GetAttribute<DescriptionAttribute>();

        return attr == null
            ? en.ToString()
            : attr.Description;
    }
}
