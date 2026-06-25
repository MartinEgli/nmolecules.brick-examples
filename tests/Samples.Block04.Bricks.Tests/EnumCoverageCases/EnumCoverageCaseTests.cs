using System;
using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;
using Samples.Block04.Bricks.EnumCoverage.AttributeOnly;
using Samples.Block04.Bricks.EnumCoverage.Code;
using Xunit;

namespace BrickExamples.Tests.EnumCoverageCases;

/// <summary>
/// Verifies that every public Bricks enum value has both an attribute-only and a code sample.
/// </summary>
public sealed class EnumCoverageCaseTests
{
    [Fact]
    public void AttributeOnlyCatalogCoversEveryPublicBricksEnumValue()
    {
        var expected = GetPublicBricksEnumValues();
        var actual = AttributeOnlyEnumCoverageCatalog.All
            .Select(record => (record.EnumName, record.ValueName))
            .ToHashSet();

        Assert.Equal(expected.Count, actual.Count);
        Assert.Empty(expected.Except(actual));
        Assert.Empty(actual.Except(expected));
    }

    [Fact]
    public void CodeCatalogCoversEveryPublicBricksEnumValue()
    {
        var expected = GetPublicBricksEnumValues();
        var actual = CodeEnumCoverageCatalog.All
            .Select(record => (record.EnumName, record.ValueName))
            .ToHashSet();

        Assert.Equal(expected.Count, actual.Count);
        Assert.Empty(expected.Except(actual));
        Assert.Empty(actual.Except(expected));
    }

    [Fact]
    public void CodeCatalogKeepsTypedEnumConstants()
    {
        Assert.All(CodeEnumCoverageCatalog.All, record =>
        {
            var enumType = typeof(BrickElementKind).Assembly.GetType($"NMolecules.Bricks.{record.EnumName}", throwOnError: true);

            Assert.True(enumType!.IsEnum);
            Assert.IsType(enumType, record.Value);
            Assert.Equal(record.ValueName, record.Value.ToString());
        });
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
}
