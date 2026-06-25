namespace Samples.Block04.Bricks.EnumCoverage.Code;

/// <summary>
/// Describes one enum value that is represented by code-based Bricks configuration.
/// </summary>
public sealed record CodeEnumCoverageRecord(string EnumName, string ValueName, string Scenario, object Value);