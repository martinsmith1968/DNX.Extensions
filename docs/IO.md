# DNX.Extensions.IO

Extension methods for `DirectoryInfo`, `FileInfo` and `string` (as a path) - finding files and
directories by pattern, ensuring a directory exists, computing relative paths, getting a
human-friendly file size, and combining path segments.

## DirectoryInfoExtensions class

| Method Name | Parameters | Description |
| --- | --- | --- |
| FindFiles | pattern, recurseDirectories (default `true`) | Finds files matching `pattern`. Returns an empty sequence (not an exception) if the directory does not exist |
| FindFiles | patterns (string[]), recurseDirectories (default `true`) | As above, for multiple patterns, concatenating the results of each |
| FindDirectories | pattern, recurseDirectories (default `true`) | Finds subdirectories matching `pattern`. Returns an empty sequence if the directory does not exist |
| FindDirectories | patterns (string[]), recurseDirectories (default `true`) | As above, for multiple patterns, concatenating the results of each |
| EnsureExists | | Creates the directory (and any missing parent directories) if it doesn't already exist, then refreshes and returns the same `DirectoryInfo`. Throws `ArgumentNullException` if `directoryInfo` is null |
| GetRelativePath | owningDirectoryInfo | Returns this directory's path relative to `owningDirectoryInfo`. Returns `null` if either argument is null |

Examples:

```csharp
// Given a directory tree:
//   root\file.txt
//   root\file.json
//   root\dir1\file1.txt
//   root\dir1\file1.json
//   root\dir1\dur3\file1.tf
//   root\dir2\file2.txt
//   root\dir2\file2.json
//   root\dir2\dur4\file2.tf

var root = new DirectoryInfo("root");

// FindFiles
var topLevelTxt = root.FindFiles("*.txt", recurseDirectories: false);
// topLevelTxt contains 1 file: file.txt

var allTxt = root.FindFiles("*.txt", recurseDirectories: true);
// allTxt contains 3 files: file.txt, file1.txt, file2.txt

var allTxtAndJson = root.FindFiles(["*.txt", "*.json"], recurseDirectories: true);
// allTxtAndJson contains 6 files (3 *.txt + 3 *.json)

var missingDirFiles = new DirectoryInfo("does-not-exist").FindFiles("*.txt");
// missingDirFiles is an empty sequence, not an exception

// FindDirectories
var topLevelDirs = root.FindDirectories("dir*", recurseDirectories: false);
// topLevelDirs contains 2 directories: dir1, dir2

var allMatchingDirs = root.FindDirectories("d?r*", recurseDirectories: true);
// allMatchingDirs contains 4 directories: dir1, dir2, dur3, dur4

// EnsureExists
var createdDir = new DirectoryInfo(Path.Combine(root.FullName, "missing-subdir"));
createdDir.Exists.ShouldBeFalse(); // not created yet
var result = createdDir.EnsureExists();
// result.Exists == true, and result is the same DirectoryInfo instance, refreshed

// GetRelativePath
var sameDir = root.GetRelativePath(root);
// sameDir == "" (same directory)

var childDir = new DirectoryInfo(Path.Combine(root.FullName, "dir3"));
var childRelative = childDir.GetRelativePath(root);
// childRelative == "dir3"

var siblingDir = new DirectoryInfo(Path.Combine(root.Parent.FullName, "folder1"));
var otherSiblingDir = new DirectoryInfo(Path.Combine(root.Parent.FullName, "folder2"));
var siblingRelative = siblingDir.GetRelativePath(otherSiblingDir);
// siblingRelative == Path.Combine("..", "folder1")
```

## FileInfoExtensions class

| Method Name | Parameters | Description |
| --- | --- | --- |
| GetRelativeFileName | directoryInfo | Returns this file's name, prefixed with its relative directory path (if any), relative to `directoryInfo` |
| GetRelativeFilePath | directoryInfo | Returns this file's containing directory, as a path relative to `directoryInfo` |
| GetFriendlyFileSize | | (Instance, on `FileInfo`) Returns `GetFriendlyFileSize(fileInfo.Length)`, or `"0B"` if `fileInfo` is null |
| GetFriendlyFileSize | fileSize (long) | (Static) Formats a byte count as a human-friendly string (`B`, `KB`, `MB`, `GB`, `TB`, `PB`, `EB`), rounded to 1 decimal place |

Examples:

```csharp
// GetRelativeFileName / GetRelativeFilePath
var dir = new DirectoryInfo(Path.Combine("Temp", "abcdefg"));
var file = new FileInfo(Path.Combine("Temp", "abcdefg", "dir3", "file1.tf"));

var relativeFileName = file.GetRelativeFileName(dir);
// relativeFileName == Path.Combine("dir3", "file1.tf")

var relativeFilePath = file.GetRelativeFilePath(dir);
// relativeFilePath == "dir3"

var sameDirFile = new FileInfo(Path.Combine("Temp", "abcdefg", "file.txt"));
var sameDirFileName = sameDirFile.GetRelativeFileName(dir);
// sameDirFileName == "file.txt"

// GetFriendlyFileSize
var size0 = FileInfoExtensions.GetFriendlyFileSize(0);        // "0B"
var size1023 = FileInfoExtensions.GetFriendlyFileSize(1023);  // "1023B"
var size1024 = FileInfoExtensions.GetFriendlyFileSize(1024);  // "1KB"
var size1536 = FileInfoExtensions.GetFriendlyFileSize(1536);  // "1.5KB"
var size1MB = FileInfoExtensions.GetFriendlyFileSize(1048576); // "1MB"

FileInfo nullFile = null;
var nullFileSize = nullFile.GetFriendlyFileSize(); // "0B"
```

## PathExtensions class

| Method Name | Parameters | Description |
| --- | --- | --- |
| CombineWith | paths (params string[]) | Combines `path` with the supplied additional path segments, equivalent to `Path.Combine(path, ...paths)`. A null `paths` array is treated the same as supplying none. Throws `ArgumentNullException` if `path` is null |

Examples:

```csharp
var combined = "base".CombineWith("sub", "file.txt");
// combined == Path.Combine("base", "sub", "file.txt")

var unchanged = "base".CombineWith();
// unchanged == "base"

var unchangedWithNullArray = "base".CombineWith(null);
// unchangedWithNullArray == "base" (null paths array treated as none supplied)
```
