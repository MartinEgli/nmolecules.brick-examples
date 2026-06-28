using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NMolecules.Bricks;
using Xunit;

namespace BrickExamples.Tests.EnumCoverageCases;

/// <summary>
/// Verifies that meaningful Bricks enum variation families stay complete and
/// connected to executable sample evidence.
/// </summary>
public sealed class EnumVariationMatrixTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void MatrixCoversEveryPublicBricksEnumValue()
    {
        var expected = GetPublicBricksEnumValues();
        var actual = LoadMatrix()
            .Families
            .SelectMany(family => family.Rows)
            .SelectMany(row => row.Values)
            .SelectMany(pair => pair.Value.Select(valueName => (EnumName: pair.Key, ValueName: valueName)))
            .ToHashSet();

        var missing = expected.Except(actual).OrderBy(value => value.EnumName).ThenBy(value => value.ValueName).ToArray();
        var unexpected = actual.Except(expected).OrderBy(value => value.EnumName).ThenBy(value => value.ValueName).ToArray();

        Assert.True(missing.Length == 0, $"Missing enum variation values: {Format(missing)}");
        Assert.True(unexpected.Length == 0, $"Unexpected enum variation values: {Format(unexpected)}");
    }

    [Fact]
    public void MatrixReferencesOnlyExistingEnumsAndValues()
    {
        var publicEnums = GetPublicBricksEnums();
        var matrix = LoadMatrix();
        var failures = new List<string>();

        foreach (var family in matrix.Families)
        {
            foreach (var enumName in family.Enums)
            {
                if (!publicEnums.ContainsKey(enumName))
                {
                    failures.Add($"{family.Id} references unknown enum {enumName}.");
                }
            }

            foreach (var row in family.Rows)
            {
                foreach (var (enumName, valueNames) in row.Values)
                {
                    if (!publicEnums.TryGetValue(enumName, out var knownValues))
                    {
                        failures.Add($"{family.Id}/{row.Id} references unknown enum {enumName}.");
                        continue;
                    }

                    if (!family.Enums.Contains(enumName, StringComparer.Ordinal))
                    {
                        failures.Add($"{family.Id}/{row.Id} uses {enumName} without declaring it in the family enum list.");
                    }

                    foreach (var valueName in valueNames)
                    {
                        if (!knownValues.Contains(valueName))
                        {
                            failures.Add($"{family.Id}/{row.Id} references unknown enum value {enumName}.{valueName}.");
                        }
                    }
                }
            }
        }

        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    [Fact]
    public void MatrixRowsHaveStableEvidenceAndExistingSamplePaths()
    {
        var examplesRoot = FindExamplesRoot();
        var matrix = LoadMatrix();
        var failures = new List<string>();
        var supportedFamilyKinds = new[] { "analyzer-backed", "runtime-backed", "sample-backed", "documentation-backed" };
        var supportedOutcomes = new[] { "pass", "violation", "report", "documentation" };

        if (matrix.SchemaVersion != 1)
        {
            failures.Add($"Unsupported schema version {matrix.SchemaVersion}.");
        }

        if (!string.Equals(matrix.Status, "active", StringComparison.Ordinal))
        {
            failures.Add($"Unsupported matrix status '{matrix.Status}'.");
        }

        AddDuplicateFailures("family", matrix.Families.Select(family => family.Id), failures);

        foreach (var family in matrix.Families)
        {
            if (!supportedFamilyKinds.Contains(family.Support, StringComparer.Ordinal))
            {
                failures.Add($"{family.Id} has unsupported support kind '{family.Support}'.");
            }

            AddDuplicateFailures($"{family.Id} sample", family.Samples.Select(sample => sample.Id), failures);
            AddDuplicateFailures($"{family.Id} row", family.Rows.Select(row => row.Id), failures);

            var sampleIds = family.Samples.Select(sample => sample.Id).ToHashSet(StringComparer.Ordinal);
            foreach (var sample in family.Samples)
            {
                var samplePath = Path.Combine(examplesRoot, sample.Path.Replace('/', Path.DirectorySeparatorChar));
                if (!File.Exists(samplePath) && !Directory.Exists(samplePath))
                {
                    failures.Add($"{family.Id}/{sample.Id} points to missing sample path {sample.Path}.");
                }
            }

            foreach (var enumName in family.Enums)
            {
                if (!family.Rows.Any(row => row.Values.ContainsKey(enumName)))
                {
                    failures.Add($"{family.Id} declares {enumName} but no row uses it.");
                }
            }

            foreach (var row in family.Rows)
            {
                if (!supportedOutcomes.Contains(row.Outcome, StringComparer.Ordinal))
                {
                    failures.Add($"{family.Id}/{row.Id} has unsupported outcome '{row.Outcome}'.");
                }

                if (!sampleIds.Contains(row.Evidence))
                {
                    failures.Add($"{family.Id}/{row.Id} references missing evidence sample '{row.Evidence}'.");
                }

                if (row.Values.Count == 0)
                {
                    failures.Add($"{family.Id}/{row.Id} does not reference any enum values.");
                }
            }
        }

        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    private static VariationMatrix LoadMatrix()
    {
        var matrixPath = Path.Combine(FindExamplesRoot(), "coverage", "bricks-enum-variation-matrix.json");
        using var stream = File.OpenRead(matrixPath);
        return JsonSerializer.Deserialize<VariationMatrix>(stream, JsonOptions)
            ?? throw new InvalidOperationException("Bricks enum variation matrix could not be deserialized.");
    }

    private static string FindExamplesRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var matrixPath = Path.Combine(directory.FullName, "coverage", "bricks-enum-variation-matrix.json");
            if (File.Exists(matrixPath))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("Could not find nmolecules.brick-examples root.");
    }

    private static Dictionary<string, HashSet<string>> GetPublicBricksEnums()
    {
        return typeof(BrickElementKind)
            .Assembly
            .GetTypes()
            .Where(type => type.IsEnum && type.IsPublic && type.Namespace == "NMolecules.Bricks")
            .ToDictionary(type => type.Name, type => Enum.GetNames(type).ToHashSet(StringComparer.Ordinal), StringComparer.Ordinal);
    }

    private static HashSet<(string EnumName, string ValueName)> GetPublicBricksEnumValues()
    {
        return GetPublicBricksEnums()
            .SelectMany(pair => pair.Value.Select(valueName => (EnumName: pair.Key, ValueName: valueName)))
            .ToHashSet();
    }

    private static void AddDuplicateFailures(string name, IEnumerable<string> ids, ICollection<string> failures)
    {
        var duplicates = ids
            .GroupBy(id => id, StringComparer.Ordinal)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToArray();

        foreach (var duplicate in duplicates)
        {
            failures.Add($"Duplicate {name} id '{duplicate}'.");
        }
    }

    private static string Format(IEnumerable<(string EnumName, string ValueName)> values)
    {
        return string.Join(", ", values.Select(value => $"{value.EnumName}.{value.ValueName}"));
    }

    private sealed class VariationMatrix
    {
        public int SchemaVersion { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<VariationFamily> Families { get; set; } = new();
    }

    private sealed class VariationFamily
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Support { get; set; } = string.Empty;

        public List<string> Enums { get; set; } = new();

        public List<VariationSample> Samples { get; set; } = new();

        public List<VariationRow> Rows { get; set; } = new();
    }

    private sealed class VariationSample
    {
        public string Id { get; set; } = string.Empty;

        public string Kind { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string? ExpectedDiagnostics { get; set; }
    }

    private sealed class VariationRow
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Outcome { get; set; } = string.Empty;

        public string Evidence { get; set; } = string.Empty;

        public Dictionary<string, string[]> Values { get; set; } = new(StringComparer.Ordinal);
    }
}
