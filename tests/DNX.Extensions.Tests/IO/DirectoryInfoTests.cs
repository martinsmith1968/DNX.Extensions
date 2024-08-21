using DNX.Extensions.IO;
using DNX.Extensions.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable StringLiteralTypo

namespace DNX.Extensions.Tests.IO
{
    public class DirectoryInfoExtensionsTests : IDisposable
    {
        private readonly DirectoryInfo _directoryInfo;
        private readonly ITestOutputHelper _outputHelper;

        public DirectoryInfoExtensionsTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;

            var directoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _directoryInfo = new DirectoryInfo(directoryPath);
            _directoryInfo.Create();

            SetupStandardFileStructure(_directoryInfo);
        }

        public void Dispose()
        {
            _directoryInfo.Delete(true);
            GC.SuppressFinalize(this);
        }

        internal static void CreateFile(DirectoryInfo directoryInfo, string fileName)
        {
            var filePath = Path.Combine(directoryInfo.FullName, fileName);

            File.WriteAllText(filePath, string.Empty);
        }

        internal static void SetupStandardFileStructure(DirectoryInfo directoryInfo)
        {
            var dir1 = directoryInfo.CreateSubdirectory("dir1");
            var dir2 = directoryInfo.CreateSubdirectory("dir2");
            var dir3 = dir1.CreateSubdirectory("dur3");
            var dir4 = dir2.CreateSubdirectory("dur4");

            CreateFile(directoryInfo, "file.txt");
            CreateFile(directoryInfo, "file.json");
            CreateFile(dir1, "file1.txt");
            CreateFile(dir2, "file2.txt");
            CreateFile(dir1, "file1.json");
            CreateFile(dir2, "file2.json");
            CreateFile(dir3, "file1.tf");
            CreateFile(dir4, "file2.tf");
        }

        [Fact]
        public void FindFiles_for_directory_that_does_not_exist_finds_expected_files()
        {
            // Arrange
            const string pattern = "*.txt";
            var directoryInfo = new DirectoryInfo(Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString()));

