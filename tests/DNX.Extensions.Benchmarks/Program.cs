using System.Reflection;
using BenchmarkDotNet.Running;

namespace DNX.Extensions.Benchmarks;

public class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run(
            Assembly.GetExecutingAssembly(),
#if DEBUG
            new DevelopmentConfig()
#else
            null
#endif
            );
    }
}
