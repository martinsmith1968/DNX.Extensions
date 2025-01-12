using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DNX.Extensions.Generators.Extensions;
using Microsoft.CodeAnalysis;

namespace DNX.Extensions.Generators.Generators;

[ExcludeFromCodeCoverage]
[Generator]
public class ConversionGenerator : ISourceGenerator
{
    private static readonly Dictionary<string, string> TypesForGeneration = new()
    {
        { "short", nameof(Int16) },
        { "int", nameof(Int32) },
        { "long", nameof(Int64) },
        { "bool", nameof(Boolean) },
    };

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        context.ReportDiagnostic(
            Diagnostic.Create(
                new DiagnosticDescriptor(
                    "DNX0001",
                    $"{nameof(ConversionGenerator)} Executing",
                    nameof(ConversionGenerator) + " Executing for {0} types",
                    "Build",
                    DiagnosticSeverity.Info,
                    true
                    ),
                Location.None,
                TypesForGeneration.Count.ToString()
                )
            );

        var targetNamespace = GetType().Name.Replace("Generator", "");

        var nameSpace = GetType().Namespace.Replace(".Generators", "")
            .Trim('.')
            + $".{targetNamespace}";

        var templateFileName = $"{GetType().Name}.Template.txt";

        var templateText = this.GetEmbeddedResourceText(templateFileName);

        if (string.IsNullOrWhiteSpace(templateText))
            throw new Exception($"{nameof(templateText)} is empty");

        foreach (var kvp in TypesForGeneration)
        {
            var typeName = kvp.Key;
            var typeDescription = kvp.Value;

            context.ReportDiagnostic(
                Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "DNX0002",
                        $"{nameof(ConversionGenerator)} Generating: {typeName}",
                        nameof(ConversionGenerator) + " Generating: {0} as {1}, under {2}",
                        "",
                        DiagnosticSeverity.Info,
                        true
                        ),
                    Location.None,
                    typeName,
                    typeDescription,
                    nameSpace
                )
            );

            var sourceText = templateText
                    .Replace("#namespace#", nameSpace)
                    .Replace("#type#", typeName)
                    .Replace("#name#", typeDescription)
                ;

            context.AddSource($"Convert{kvp.Value}.generated.cs", sourceText);
        }
    }
}
