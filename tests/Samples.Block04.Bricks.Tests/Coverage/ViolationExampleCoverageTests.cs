using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NMolecules.Bricks;
using Samples.Block04.Bricks.FunctionCoverage;
using Xunit;

namespace BrickExamples.Tests.Coverage;

/// <summary>
/// Verifies that every public Bricks API family and enum variation family has a
/// clean violation example, even when the behavior is runtime-backed rather
/// than analyzer-backed.
/// </summary>
public sealed class ViolationExampleCoverageTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void CatalogCoversEveryPublicApiFamilyWithViolationExample()
    {
        var expected = LoadApiMatrix().Areas.Select(area => area.Id).ToHashSet(StringComparer.Ordinal);
        var actual = ViolationExamples.All
            .SelectMany(example => example.ApiFamilyIds)
            .ToHashSet(StringComparer.Ordinal);

        var missing = expected.Except(actual, StringComparer.Ordinal).OrderBy(id => id, StringComparer.Ordinal).ToArray();
        var unexpected = actual.Except(expected, StringComparer.Ordinal).OrderBy(id => id, StringComparer.Ordinal).ToArray();

        Assert.True(missing.Length == 0, $"Missing API-family violation examples: {string.Join(", ", missing)}");
        Assert.True(unexpected.Length == 0, $"Unexpected API-family violation examples: {string.Join(", ", unexpected)}");
    }

    [Fact]
    public void CatalogCoversEveryEnumVariationFamilyWithViolationExample()
    {
        var expected = LoadEnumMatrix().Families.Select(family => family.Id).ToHashSet(StringComparer.Ordinal);
        var actual = ViolationExamples.All
            .Select(example => example.EnumVariationFamilyId)
            .Where(id => id is not null)
            .Select(id => id!)
            .ToHashSet(StringComparer.Ordinal);

        var missing = expected.Except(actual, StringComparer.Ordinal).OrderBy(id => id, StringComparer.Ordinal).ToArray();
        var unexpected = actual.Except(expected, StringComparer.Ordinal).OrderBy(id => id, StringComparer.Ordinal).ToArray();

        Assert.True(missing.Length == 0, $"Missing enum-family violation examples: {string.Join(", ", missing)}");
        Assert.True(unexpected.Length == 0, $"Unexpected enum-family violation examples: {string.Join(", ", unexpected)}");
    }

    [Fact]
    public void CatalogCoversEveryPublicEnumValueInViolationContext()
    {
        var expected = GetPublicBricksEnumValues();
        var actual = ViolationExamples.All
            .SelectMany(example => example.EnumValues)
            .SelectMany(pair => pair.Value.Select(valueName => (EnumName: pair.Key, ValueName: valueName)))
            .ToHashSet();

        var missing = expected.Except(actual).OrderBy(value => value.EnumName).ThenBy(value => value.ValueName).ToArray();
        var unexpected = actual.Except(expected).OrderBy(value => value.EnumName).ThenBy(value => value.ValueName).ToArray();

        Assert.True(missing.Length == 0, $"Missing enum values in violation examples: {Format(missing)}");
        Assert.True(unexpected.Length == 0, $"Unexpected enum values in violation examples: {Format(unexpected)}");
    }

    [Fact]
    public void CatalogUsesConcreteViolationsAndExistingEvidence()
    {
        var examplesRoot = FindExamplesRoot();
        var failures = new List<string>();

        foreach (var example in ViolationExamples.All)
        {
            if (string.IsNullOrWhiteSpace(example.Id))
            {
                failures.Add("Violation example has an empty id.");
            }

            if (string.IsNullOrWhiteSpace(example.Title))
            {
                failures.Add($"{example.Id} has an empty title.");
            }

            if (example.ApiFamilyIds.Count == 0)
            {
                failures.Add($"{example.Id} has no API-family id.");
            }

            if (example.Violation.Source is null)
            {
                failures.Add($"{example.Id} has no violation source.");
            }

            if (string.IsNullOrWhiteSpace(example.Violation.Message))
            {
                failures.Add($"{example.Id} has an empty violation message.");
            }

            if (example.Violation.RuleId is null || example.Violation.RuleId.Value.IsEmpty)
            {
                failures.Add($"{example.Id} has no stable rule id.");
            }

            var evidencePath = Path.Combine(examplesRoot, example.EvidencePath.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(evidencePath) && !Directory.Exists(evidencePath))
            {
                failures.Add($"{example.Id} points to missing evidence path {example.EvidencePath}.");
            }
        }

        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    private static ApiFamilyCoverageMatrix LoadApiMatrix()
    {
        var matrixPath = Path.Combine(FindExamplesRoot(), "coverage", "bricks-api-family-coverage.json");
        using var stream = File.OpenRead(matrixPath);
        return JsonSerializer.Deserialize<ApiFamilyCoverageMatrix>(stream, JsonOptions)
            ?? throw new InvalidOperationException("Bricks API family coverage matrix could not be deserialized.");
    }

    private static EnumVariationMatrix LoadEnumMatrix()
    {
        var matrixPath = Path.Combine(FindExamplesRoot(), "coverage", "bricks-enum-variation-matrix.json");
        using var stream = File.OpenRead(matrixPath);
        return JsonSerializer.Deserialize<EnumVariationMatrix>(stream, JsonOptions)
            ?? throw new InvalidOperationException("Bricks enum variation matrix could not be deserialized.");
    }

    private static HashSet<(string EnumName, string ValueName)> GetPublicBricksEnumValues()
    {
        return typeof(BrickElementKind)
            .Assembly
            .GetTypes()
            .Where(type => type.IsEnum && type.IsPublic && type.Namespace == "NMolecules.Bricks")
            .SelectMany(type => Enum.GetNames(type).Select(valueName => (type.Name, valueName)))
            .ToHashSet();
    }

    private static string FindExamplesRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var matrixPath = Path.Combine(directory.FullName, "coverage", "bricks-api-family-coverage.json");
            if (File.Exists(matrixPath))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("Could not find nmolecules.brick-examples root.");
    }

    private static string Format(IEnumerable<(string EnumName, string ValueName)> values)
    {
        return string.Join(", ", values.Select(value => $"{value.EnumName}.{value.ValueName}"));
    }

    private sealed class ApiFamilyCoverageMatrix
    {
        public List<ApiFamilyCoverageArea> Areas { get; set; } = new();
    }

    private sealed class ApiFamilyCoverageArea
    {
        public string Id { get; set; } = string.Empty;
    }

    private sealed class EnumVariationMatrix
    {
        public List<EnumVariationFamily> Families { get; set; } = new();
    }

    private sealed class EnumVariationFamily
    {
        public string Id { get; set; } = string.Empty;
    }
}
