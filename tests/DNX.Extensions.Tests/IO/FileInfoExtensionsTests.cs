using DNX.Extensions.IO;
using DNX.Extensions.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace DNX.Extensions.Tests.IO
{
    public class FileInfoExtensionsTests(ITestOutputHelper outputHelper)
    {
        private static string DriveRoot1
        {
            get
            {
                return Environment.OSVersion.Platform.IsOneOf(PlatformID.Unix, PlatformID.MacOSX)
                    ? "/root1"
                    : "C:";
            }
        }

        private static string DriveRoot2
        {
            get
            {
                return Environment.OSVersion.Platform.IsOneOf(PlatformID.Unix, PlatformID.MacOSX)
                    ? "/root2"
                    : "D:";
            }
        }

        [Theory]
        [MemberData(nameof(GetRelativeFileName_Data))]
        public void GetRelativeFileName_can_extract_relative_filename_correctly(string fileName, string dirName, string expected)
        {
            outputHelper.WriteLine($"Checking FileName: {fileName} -> {dirName}");

            var fileInfo = new FileInfo(fileName);
            var dirInfo = new DirectoryInfo(dirName);

            var result = fileInfo.GetRelativeFileName(dirInfo);

            result.Should().Be(expected, $"{nameof(dirName)}: {dirName} - {nameof(fileName)}: {fileName}");
        }

        [Theory]
        [MemberData(nameof(GetRelativeFilePath_Data))]
        public void GetRelativeFilePath_can_extract_relative_path_correctly(string fileName, string dirName, string expected)
        {
            outputHelper.WriteLine($"Checking FileName: {fileName} -> {dirName}");

            var fileInfo = new FileInfo(fileName);
            var dirInfo = new DirectoryInfo(dirName);

            var result = fileInfo.GetRelativeFilePath(dirInfo);

            result.Should().Be(expected, $"{nameof(dirName)}: {dirName} - {nameof(fileName)}: {fileName}");
        }

        [Theory]
        [MemberData(nameof(FileSizeData))]
        public void GetFriendlyFileSize_given_a_fileSize_should_return_expected_text(long fileSize, string expected)
        {
            var result = FileInfoExtensions.GetFriendlyFileSize(fileSize);

            result.Should().Be(expected);
        }

        [Fact]
        public void GetFriendlyFileSize_given_an_invalid_FileInfo_should_return_expected_text()
        {
            // Arrange
            var fileInfo = (FileInfo)null;

            // Act
            var result = FileInfoExtensions.GetFriendlyFileSize(fileInfo);

            // Assert
            result.Should().Be("0B");
        }

        [Theory]
        [MemberData(nameof(FileSizeData))]
        public void GetFriendlyFileSize_given_a_valid_FileInfo_should_return_expected_text(long fileSize, string expected)
        {
            var fileName = Path.GetTempFileName();
            var fileInfo = new FileInfo(fileName);

            var data = new byte[fileSize];

            File.WriteAllBytes(fileInfo.FullName, data);

            // Act
            var result = FileInfoExtensions.GetFriendlyFileSize(fileInfo);

            // Assert
            result.Should().Be(expected);

            // Cleanup
            fileInfo.Delete();
        }

        public static TheoryData<long, string> FileSizeData()
        {
            return new TheoryData<long, string>
            {
                { 0, "0B" },
                { 1000, "1000B" },
                { 1023, "1023B" },
                { 1024, "1KB" },
                { 1536, "1.5KB" },
                { 1792, "1.8KB" },
                { 2048, "2KB" },
                { 10240, "10KB" },
                { 102400, "100KB" },
                { 1024000, "1000KB" },
                { 1048500, "1023.9KB" },
                { 1048575, "1024KB" },
                { 1048576, "1MB" },
                { 2097152, "2MB" },
                { 10485760, "10MB" },
            };
        }

        public static TheoryData<string, string, string> GetRelativeFileName_Data()
        {
            return new TheoryData<string, string, string>
            {
                { Path.Combine(DriveRoot1, "Temp", "abcdefg", "file.txt"), Path.Combine(DriveRoot1, "Temp", "abcdefg"), "file.txt" },
                { Path.Combine(DriveRoot1, "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine(DriveRoot1, "Temp", "abcdefg"), Path.Combine("dir3", "file1.tf") },
                { Path.Combine(DriveRoot1, "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine(DriveRoot1, "Temp", "abcdefg", "dir3"), "file1.tf" },
                { Path.Combine(DriveRoot1, "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine(DriveRoot1, "Temp", "abcdefg", "dir3"), "file1.tf" },
                { Path.Combine(DriveRoot1, "Temp", "folder1", "file.txt"), Path.Combine(DriveRoot2, "folder2"), Path.Combine(DriveRoot1, "Temp", "folder1", "file.txt") },
                { Path.Combine(DriveRoot1, "Temp", "folder1", "file.txt"), Path.Combine(DriveRoot1, "Temp", "folder2"), Path.Combine("..", "folder1", "file.txt") },
            };
        }

        public static TheoryData<string, string, string> GetRelativeFilePath_Data()
        {
            return new TheoryData<string, string, string>
            {
                { Path.Combine(DriveRoot1, "Temp", "abcdefg", "file.txt"), Path.Combine(DriveRoot1, "Temp", "abcdefg"), "" },
                { Path.Combine(DriveRoot1, "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine(DriveRoot1, "Temp", "abcdefg"), "dir3" },
                { Path.Combine(DriveRoot1, "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine(DriveRoot1, "Temp", "abcdefg", "dir3"), "" },
                { Path.Combine(DriveRoot1, "Temp", "folder1", "file.txt"), Path.Combine(DriveRoot1, "Temp", "folder2"), Path.Combine("..", "folder1") },
                { Path.Combine(DriveRoot1, "Temp", "folder1", "file.txt"), Path.Combine(DriveRoot2, "folder2"), Path.Combine(DriveRoot1, "Temp", "folder1") },
            };
        }
    }
}