            // Act
            var result = directoryInfo.FindFiles(pattern, false);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(0);
        }

        [Fact]
        public void FindFiles_for_single_pattern_without_recursion_finds_expected_files()
        {
            // Arrange
            const string pattern = "*.txt";

            // Act
            var result = _directoryInfo.FindFiles(pattern, false);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            result.Count(x => x.Name == "file.txt").Should().Be(1);
        }

        [Fact]
        public void FindFiles_for_single_pattern_with_recursion_finds_expected_files()
        {
            // Arrange
            const string pattern = "*.txt";

            // Act
            var result = _directoryInfo.FindFiles(pattern, true);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(3);
            result.Count(x => x.Name == "file.txt").Should().Be(1);
            result.Count(x => x.Name == "file1.txt").Should().Be(1);
            result.Count(x => x.Name == "file2.txt").Should().Be(1);
        }

        [Fact]
        public void FindFiles_for_multiple_patterns_without_recursion_finds_expected_files()
        {
            // Arrange
            var patterns = new[] { "*.txt", "*.json" };
            SetupStandardFileStructure(_directoryInfo);

            // Act
            var result = _directoryInfo.FindFiles(patterns, false);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.Count(x => x.Name == "file.txt").Should().Be(1);
            result.Count(x => x.Name == "file.json").Should().Be(1);
        }

        [Fact]
        public void FindFiles_for_multiple_patterns_with_recursion_finds_expected_files()
        {
            // Arrange
            var patterns = new[] { "*.txt", "*.json" };
            SetupStandardFileStructure(_directoryInfo);

            // Act
            var result = _directoryInfo.FindFiles(patterns, true);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(6);
            result.Count(x => x.Name == "file.txt").Should().Be(1);
            result.Count(x => x.Name == "file1.txt").Should().Be(1);
            result.Count(x => x.Name == "file2.txt").Should().Be(1);
            result.Count(x => x.Name == "file.json").Should().Be(1);
            result.Count(x => x.Name == "file1.json").Should().Be(1);
            result.Count(x => x.Name == "file2.json").Should().Be(1);
        }

        [Fact]
        public void FindDirectories_for_directory_that_does_not_exist_finds_expected_files()
        {
            // Arrange
            const string pattern = "dir*";
            var directoryInfo = new DirectoryInfo(Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString()));

            // Act
            var result = directoryInfo.FindDirectories(pattern, false);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(0);
        }

        [Fact]
        public void FindDirectories_for_single_pattern_without_recursion_finds_expected_files()
        {
            // Arrange
            const string pattern = "dir*";

            // Act
            var result = _directoryInfo.FindDirectories(pattern, false);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.Count(x => x.Name == "dir1").Should().Be(1);
            result.Count(x => x.Name == "dir2").Should().Be(1);
        }

        [Fact]
        public void FindDirectories_for_single_pattern_with_recursion_finds_expected_files()
        {
            // Arrange
            const string pattern = "d?r*";

            // Act
            var result = _directoryInfo.FindDirectories(pattern, true);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(4);
            result.Count(x => x.Name == "dir1").Should().Be(1);
            result.Count(x => x.Name == "dir2").Should().Be(1);
            result.Count(x => x.Name == "dur3").Should().Be(1);
            result.Count(x => x.Name == "dur4").Should().Be(1);
        }

        [Fact]
        public void FindDirectories_for_multiple_patterns_without_recursion_finds_expected_files()
        {
            // Arrange
            var patterns = new[] { "dir*", "dur*" };

            // Act
            var result = _directoryInfo.FindDirectories(patterns, false);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.Count(x => x.Name == "dir1").Should().Be(1);
            result.Count(x => x.Name == "dir2").Should().Be(1);
        }

        [Fact]
        public void FindDirectories_for_multiple_patterns_with_recursion_finds_expected_files()
        {
            // Arrange
            var patterns = new[] { "dir*", "dur*" };

            // Act
            var result = _directoryInfo.FindDirectories(patterns, true);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(4);
            result.Count(x => x.Name == "dir1").Should().Be(1);
            result.Count(x => x.Name == "dir2").Should().Be(1);
            result.Count(x => x.Name == "dur3").Should().Be(1);
            result.Count(x => x.Name == "dur4").Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(GetRelativePath_Data))]
        public void GetRelativePath_can_extract_relative_path_correctly(string dirName, string relativeToDirName, string expected)
        {
            _outputHelper.WriteLine($"{Environment.OSVersion.Platform} - Checking DirName: {dirName} -> {relativeToDirName}");

            var dirInfo = dirName == null ? null : new DirectoryInfo(dirName);
            var relativeToDirInfo = relativeToDirName == null ? null : new DirectoryInfo(relativeToDirName);

            var result = dirInfo.GetRelativePath(relativeToDirInfo);

            result.Should().Be(expected, $"{nameof(dirName)}: {dirName} - {nameof(relativeToDirInfo)}: {relativeToDirInfo}");
        }

        public static TheoryData<string, string, string> GetRelativePath_Data()
        {
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();
            var guid3 = Guid.NewGuid().ToString();

            var data = new TheoryData<string, string, string>()
            {
                { Path.Combine(Path.GetTempPath(), guid1), null, null },
                { null, Path.Combine(Path.GetTempPath(), guid1), null },
                { Path.Combine(Path.GetTempPath(), guid1), Path.Combine(Path.GetTempPath(), "abcdefg"), Path.Join("..", guid1) },
                { Path.Combine(Path.GetTempPath(), guid2), Path.Combine(Path.GetTempPath(), guid3), Path.Join("..", guid2) },
                { Path.Combine(Path.GetTempPath(), "abcdefg"), Path.Combine(Path.GetTempPath(), "abcdefg"), "" },
                { Path.Combine(Path.GetTempPath(), "abcdefg", "dir3"), Path.Combine(Path.GetTempPath(), "abcdefg"), "dir3" },
                { Path.Combine(Path.GetTempPath(), "abcdefg", "dir3"), Path.Combine(Path.GetTempPath(), "abcdefg", "dir3"), "" },
                { Path.Combine(Path.GetTempPath(), "folder1"), Path.Combine(Path.GetTempPath(), "folder2"), Path.Combine("..", "folder1") },
            };

            if (Configuration.EnvironmentConfig.IsWindowsStyleFileSystem)
            {
                data.Add(Path.Combine(Path.GetTempPath(), "folder1"), Path.Combine("D:", "folder2"), Path.Combine(Path.GetTempPath(), "folder1"));
            }
            else if (Configuration.EnvironmentConfig.IsLinuxStyleFileSystem)
            {
                data.Add(Path.Combine(Path.GetTempPath(), "folder1"), Path.Combine("/etc", "folder2"), Path.Combine("..", "..", Path.GetTempPath(), "folder1"));
            }

            return data;
        }
    }
}
