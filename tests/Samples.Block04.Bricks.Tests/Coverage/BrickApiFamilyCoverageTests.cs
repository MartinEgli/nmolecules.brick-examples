using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;

namespace BrickExamples.Tests.Coverage;

/// <summary>
/// Verifies that every public Bricks API source area has executable or
/// documented sample evidence.
/// </summary>
public sealed class BrickApiFamilyCoverageTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly Regex PublicTypePattern = new(
        @"public\s+(?:sealed\s+|static\s+|readonly\s+|abstract\s+|partial\s+)*(?:class|struct|enum|interface)\s+([A-Za-z_][A-Za-z0-9_]*)",
        RegexOptions.Compiled);

    [Fact]
    public void MatrixCoversEveryPublicBricksSourceAreaAndType()
    {
        var expected = GetPublicTypesBySourceArea();
        var matrix = LoadMatrix();
        var actual = matrix.Areas.ToDictionary(area => area.SourceDirectory, StringComparer.Ordinal);
        var failures = new List<string>();

        AddDuplicateFailures("area", matrix.Areas.Select(area => area.Id), failures);
        AddDuplicateFailures("source directory", matrix.Areas.Select(area => area.SourceDirectory), failures);

        foreach (var expectedArea in expected.Keys.OrderBy(area => area, StringComparer.Ordinal))
        {
            if (!actual.TryGetValue(expectedArea, out var area))
            {
                failures.Add($"Missing API family coverage area for source directory '{expectedArea}'.");
                continue;
            }

            var expectedTypes = expected[expectedArea];
            var actualTypes = area.PublicTypes.ToHashSet(StringComparer.Ordinal);
            var missingTypes = expectedTypes.Except(actualTypes).OrderBy(type => type, StringComparer.Ordinal).ToArray();
            var unexpectedTypes = actualTypes.Except(expectedTypes).OrderBy(type => type, StringComparer.Ordinal).ToArray();

            if (missingTypes.Length > 0)
            {
                failures.Add($"{expectedArea} is missing public types: {string.Join(", ", missingTypes)}.");
            }

            if (unexpectedTypes.Length > 0)
            {
                failures.Add($"{expectedArea} lists unknown public types: {string.Join(", ", unexpectedTypes)}.");
            }
        }

        foreach (var unexpectedArea in actual.Keys.Except(expected.Keys, StringComparer.Ordinal).OrderBy(area => area, StringComparer.Ordinal))
        {
            failures.Add($"Matrix lists source directory '{unexpectedArea}' but no public Bricks types were found there.");
        }

        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    [Fact]
    public void MatrixReferencesStableExistingSampleEvidence()
    {
        var examplesRoot = FindExamplesRoot();
        var matrix = LoadMatrix();
        var failures = new List<string>();
        var supportedKinds = new[] { "analyzer-backed", "runtime-backed", "sample-backed", "documentation-backed" };

        if (matrix.SchemaVersion != 1)
        {
            failures.Add($"Unsupported schema version {matrix.SchemaVersion}.");
        }

        if (!string.Equals(matrix.Status, "active", StringComparison.Ordinal))
        {
            failures.Add($"Unsupported matrix status '{matrix.Status}'.");
        }

        foreach (var area in matrix.Areas)
        {
            if (!supportedKinds.Contains(area.Support, StringComparer.Ordinal))
            {
                failures.Add($"{area.Id} has unsupported support kind '{area.Support}'.");
            }

            if (area.Samples.Count == 0)
            {
                failures.Add($"{area.Id} does not reference sample evidence.");
            }

            AddDuplicateFailures($"{area.Id} sample", area.Samples.Select(sample => sample.Id), failures);

            foreach (var sample in area.Samples)
            {
                var samplePath = Path.Combine(examplesRoot, sample.Path.Replace('/', Path.DirectorySeparatorChar));
                if (!File.Exists(samplePath) && !Directory.Exists(samplePath))
                {
                    failures.Add($"{area.Id}/{sample.Id} points to missing sample path {sample.Path}.");
                }
            }
        }

        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    [Fact]
    public void ApiCoverageChecklistMentionsEveryCoveredFamily()
    {
        var examplesRoot = FindExamplesRoot();
        var checklist = File.ReadAllText(Path.Combine(
            examplesRoot,
            "samples",
            "bricks",
            "implementation-samples",
            "function-coverage",
            "API_COVERAGE.md"));
        var failures = LoadMatrix()
            .Areas
            .Where(area => !checklist.Contains(area.CoverageLabel, StringComparison.Ordinal))
            .Select(area => $"{area.Id} is missing checklist label '{area.CoverageLabel}'.")
            .ToArray();

        Assert.True(failures.Length == 0, string.Join(Environment.NewLine, failures));
    }

    private static ApiFamilyCoverageMatrix LoadMatrix()
    {
        var matrixPath = Path.Combine(FindExamplesRoot(), "coverage", "bricks-api-family-coverage.json");
        using var stream = File.OpenRead(matrixPath);
        return JsonSerializer.Deserialize<ApiFamilyCoverageMatrix>(stream, JsonOptions)
            ?? throw new InvalidOperationException("Bricks API family coverage matrix could not be deserialized.");
    }

    private static Dictionary<string, HashSet<string>> GetPublicTypesBySourceArea()
    {
        var sourceRoot = FindBricksSourceRoot();
        var result = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);

        foreach (var file in Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories))
        {
            if (IsExcludedSourceFile(file))
            {
                continue;
            }

            var relative = Path.GetRelativePath(sourceRoot, file);
            var parts = relative.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (parts.Length < 2)
            {
                continue;
            }

            var area = parts[0];
            var text = File.ReadAllText(file);
            var typeNames = PublicTypePattern
                .Matches(text)
                .Select(match => match.Groups[1].Value)
                .ToArray();

            if (typeNames.Length == 0)
            {
                continue;
            }

            if (!result.TryGetValue(area, out var publicTypes))
            {
                publicTypes = new HashSet<string>(StringComparer.Ordinal);
                result.Add(area, publicTypes);
            }

            foreach (var typeName in typeNames)
            {
                publicTypes.Add(typeName);
            }
        }

        return result;
    }

    private static bool IsExcludedSourceFile(string file)
    {
        var normalized = file.Replace(Path.DirectorySeparatorChar, '/');
        return normalized.Contains("/docs/", StringComparison.Ordinal)
            || normalized.Contains("/bin/", StringComparison.Ordinal)
            || normalized.Contains("/obj/", StringComparison.Ordinal)
            || Path.GetFileName(file).Equals("NamespaceDoc.cs", StringComparison.Ordinal);
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

    private static string FindBricksSourceRoot()
    {
        var examplesRoot = new DirectoryInfo(FindExamplesRoot());
        var superProjectRoot = examplesRoot.Parent?.FullName
            ?? throw new InvalidOperationException("Could not locate superproject root.");
        var sourceRoot = Path.Combine(superProjectRoot, "nmolecules", "src", "nMolecules.Bricks");

        if (!Directory.Exists(sourceRoot))
        {
            throw new InvalidOperationException($"Could not find Bricks source root at {sourceRoot}.");
        }

        return sourceRoot;
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

    private sealed class ApiFamilyCoverageMatrix
    {
        public int SchemaVersion { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<ApiFamilyCoverageArea> Areas { get; set; } = new();
    }

    private sealed class ApiFamilyCoverageArea
    {
        public string Id { get; set; } = string.Empty;

        public string SourceDirectory { get; set; } = string.Empty;

        public string CoverageLabel { get; set; } = string.Empty;

        public string Support { get; set; } = string.Empty;

        public List<ApiFamilyCoverageSample> Samples { get; set; } = new();

        public List<string> PublicTypes { get; set; } = new();
    }

    private sealed class ApiFamilyCoverageSample
    {
        public string Id { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;
    }
}
