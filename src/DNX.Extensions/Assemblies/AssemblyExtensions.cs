using System;
using System.IO;
using System.Reflection;
using System.Resources;

// ReSharper disable ConvertToUsingDeclaration

namespace DNX.Extensions.Assemblies;

/// <summary>
/// Assembly Extensions
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Gets the embedded resource text.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="relativeResourceName">Name of the relative resource.</param>
    /// <param name="nameSpace">The name space.</param>
    /// <returns></returns>
    /// <exception cref="MissingManifestResourceException"></exception>
    public static string GetEmbeddedResourceText(this Assembly assembly, string relativeResourceName, string nameSpace = null)
    {
        try
        {
            nameSpace = string.IsNullOrWhiteSpace(nameSpace)
                ? Path.GetFileNameWithoutExtension(assembly.Location)
                : nameSpace;

            var resourceName = $"{nameSpace}.{relativeResourceName}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();

                    return result;
                }
            }
        }
        catch (Exception e)
        {
            throw new MissingManifestResourceException($"{relativeResourceName} not found", e);
        }
    }
}
