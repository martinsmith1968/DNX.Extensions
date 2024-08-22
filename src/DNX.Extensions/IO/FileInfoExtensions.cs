using System;
using System.IO;

namespace DNX.Extensions.IO;

/// <summary>
/// FileInfo Extensions.
/// </summary>
public static class FileInfoExtensions
{
    /// <summary>
    /// Gets the name of the relative file.
    /// </summary>
    /// <param name="fileInfo">The file information.</param>
    /// <param name="directoryInfo">The directory information.</param>
    /// <returns>System.String.</returns>
    public static string GetRelativeFileName(this FileInfo fileInfo, DirectoryInfo directoryInfo)
    {
        return Path.Combine(fileInfo.GetRelativeFilePath(directoryInfo), fileInfo.Name);
    }

    /// <summary>
    /// Gets the relative file path.
    /// </summary>
    /// <param name="fileInfo">The file information.</param>
    /// <param name="directoryInfo">The directory information.</param>
    /// <returns>System.String.</returns>
    public static string GetRelativeFilePath(this FileInfo fileInfo, DirectoryInfo directoryInfo)
    {
        return fileInfo.Directory.GetRelativePath(directoryInfo);
    }

    /// <summary>
    /// Gets the friendly size of the file.
    /// </summary>
    /// <param name="fileInfo">The file information.</param>
    /// <returns>System.String.</returns>
    public static string GetFriendlyFileSize(FileInfo fileInfo)
    {
        return GetFriendlyFileSize(fileInfo?.Length ?? 0);
    }

    /// <summary>
    /// Gets the friendly size of the file.
    /// </summary>
    /// <param name="fileSize">Size of the file.</param>
    /// <returns>System.String.</returns>
    /// &gt;
    /// <remarks>
    /// Based on: https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net
    /// </remarks>
    public static string GetFriendlyFileSize(long fileSize)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB

        var num = 0d;
        var place = 0;

        if (fileSize > 0)
        {
            var bytes = Math.Abs(fileSize);

            place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            num = Math.Round(bytes / Math.Pow(1024, place), 1);
        }

        return $"{Math.Sign(fileSize) * num}{suffixes[place]}";
    }
}
