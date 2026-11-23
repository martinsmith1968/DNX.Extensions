using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DNX.Extensions.Execution;
using DNX.Extensions.Reflection;
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
    /// Converts the object to the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="exceptionHandler">Invoked with the exception if the cast fails, instead of discarding it silently.</param>
    /// <returns>T.</returns>
    public static T To<T>(this object obj, T defaultValue = default, Action<Exception> exceptionHandler = null)
    {
        return RunSafely.Execute(() => (T)obj ?? defaultValue, defaultValue, exceptionHandler);
    }

    /// <summary>
    /// Converts the supplied object to dictionary.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static Dictionary<string, object> ToDictionary(this object obj)
    {
        return obj?.GetObjectPropertyValues(ReflectionExtensions.GetPropertiesForType(obj.GetType()));
    }

    /// <summary>
    /// Converts the supplied object to dictionary.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="bindingFlags">The binding flags.</param>
    /// <returns></returns>
    public static Dictionary<string, object> ToDictionary(this object obj, BindingFlags bindingFlags)
    {
        return obj?.GetObjectPropertyValues(ReflectionExtensions.GetPropertiesForType(obj.GetType(), bindingFlags));
    }

    private static Dictionary<string, object> GetObjectPropertyValues(this object obj, IList<PropertyInfo> properties)
    {
        return properties
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .ToDictionary(p => p.Name, p => p.GetValue(obj));
    }

    /// <summary>
    /// Changes the value to the specified type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object.</param>
    /// <returns>T.</returns>
    public static T ChangeType<T>(this object obj)
    {
        return (T)ChangeType(obj, typeof(T));
    }

    /// <summary>
    /// Changes the value to the specified type
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="type">The type.</param>
    /// <returns>object</returns>
    public static object ChangeType(this object obj, Type type)
    {
        return Convert.ChangeType(obj, type);
    }

    /// <summary>
    /// Changes the value to the specified type, with a default value if conversion fails
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="exceptionHandler">Invoked with the exception if the conversion fails, instead of discarding it silently.</param>
    /// <returns>T.</returns>
    public static T ChangeType<T>(this object obj, T defaultValue, Action<Exception> exceptionHandler = null)
    {
        return RunSafely.Execute(() => ChangeType<T>(obj), defaultValue, exceptionHandler);
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
