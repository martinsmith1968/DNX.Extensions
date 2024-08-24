using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using DNX.Extensions.Strings;

namespace DNX.Extensions.Benchmarks.Strings;

[ExcludeFromCodeCoverage]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class StringExtensionBenchmarks
{
    private const string SearchText = nameof(SearchText);

    private string _text = "";

    [Params(100)]
    public int StartTextSize { get; set; }

    [Params(100)]
    public int EndTextSize { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _text = string.Format(
            "{0}{1}{2}",
            new string('-', StartTextSize),
            SearchText,
            new string('-', EndTextSize)
        );
    }

    [Benchmark]
    public string Before() => _text.Before(SearchText);

    [Benchmark]
    public string After() => _text.After(SearchText);

    [Benchmark]
    public string Reverse() => _text.Reverse();
}
