using System;
using System.IO;
using System.Linq;
using Xunit;

namespace BrickExamples.Tests.Documentation;

public sealed class BricksSampleRoadmapTests
{
    [Fact]
    public void ConsumerSampleRoadmapMentionsEveryExecutableSampleArea()
    {
        var examplesRoot = FindExamplesRoot();
        var roadmap = File.ReadAllText(Path.Combine(
            examplesRoot,
            "samples",
            "bricks",
            "docs",
            "consumer-sample-roadmap.md"));
        var implementationSamplesRoot = Path.Combine(
            examplesRoot,
            "samples",
            "bricks",
            "implementation-samples");
        var sampleAreas = Directory.GetDirectories(implementationSamplesRoot)
            .Select(Path.GetFileName)
            .Where(name => name is not null)
            .Where(name => name != "bin" && name != "obj")
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        Assert.Contains("## Current Executable Sample Areas", roadmap);
        Assert.Contains("## Explicit Capability Boundary", roadmap);
        Assert.DoesNotContain("Valuable Samples That Do Not Exist Yet", roadmap);

        foreach (var sampleArea in sampleAreas)
        {
            Assert.Contains($"`{sampleArea}`", roadmap);
        }
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
}
