using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using DNX.Extensions.Strings;

// ReSharper disable UseStringInterpolation

namespace DNX.Extensions.Benchmarks.Strings;

[ExcludeFromCodeCoverage]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class StringExtensionBenchmarks
{
    private const string SearchText = nameof(SearchText);

    private string _text1 = "";
    private string _text2 = "";

    [GlobalSetup]
    public void GlobalSetup()
    {
        const int padSize = 50;

        _text1 = string.Format(
            "{0}{1}{2}",
            new string('-', padSize),
            SearchText,
            new string('-', padSize)
        );

        _text2 = SearchText;
        for (var x = 0; x < padSize; ++x)
            _text2 = "-+" + _text2 + "*-";
    }

    [Benchmark]
    public string Before() => _text1.Before(SearchText);

    [Benchmark]
    public string BeforeLast() => _text2.BeforeLast("+");

    [Benchmark]
    public string After() => _text1.After(SearchText);

    [Benchmark]
    public string AfterLast() => _text2.AfterLast("+");

    [Benchmark]
    public string Reverse() => _text1.Reverse();

    [Benchmark]
    public string EnsureStartsWith() => _text1.EnsureStartsWith("--");

    [Benchmark]
    public string EnsureEndsWith() => _text1.EnsureEndsWith("--");

    [Benchmark]
    public string EnsureStartsAndEndsWith() => _text1.EnsureStartsAndEndsWith("--");
}
