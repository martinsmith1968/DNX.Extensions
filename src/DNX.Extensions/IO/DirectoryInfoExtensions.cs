using System.Collections.Generic;
using System.IO;
using System.Linq;
using DNX.Extensions.Strings;

namespace DNX.Extensions.IO;

/// <summary>
/// DirectoryInfo Extensions.
/// </summary>
public static class DirectoryInfoExtensions
{
    /// <summary>
    /// Finds files based on pattern
    /// </summary>
    /// <param name="directoryInfo">The directory information.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="recurseDirectories">if set to <c>true</c> [recurse directories].</param>
    /// <returns>IEnumerable&lt;FileInfo&gt;.</returns>
    public static IEnumerable<FileInfo> FindFiles(this DirectoryInfo directoryInfo, string pattern, bool recurseDirectories = true)
    {
        var searchOption = recurseDirectories
            ? SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;

        var files = directoryInfo.Exists
            ? directoryInfo.EnumerateFiles(pattern, searchOption)
            : [];

        return files;
    }

    /// <summary>
    /// Finds files based on pattern
    /// </summary>
    /// <param name="directoryInfo">The directory information.</param>
    /// <param name="patterns">The patterns.</param>
    /// <param name="recurseDirectories">if set to <c>true</c> [recurse directories].</param>
    /// <returns>IEnumerable&lt;FileInfo&gt;.</returns>
    public static IEnumerable<FileInfo> FindFiles(this DirectoryInfo directoryInfo, string[] patterns, bool recurseDirectories = true)
    {
        var fileInfos = patterns
            .SelectMany(p => directoryInfo.FindFiles(p, recurseDirectories));

        return fileInfos;
    }

    /// <summary>
    /// Finds directories based on pattern
    /// </summary>
    /// <param name="directoryInfo">The directory information.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="recurseDirectories">if set to <c>true</c> [recurse directories].</param>
    /// <returns>IEnumerable&lt;DirectoryInfo&gt;.</returns>
    public static IEnumerable<DirectoryInfo> FindDirectories(this DirectoryInfo directoryInfo, string pattern, bool recurseDirectories = true)
    {
        var searchOption = recurseDirectories
            ? SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;

        var directoryInfos = directoryInfo.Exists
            ? directoryInfo.EnumerateDirectories(pattern, searchOption)
            : [];

        return directoryInfos;
    }

    /// <summary>
    /// Finds directories based on pattern
    /// </summary>
    /// <param name="directoryInfo">The directory information.</param>
    /// <param name="patterns">The patterns.</param>
    /// <param name="recurseDirectories">if set to <c>true</c> [recurse directories].</param>
    /// <returns>IEnumerable&lt;DirectoryInfo&gt;.</returns>
    public static IEnumerable<DirectoryInfo> FindDirectories(this DirectoryInfo directoryInfo, string[] patterns, bool recurseDirectories = true)
    {
        var directoryInfos = patterns
            .SelectMany(p => directoryInfo.FindDirectories(p, recurseDirectories));

        return directoryInfos;
    }

    /// <summary>
    /// Gets the relative file path.
    /// </summary>
    /// <param name="directoryInfo">The directory information.</param>
    /// <param name="owningDirectoryInfo">The owning directory information.</param>
    /// <returns>System.String.</returns>
    public static string GetRelativePath(this DirectoryInfo directoryInfo, DirectoryInfo owningDirectoryInfo)
    {
        var relativePath = owningDirectoryInfo == null || directoryInfo == null
            ? null
            : ""; //Path.GetRelativePath(owningDirectoryInfo.FullName, directoryInfo.FullName)
                //.RemoveStartsWith($".{Path.DirectorySeparatorChar}");

        if (relativePath == ".")
            relativePath = string.Empty;

        return relativePath;
    }
}
