using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.FunctionCoverage;

/// <summary>
/// Demonstrates how Bricks can describe rollout maturity, operational governance,
/// roadmap status and dependency-observability coverage.
/// </summary>
internal static class GovernanceAndReadinessExamples
{
    public static BrickGovernanceReport AssessGovernance()
    {
        var assessments = BrickBuiltInGovernanceAreas.All.Select(area =>
            new BrickGovernanceAreaAssessment(
                area,
                area.Requirements.Select(requirement =>
                    new BrickGovernanceRequirementResult(
                        requirement,
                        requirement.Id is "suppression-owner" or "baseline-expiry" ? BrickGovernanceRequirementStatus.Missing : BrickGovernanceRequirementStatus.Satisfied,
                        requirement.Required ? "Covered by architecture review checklist." : "Optional for this rollout."))));

        return new BrickGovernanceReport(SampleBrickModel.GeneratedAt, assessments);
    }

    public static BrickConformanceReport AssessConformance()
    {
        var assessments = BrickBuiltInConformanceLevels.All.Select(level =>
            new BrickConformanceLevelAssessment(
                level,
                level.Capabilities.Select(capability =>
                    new BrickConformanceCapabilityResult(
                        capability,
                        capability.Id is "ide-visualisation" ? BrickConformanceCapabilityStatus.NotApplicable : BrickConformanceCapabilityStatus.Satisfied,
                        "Covered by the focused Brick examples and core implementation."))));

        return new BrickConformanceReport(SampleBrickModel.GeneratedAt, assessments);
    }

    public static BrickRoadmapReport AssessRoadmap()
    {
        var assessments = BrickBuiltInRoadmapStages.All.Select(stage =>
            new BrickRoadmapStageAssessment(
                stage,
                stage.IncludedItems.Select(item =>
                    new BrickRoadmapItemResult(
                        item,
                        item.Id.Contains("ide") ? BrickRoadmapItemStatus.NotRequired : BrickRoadmapItemStatus.Completed,
                        "Example-backed implementation slice."))));

        return new BrickRoadmapReport(SampleBrickModel.GeneratedAt, assessments);
    }

    public static BrickDependencyCoverageReport AssessDependencyCoverage()
    {
        var results = BrickBuiltInDependencyCoverageTargets.All.Select(target =>
            new BrickDependencyCoverageResult(
                target,
                analyzedDependencies: target.Layer == BrickDependencyLayer.Static ? 20 : 5,
                observableDependencies: target.Layer == BrickDependencyLayer.Static ? 20 : 4,
                observedEvidenceLevel: target.MinimumEvidenceLevel,
                notes: "Sample coverage explains how this dependency kind becomes visible."));

        return new BrickDependencyCoverageReport(SampleBrickModel.GeneratedAt, results);
    }

    public static string[] DescribeBuiltInCatalogs()
    {
        var profiles = BrickBuiltInProfiles.All.Select(profile => profile.Id).ToArray();
        var rolePacks = BrickBuiltInRolePacks.All.Select(pack => pack.Id).ToArray();
        var governanceJson = BrickGovernanceReportJsonSerializer.Serialize(AssessGovernance());
        var conformanceJson = BrickConformanceReportJsonSerializer.Serialize(AssessConformance());
        var roadmapJson = BrickRoadmapReportJsonSerializer.Serialize(AssessRoadmap());
        var coverageJson = BrickDependencyCoverageReportJsonSerializer.Serialize(AssessDependencyCoverage());

        return profiles
            .Concat(rolePacks)
            .Concat(new[] { governanceJson, conformanceJson, roadmapJson, coverageJson })
            .ToArray();
    }
}
