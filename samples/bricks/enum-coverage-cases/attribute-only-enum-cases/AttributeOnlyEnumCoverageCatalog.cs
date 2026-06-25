using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Samples.Block04.Bricks.EnumCoverage.AttributeOnly;

/// <summary>
/// Reads enum coverage facts from assembly attributes so teams can see a pure attribute configuration style.
/// </summary>
public static class AttributeOnlyEnumCoverageCatalog
{
    public static IReadOnlyList<AttributeOnlyEnumCoverageRecord> All { get; } = typeof(AttributeOnlyEnumCoverageCatalog)
        .Assembly
        .GetCustomAttributes<EnumCoverageAttribute>()
        .Select(attribute => new AttributeOnlyEnumCoverageRecord(attribute.EnumName, attribute.ValueName, attribute.Scenario))
        .OrderBy(record => record.EnumName)
        .ThenBy(record => record.ValueName)
        .ToArray();
}