using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.EnumCoverage.AttributeOnly;

/// <summary>
/// Assembly-level marker used to prove that every Bricks enum value can be expressed by attributes only.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class EnumCoverageAttribute : Attribute
{
    public EnumCoverageAttribute(string scenario, BrickAiCommentFormat value)
        : this(scenario, nameof(BrickAiCommentFormat), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickAiMode value)
        : this(scenario, nameof(BrickAiMode), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickAssignmentAuthority value)
        : this(scenario, nameof(BrickAssignmentAuthority), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickAssignmentBehavior value)
        : this(scenario, nameof(BrickAssignmentBehavior), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickAssignmentMode value)
        : this(scenario, nameof(BrickAssignmentMode), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickAssignmentSource value)
        : this(scenario, nameof(BrickAssignmentSource), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickAssignmentSpecificity value)
        : this(scenario, nameof(BrickAssignmentSpecificity), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickBenchmarkComparisonStatus value)
        : this(scenario, nameof(BrickBenchmarkComparisonStatus), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickBenchmarkStatus value)
        : this(scenario, nameof(BrickBenchmarkStatus), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickBenchmarkSubject value)
        : this(scenario, nameof(BrickBenchmarkSubject), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickCombinationKind value)
        : this(scenario, nameof(BrickCombinationKind), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickConfigurationSourceKind value)
        : this(scenario, nameof(BrickConfigurationSourceKind), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickConformanceCapabilityStatus value)
        : this(scenario, nameof(BrickConformanceCapabilityStatus), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickConformanceLevel value)
        : this(scenario, nameof(BrickConformanceLevel), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickConformanceLevelStatus value)
        : this(scenario, nameof(BrickConformanceLevelStatus), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickDecision value)
        : this(scenario, nameof(BrickDecision), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickDependencyCoverageStatus value)
        : this(scenario, nameof(BrickDependencyCoverageStatus), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickDependencyLayer value)
        : this(scenario, nameof(BrickDependencyLayer), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickDependencyStrength value)
        : this(scenario, nameof(BrickDependencyStrength), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickElementKind value)
        : this(scenario, nameof(BrickElementKind), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickElementOrigin value)
        : this(scenario, nameof(BrickElementOrigin), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickElementSource value)
        : this(scenario, nameof(BrickElementSource), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickEnforcementMode value)
        : this(scenario, nameof(BrickEnforcementMode), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickEvidenceLevel value)
        : this(scenario, nameof(BrickEvidenceLevel), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickGovernanceArea value)
        : this(scenario, nameof(BrickGovernanceArea), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickGovernanceAreaStatus value)
        : this(scenario, nameof(BrickGovernanceAreaStatus), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickGovernanceRequirementStatus value)
        : this(scenario, nameof(BrickGovernanceRequirementStatus), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickPermissionDefault value)
        : this(scenario, nameof(BrickPermissionDefault), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickPolicyImportMode value)
        : this(scenario, nameof(BrickPolicyImportMode), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickReflectionConfidence value)
        : this(scenario, nameof(BrickReflectionConfidence), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickRemediationKind value)
        : this(scenario, nameof(BrickRemediationKind), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickRemediationRisk value)
        : this(scenario, nameof(BrickRemediationRisk), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickRoadmapItemStatus value)
        : this(scenario, nameof(BrickRoadmapItemStatus), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickRoadmapStage value)
        : this(scenario, nameof(BrickRoadmapStage), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickRoadmapStageStatus value)
        : this(scenario, nameof(BrickRoadmapStageStatus), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickRuleLifecycleState value)
        : this(scenario, nameof(BrickRuleLifecycleState), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickScope value)
        : this(scenario, nameof(BrickScope), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickSeverity value)
        : this(scenario, nameof(BrickSeverity), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickViolationKind value)
        : this(scenario, nameof(BrickViolationKind), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, BrickViolationState value)
        : this(scenario, nameof(BrickViolationState), value.ToString())
    {
    }

    public EnumCoverageAttribute(string scenario, RuleMode value)
        : this(scenario, nameof(RuleMode), value.ToString())
    {
    }

    private EnumCoverageAttribute(string scenario, string enumName, string valueName)
    {
        Scenario = scenario;
        EnumName = enumName;
        ValueName = valueName;
    }

    public string Scenario { get; }
    public string EnumName { get; }
    public string ValueName { get; }
}
