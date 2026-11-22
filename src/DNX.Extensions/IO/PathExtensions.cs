using System.IO;
using DNX.Extensions.Validation.BuiltInTypes;

namespace DNX.Extensions.IO;

/// <summary>
/// Path Extensions
/// </summary>
public static class PathExtensions
{
    /// <summary>
    /// Combines the path with the specified additional path segments.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="paths">The additional path segments.</param>
    /// <returns>System.String.</returns>
    public static string CombineWith(this string path, params string[] paths)
    {
        Guard.IsNotNull(() => path);

        return Path.Combine([path, .. paths ?? []]);
    }
}
