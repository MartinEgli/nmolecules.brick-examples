using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.FunctionCoverage;

/// <summary>
/// Demonstrates configuration precedence and the bridge from established
/// nMolecules attributes to Bricks role assignments.
/// </summary>
internal static class ConfigurationExamples
{
    public static BrickConfigurationResolution ResolveConfiguration()
    {
        var generated = new BrickConfigurationEntry(
            "bricks.enforcement",
            "Document",
            BrickConfigurationSource.Generated);
        var packageDefault = new BrickConfigurationEntry(
            "bricks.enforcement",
            "Analyze",
            new BrickConfigurationSource("package-defaults", BrickConfigurationSourceKind.Package, "Default package behavior."));
        var policyFile = new BrickConfigurationEntry(
            "bricks.enforcement",
            "Enforce",
            new BrickConfigurationSource("billing-policy.json", BrickConfigurationSourceKind.PolicyFile, "Team-owned policy."));
        var msBuild = new BrickConfigurationEntry(
            "bricks.enforcement",
            "Analyze",
            new BrickConfigurationSource("Directory.Build.props", BrickConfigurationSourceKind.MSBuild, "Repository build setting."));
        var analyzerConfig = new BrickConfigurationEntry(
            "bricks.enforcement",
            "Analyze",
            new BrickConfigurationSource(".editorconfig", BrickConfigurationSourceKind.AnalyzerConfig, "Analyzer config."));
        var sourceAnnotation = new BrickConfigurationEntry(
            "bricks.enforcement",
            "Enforce",
            new BrickConfigurationSource("assembly attribute", BrickConfigurationSourceKind.SourceAnnotation, "Most local override."));

        _ = BrickConfigurationPrecedence.Rank(BrickConfigurationSourceKind.PolicyFile);
        return BrickConfigurationResolver.Resolve(new[] { generated, packageDefault, policyFile, msBuild, analyzerConfig, sourceAnnotation });
    }

    public static BrickRoleAssignment[] BridgeDddAttributesToBrickRoles()
    {
        var order = SampleBrickModel.Type("Billing.Domain.Order");
        var bridge = BrickBuiltInAttributeRoleBridges.Ddd;

        _ = BrickAttributeRoleMapping.Normalize("NMolecules.DDD.AggregateRootAttribute");
        _ = bridge.FindRoleId("EntityAttribute");
        _ = bridge.FindRoleIds(new[] { "AggregateRootAttribute", "EntityAttribute", "UnknownAttribute" }).ToArray();

        return bridge.CreateAssignments(order, new[] { "AggregateRootAttribute", "EntityAttribute" }).ToArray();
    }

    public static BrickPolicy UseBuiltInProfileAsPolicy()
    {
        var profile = BrickBuiltInProfiles.Layered;
        var containsApplicationRole = profile.ContainsRole(RoleId.From("Architecture.Layer.Application"));
        var policy = profile.ToPolicy(BrickPolicyId.From("billing-layered-profile"));

        return containsApplicationRole ? policy : BrickBuiltInProfiles.Hexagonal.ToPolicy();
    }
}
