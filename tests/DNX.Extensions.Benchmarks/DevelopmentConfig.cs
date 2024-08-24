using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;

namespace DNX.Extensions.Benchmarks;
internal class DevelopmentConfig : ManualConfig
{
    public DevelopmentConfig()
    {
        AddExporter(DefaultConfig.Instance.GetExporters().ToArray());
        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
        AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());

        AddExporter(new HtmlExporter());

        WithOptions(ConfigOptions.DisableOptimizationsValidator);
    }
}
