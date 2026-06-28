using System.Collections.Generic;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.FunctionCoverage;

/// <summary>
/// Catalogs clean violation examples for every public Bricks API family and
/// every meaningful enum variation family.
/// </summary>
internal static class ViolationExamples
{
    public static IReadOnlyList<BrickViolationExample> All { get; } = new[]
    {
        Api(
            "adoption-expired-suppression",
            "Expired suppression keeps the violation visible.",
            "adoption",
            BrickViolationKind.Suppression,
            BrickViolationState.ExpiredSuppression,
            "samples/bricks/implementation-samples/function-coverage/ViolationExamples.cs"),
        Api(
            "ai-unreviewed-rule-proposal",
            "AI may propose a rule, but unreviewed proposals are not enforcement authority.",
            "ai",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/ai-governance"),
        Api(
            "attribute-incomplete-metadata",
            "Incomplete role, rule, dependency or member-contract attributes produce configuration diagnostics.",
            "attributes",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/analyzer-samples/metadata-validation/Samples.Bricks.Analyzers.MetadataValidation.csproj",
            "15 XMoleculesBricks0002 warnings"),
        Api(
            "benchmark-over-budget",
            "A Bricks operation that exceeds its benchmark budget is reported as a regression.",
            "benchmarking",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/BenchmarkAndAiExamples.cs"),
        Api(
            "configuration-shadowed-source",
            "Lower-precedence configuration is shadowed by an explicit source annotation.",
            "configuration",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/ConfigurationExamples.cs"),
        Api(
            "conformance-missing-capability",
            "A required conformance capability is missing from the current Bricks maturity level.",
            "conformance",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/GovernanceAndReadinessExamples.cs"),
        Api(
            "core-unknown-element-boundary",
            "Unknown element metadata is kept explicit instead of being silently inferred.",
            "core",
            BrickViolationKind.RoleResolution,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/SampleBrickModel.cs"),
        Api(
            "dependency-insufficient-evidence",
            "Runtime and configuration dependencies remain visible when static evidence is insufficient.",
            "dependencies",
            BrickViolationKind.RequiredDependency,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/GovernanceAndReadinessExamples.cs"),
        Api(
            "export-active-violation",
            "Exports carry active rule violations into role maps, dependency graphs and reports.",
            "export",
            BrickViolationKind.DependencyRule,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/RuntimeAndExportExamples.cs"),
        Api(
            "governance-missing-requirement",
            "Governance reports make missing exception-handling requirements visible.",
            "governance",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/GovernanceAndReadinessExamples.cs"),
        Api(
            "identity-empty-rule-id",
            "Typed IDs prevent empty or unstable identifiers from becoming silent rule keys.",
            "identity",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/PolicyAndResolutionExamples.cs"),
        Api(
            "io-invalid-policy-document",
            "JSON helpers surface invalid Bricks documents instead of hiding file-shape errors.",
            "io",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/RuntimeAndExportExamples.cs"),
        Api(
            "members-cardinality-mismatch",
            "Member-cardinality contracts fail when required markers are missing, duplicated, out of range, forbidden, duplicate-named or combined incorrectly.",
            "members",
            BrickViolationKind.MemberCardinality,
            BrickViolationState.Active,
            "samples/bricks/analyzer-samples/member-contract-validation/Samples.Bricks.Analyzers.MemberContractValidation.csproj",
            "9 XMoleculesBricks0003-0010 errors"),
        Api(
            "model-direct-dependency-violation",
            "A modeled source element depends directly on a forbidden target element.",
            "model",
            BrickViolationKind.DependencyRule,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/SampleBrickModel.cs"),
        Api(
            "policy-invalid-composition",
            "Policy composition can expose invalid imports, conflicting defaults or missing rule metadata.",
            "policies",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/analyzer-samples/brick-policy-violations/Samples.Block04.Bricks.Violations.csproj",
            "1 XMoleculesBricks0001 error and 1 XMoleculesBricks0002 warning"),
        Api(
            "profile-unfit-default",
            "A built-in profile can reveal that the selected defaults do not fit a team's dependency shape.",
            "profiles",
            BrickViolationKind.RoleCombination,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/GovernanceAndReadinessExamples.cs"),
        Api(
            "reflection-low-confidence-access",
            "Reflection access with low confidence remains a reviewable architecture risk.",
            "reflection",
            BrickViolationKind.RequiredDependency,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/RuntimeAndExportExamples.cs"),
        Api(
            "report-active-finding",
            "Reports and SARIF keep active findings explicit for CI and dashboard consumers.",
            "reports",
            BrickViolationKind.DependencyRule,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/RuntimeAndExportExamples.cs"),
        Api(
            "roadmap-missing-item",
            "Roadmap reports keep missing rollout items visible instead of treating them as complete.",
            "roadmap",
            BrickViolationKind.PolicyConfiguration,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/GovernanceAndReadinessExamples.cs"),
        Api(
            "role-combination-conflict",
            "An element with incompatible roles is reported as a role-combination violation.",
            "roles",
            BrickViolationKind.RoleCombination,
            BrickViolationState.Active,
            "samples/bricks/analyzer-samples/generic-role-combinations/Samples.Bricks.Analyzers.GenericRoleCombinations.csproj",
            "6 XMoleculesBricks0001 errors"),
        Api(
            "rule-forbidden-dependency",
            "A forbidden dependency rule is violated by direct type usage.",
            "rules",
            BrickViolationKind.DependencyRule,
            BrickViolationState.Active,
            "samples/bricks/analyzer-samples/dependency-rule-evaluation/Samples.Bricks.Analyzers.DependencyRuleEvaluation.csproj",
            "3 XMoleculesBricks0001 errors"),
        Api(
            "runtime-unreviewed-registration",
            "A runtime registration exposes a dependency that static type checks cannot see.",
            "runtime",
            BrickViolationKind.RequiredDependency,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/RuntimeAndExportExamples.cs"),
        Api(
            "visibility-friend-assembly-exception",
            "Friend-assembly visibility exceptions stay reviewable as architecture dependencies.",
            "visibility",
            BrickViolationKind.DependencyRule,
            BrickViolationState.Active,
            "samples/bricks/implementation-samples/function-coverage/RuntimeAndExportExamples.cs"),

        EnumFamily(
            "element-classification-violation",
            "Unknown, generated and external element classifications are treated as review boundaries.",
            "element-classification",
            new Dictionary<string, string[]>
            {
                ["BrickElementKind"] = new[] { "Assembly", "Namespace", "Type", "Member", "Attribute", "DependencyRegistration", "ExternalReference", "Unknown" },
                ["BrickScope"] = new[] { "Assembly", "Namespace", "Type", "Member", "Global" },
                ["BrickElementOrigin"] = new[] { "Source", "Generated", "External", "Metadata", "Unknown" },
                ["BrickElementSource"] = new[] { "Code", "Configuration", "Convention", "Inference", "Import", "Unknown" }
            }),
        EnumFamily(
            "dependency-policy-violation",
            "Denied or required dependencies are evaluated with explicit layer, strength and evidence.",
            "dependency-policy",
            new Dictionary<string, string[]>
            {
                ["RuleMode"] = new[] { "ForbidDependency", "RequireDependency" },
                ["BrickDecision"] = new[] { "Allow", "Deny", "Require" },
                ["BrickDependencyLayer"] = new[] { "Static", "Runtime", "Configuration", "Visibility" },
                ["BrickDependencyStrength"] = new[] { "Direct", "Indirect", "Inferred" },
                ["BrickEvidenceLevel"] = new[] { "CompilerConfirmed", "AnalyzerInferred", "RuntimeInferred", "ConfigurationDeclared", "Unknown" },
                ["BrickDependencyCoverageStatus"] = new[] { "Covered", "PartiallyObservable", "NotObservable", "InsufficientEvidence" }
            }),
        EnumFamily(
            "violation-enforcement-violation",
            "Violation state, severity and lifecycle determine whether a finding blocks or informs.",
            "violation-enforcement",
            new Dictionary<string, string[]>
            {
                ["BrickViolationKind"] = new[] { "DependencyRule", "RequiredDependency", "RoleResolution", "RoleCombination", "PolicyConfiguration", "Baseline", "Suppression", "MemberCardinality" },
                ["BrickViolationState"] = new[] { "Active", "Suppressed", "Baselined", "ExpiredSuppression", "ExpiredBaseline" },
                ["BrickSeverity"] = new[] { "Info", "Warning", "Error" },
                ["BrickEnforcementMode"] = new[] { "Disabled", "Document", "Analyze", "Enforce" },
                ["BrickRuleLifecycleState"] = new[] { "Candidate", "Draft", "Observing", "Warning", "Enforced", "Rejected", "Deprecated" }
            }),
        EnumFamily(
            "role-assignment-violation",
            "Conflicting role assignments expose source, authority, specificity and behavior.",
            "role-assignment",
            new Dictionary<string, string[]>
            {
                ["BrickAssignmentMode"] = new[] { "DirectAttribute", "AliasMapping", "ExternalConfiguration", "ImportedPack", "Convention", "Generated", "Inference" },
                ["BrickAssignmentSource"] = new[] { "SourceAttribute", "AliasMapping", "PolicyFile", "Package", "Generator", "Convention", "Inference" },
                ["BrickAssignmentSpecificity"] = new[] { "Element", "Namespace", "Assembly", "Convention", "Inference" },
                ["BrickAssignmentAuthority"] = new[] { "Direct", "Alias", "External", "Derived" },
                ["BrickAssignmentBehavior"] = new[] { "Apply", "Suppress" }
            }),
        EnumFamily(
            "policy-composition-violation",
            "Policy imports and defaults reveal incompatible or narrowed combinations.",
            "policy-composition",
            new Dictionary<string, string[]>
            {
                ["BrickPolicyImportMode"] = new[] { "Import", "Extend", "Override", "Disable", "Narrow" },
                ["BrickConfigurationSourceKind"] = new[] { "Generated", "Package", "PolicyFile", "MSBuild", "AnalyzerConfig", "SourceAnnotation" },
                ["BrickCombinationKind"] = new[] { "Additive", "Exclusive", "Incompatible" },
                ["BrickPermissionDefault"] = new[] { "Allow", "Deny" }
            }),
        EnumFamily(
            "governance-status-violation",
            "Governance reports identify non-compliance and missing requirements by area.",
            "governance-status",
            new Dictionary<string, string[]>
            {
                ["BrickGovernanceArea"] = new[] { "PolicyOwnership", "ExceptionHandling", "RolePackEvolution", "CompatibilityExpectations" },
                ["BrickGovernanceAreaStatus"] = new[] { "Compliant", "Partial", "NonCompliant" },
                ["BrickGovernanceRequirementStatus"] = new[] { "Satisfied", "Missing", "NotApplicable" }
            }),
        EnumFamily(
            "conformance-status-violation",
            "Conformance reports show partial and missing maturity capabilities.",
            "conformance-status",
            new Dictionary<string, string[]>
            {
                ["BrickConformanceLevel"] = new[] { "Marking", "StaticValidation", "Explainability", "PolicyFiles", "RuntimeAwareAnalysis", "IntegrationAndAugmentation" },
                ["BrickConformanceLevelStatus"] = new[] { "Achieved", "Partial", "NotStarted" },
                ["BrickConformanceCapabilityStatus"] = new[] { "Satisfied", "Missing", "NotApplicable" }
            }),
        EnumFamily(
            "roadmap-status-violation",
            "Roadmap status examples show missing delivery work by stage and item.",
            "roadmap-status",
            new Dictionary<string, string[]>
            {
                ["BrickRoadmapStage"] = new[] { "V1", "V1_1", "V1_2", "V2" },
                ["BrickRoadmapStageStatus"] = new[] { "Complete", "Partial", "NotStarted" },
                ["BrickRoadmapItemStatus"] = new[] { "Completed", "Missing", "NotRequired" }
            }),
        EnumFamily(
            "benchmark-status-violation",
            "Benchmark reports expose budget overruns and regressions per subject.",
            "benchmark-status",
            new Dictionary<string, string[]>
            {
                ["BrickBenchmarkSubject"] = new[] { "RuleEvaluation", "RoleResolution", "PolicyComposition", "ViolationProjection", "RuntimeDependencyEvaluation", "ReportSerialization" },
                ["BrickBenchmarkStatus"] = new[] { "WithinBudget", "OverBudget", "NotBudgeted" },
                ["BrickBenchmarkComparisonStatus"] = new[] { "Stable", "Improved", "Regressed", "NoBaseline" }
            }),
        EnumFamily(
            "ai-remediation-violation",
            "AI remediation examples keep mode, format, risk and confidence explicit.",
            "ai-remediation",
            new Dictionary<string, string[]>
            {
                ["BrickAiMode"] = new[] { "Off", "Explain", "SuggestRules" },
                ["BrickAiCommentFormat"] = new[] { "Markdown", "Json", "Both" },
                ["BrickRemediationKind"] = new[] { "IntroduceContract", "MoveElement", "SplitRole", "AddPortAndAdapter", "ChangeDependencyDirection", "MoveToCompositionRoot", "AdjustPolicy", "AddSuppression", "AddBaselineEntry", "RenameOrReclassifyElement" },
                ["BrickRemediationRisk"] = new[] { "Low", "Medium", "High" },
                ["BrickReflectionConfidence"] = new[] { "Low", "Medium", "High" }
            })
    };

    private static BrickViolationExample Api(
        string id,
        string title,
        string apiFamilyId,
        BrickViolationKind kind,
        BrickViolationState state,
        string evidencePath,
        string expectedDiagnostics = "") =>
        new(
            id,
            title,
            new[] { apiFamilyId },
            null,
            CreateViolation(id, title, kind, state),
            new Dictionary<string, string[]>(),
            evidencePath,
            expectedDiagnostics);

    private static BrickViolationExample EnumFamily(
        string id,
        string title,
        string enumVariationFamilyId,
        IReadOnlyDictionary<string, string[]> enumValues) =>
        new(
            id,
            title,
            new[] { "core" },
            enumVariationFamilyId,
            CreateViolation(id, title, BrickViolationKind.PolicyConfiguration, BrickViolationState.Active),
            enumValues,
            "samples/bricks/implementation-samples/function-coverage/ViolationExamples.cs",
            "");

    private static BrickViolation CreateViolation(
        string id,
        string title,
        BrickViolationKind kind,
        BrickViolationState state)
    {
        var source = SampleBrickModel.Type("Billing.Application." + id.Replace('-', '_'));
        var target = SampleBrickModel.Type("Billing.Infrastructure." + id.Replace('-', '_') + "Target");
        return new BrickViolation(
            kind,
            source,
            title,
            BrickSeverity.Error,
            state,
            RuleId.From("XMoleculesBricks0001"),
            id,
            target,
            new[] { RoleId.From("Billing.Application") },
            new[] { RoleId.From("Billing.Infrastructure") },
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            BrickScope.Type,
            BrickDependencyLayer.Static,
            BrickEvidenceLevel.CompilerConfirmed,
            state == BrickViolationState.Active ? null : state.ToString());
    }
}

internal sealed record BrickViolationExample(
    string Id,
    string Title,
    IReadOnlyList<string> ApiFamilyIds,
    string? EnumVariationFamilyId,
    BrickViolation Violation,
    IReadOnlyDictionary<string, string[]> EnumValues,
    string EvidencePath,
    string ExpectedDiagnostics);
