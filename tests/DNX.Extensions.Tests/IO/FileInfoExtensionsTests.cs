using DNX.Extensions.IO;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace DNX.Extensions.Tests.IO
{
    public class FileInfoExtensionsTests(ITestOutputHelper outputHelper)
    {
        [Theory]
        [MemberData(nameof(GetRelativeFileName_Data))]
        public void GetRelativeFileName_can_extract_relative_filename_correctly(string fileName, string dirName, string expected)
        {
            outputHelper.WriteLine($"Checking FileName: {fileName} -> {dirName}");

            var fileInfo = new FileInfo(fileName);
            var dirInfo = new DirectoryInfo(dirName);

            var result = fileInfo.GetRelativeFileName(dirInfo);

            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GetRelativeFilePath_Data))]
        public void GetRelativeFilePath_can_extract_relative_path_correctly(string fileName, string dirName, string expected)
        {
            outputHelper.WriteLine($"Checking FileName: {fileName} -> {dirName}");

            var fileInfo = new FileInfo(fileName);
            var dirInfo = new DirectoryInfo(dirName);

            var result = fileInfo.GetRelativeFilePath(dirInfo);

            result.Should().Be(expected);
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

        public static IEnumerable<object[]> FileSizeData()
        {
            var data = new List<object[]>
            {
                new object[] { 0, "0B" },
                new object[] { 1000, "1000B" },
                new object[] { 1023, "1023B" },
                new object[] { 1024, "1KB" },
                new object[] { 1536, "1.5KB" },
                new object[] { 1792, "1.8KB" },
                new object[] { 2048, "2KB" },
                new object[] { 10240, "10KB" },
                new object[] { 102400, "100KB" },
                new object[] { 1024000, "1000KB" },
                new object[] { 1048500, "1023.9KB" },
                new object[] { 1048575, "1024KB" },
                new object[] { 1048576, "1MB" },
                new object[] { 2097152, "2MB" },
                new object[] { 10485760, "10MB" },
            };

            return data;
        }

        public static IEnumerable<object[]> GetRelativeFileName_Data()
        {
            return new List<object[]>()
            {
                new object[] { Path.Combine("C:", "Temp", "abcdefg", "file.txt"), Path.Combine("C:", "Temp", "abcdefg"), "file.txt" },
                new object[] { Path.Combine("C:", "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine("C:", "Temp", "abcdefg"), Path.Combine("dir3", "file1.tf") },
                new object[] { Path.Combine("C:", "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine("C:", "Temp", "abcdefg", "dir3"), "file1.tf" },
                new object[] { Path.Combine("C:", "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine("C:", "Temp", "abcdefg", "dir3"), "file1.tf" },
                new object[] { Path.Combine("C:", "Temp", "folder1", "file.txt"), Path.Combine("D:", "folder2"), Path.Combine("C:", "Temp", "folder1", "file.txt") },
                new object[] { Path.Combine("C:", "Temp", "folder1", "file.txt"), Path.Combine("C:", "Temp", "folder2"), Path.Combine("..", "folder1", "file.txt") },
            };
        }

        public static IEnumerable<object[]> GetRelativeFilePath_Data()
        {
            return new List<object[]>()
            {
                new object[] { Path.Combine("C:", "Temp", "abcdefg", "file.txt"), Path.Combine("C:", "Temp", "abcdefg"), "" },
                new object[] { Path.Combine("C:", "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine("C:", "Temp", "abcdefg"), "dir3" },
                new object[] { Path.Combine("C:", "Temp", "abcdefg", "dir3", "file1.tf"), Path.Combine("C:", "Temp", "abcdefg", "dir3"), "" },
                new object[] { Path.Combine("C:", "Temp", "folder1", "file.txt"), Path.Combine("C:", "Temp", "folder2"), Path.Combine("..", "folder1") },
                new object[] { Path.Combine("C:", "Temp", "folder1", "file.txt"), Path.Combine("D:", "folder2"), Path.Combine("C:", "Temp", "folder1") },
            };
        }
    }
}
