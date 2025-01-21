using System.Collections.Generic;
using System.Linq;
using DNX.Extensions.Strings;

// ReSharper disable UseStringInterpolation

namespace DNX.Extensions.Objects;

/// <summary>
/// Class ObjectExtensions.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Converts to string, or default.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>System.String.</returns>
    public static string ToStringOrDefault(this object obj, string defaultValue = "")
    {
        return obj?.ToString() ?? defaultValue;
    }

    /// <summary>
    /// Gets the unique instance identifier.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>System.String.</returns>
    public static string GetUniqueInstanceId(this object obj)
    {
        return obj.GetUniqueInstanceId(null);
    }

    /// <summary>
    /// Gets the unique instance identifier.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="instanceIdOverride">The instance identifier override.</param>
    /// <returns>System.String.</returns>
    public static string GetUniqueInstanceId(this object obj, string instanceIdOverride)
    {
        return string.Format("{0}:{1}",
            obj.GetType().FullName,
            instanceIdOverride.CoalesceNullOrEmptyWith(obj.GetHashCode().ToString())
        );
    }

    /// <summary>
    /// Coalesces the list of objects to find the first not null
    /// </summary>
    /// <param name="objects">The objects.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static object CoalesceNull(params object[] objects)
    {
        return objects.CoalesceNull();
    }

    /// <summary>
    /// Coalesces the list of objects to find the first not null
    /// </summary>
    /// <param name="objects">The objects.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static object CoalesceNull(this IList<object> objects)
    {
        var value = objects.FirstOrDefault(o => o != null);

        return value;
    }
}
