using DNX.Extensions.Linq;

namespace DNX.Extensions.Tests.Configuration;
public class EnvironmentConfig
{
    public static bool IsLinuxStyleFileSystem => Environment.OSVersion.Platform.IsOneOf(PlatformID.Unix, PlatformID.MacOSX);

    public static bool IsWindowsStyleFileSystem => Environment.OSVersion.Platform.ToString().StartsWith("Win");
}
