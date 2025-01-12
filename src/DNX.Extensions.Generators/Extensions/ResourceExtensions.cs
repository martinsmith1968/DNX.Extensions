using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DNX.Extensions.Generators.Extensions;

[ExcludeFromCodeCoverage]
internal static class ResourceExtensions
{
    internal static string GetEmbeddedResourceText(this object obj, string resourceName)
    {
        var type = obj.GetType();

        var assembly = type.Assembly;

        var manifestFileInfo = assembly.GetManifestResourceNames()
            .SingleOrDefault(x => x.EndsWith(resourceName));

        if (string.IsNullOrWhiteSpace(manifestFileInfo))
            throw new Exception($"Unable to locate resource : {resourceName}");

        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestFileInfo);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
