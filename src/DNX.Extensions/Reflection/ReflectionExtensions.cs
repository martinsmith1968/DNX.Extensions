using System.Reflection;

namespace DNX.Extensions.Reflection;

/// <summary>
/// Extensions for simplifying Reflection tasks
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Gets the property value of an instance by name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="flags">The flags.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>System.Object.</returns>
    public static object GetPropertyValueByName<T>(this T instance, string propertyName, BindingFlags flags, object defaultValue = default)
    {
        var pi = typeof(T).GetProperty(propertyName, flags);
        if (pi == null)
        {
            return defaultValue;
        }

        var allowNonPublic = flags.HasFlag(BindingFlags.NonPublic) || !flags.HasFlag(BindingFlags.Public);
        var getter = pi.GetGetMethod(allowNonPublic);

        var value = getter?.Invoke(instance, null)
                    ?? defaultValue;

        return value;
    }

    /// <summary>
    /// Gets a private property value by name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>System.Object.</returns>
    public static object GetPrivatePropertyValue<T>(this T instance, string propertyName, object defaultValue = default)
    {
        return instance.GetPropertyValueByName(propertyName, BindingFlags.Instance | BindingFlags.NonPublic, defaultValue);
    }
}
