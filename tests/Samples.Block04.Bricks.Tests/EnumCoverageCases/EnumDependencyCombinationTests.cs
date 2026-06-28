using System;
using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;
using Samples.Block04.Bricks.FunctionCoverage;
using Xunit;

namespace BrickExamples.Tests.EnumCoverageCases;

/// <summary>
/// Verifies enum combinations that are coupled by public Bricks API types.
/// </summary>
public sealed class EnumDependencyCombinationTests
{
    private static readonly HashSet<string> CoveredApiPairs = new(StringComparer.Ordinal)
    {
        Pair(nameof(BrickAiCommentFormat), nameof(BrickAiMode)),
        Pair(nameof(BrickAssignmentAuthority), nameof(BrickAssignmentBehavior)),
        Pair(nameof(BrickAssignmentAuthority), nameof(BrickAssignmentMode)),
        Pair(nameof(BrickAssignmentAuthority), nameof(BrickAssignmentSource)),
        Pair(nameof(BrickAssignmentAuthority), nameof(BrickAssignmentSpecificity)),
        Pair(nameof(BrickAssignmentBehavior), nameof(BrickAssignmentMode)),
        Pair(nameof(BrickAssignmentBehavior), nameof(BrickAssignmentSource)),
        Pair(nameof(BrickAssignmentBehavior), nameof(BrickAssignmentSpecificity)),
        Pair(nameof(BrickAssignmentMode), nameof(BrickAssignmentSource)),
        Pair(nameof(BrickAssignmentMode), nameof(BrickAssignmentSpecificity)),
        Pair(nameof(BrickAssignmentSource), nameof(BrickAssignmentSpecificity)),
        Pair(nameof(BrickBenchmarkComparisonStatus), nameof(BrickBenchmarkStatus)),
        Pair(nameof(BrickBenchmarkComparisonStatus), nameof(BrickBenchmarkSubject)),
        Pair(nameof(BrickBenchmarkStatus), nameof(BrickBenchmarkSubject)),
        Pair(nameof(BrickConformanceCapabilityStatus), nameof(BrickConformanceLevel)),
        Pair(nameof(BrickConformanceCapabilityStatus), nameof(BrickConformanceLevelStatus)),
        Pair(nameof(BrickConformanceLevel), nameof(BrickConformanceLevelStatus)),
        Pair(nameof(BrickDecision), nameof(BrickRuleLifecycleState)),
        Pair(nameof(BrickDecision), nameof(BrickScope)),
        Pair(nameof(BrickDecision), nameof(BrickSeverity)),
        Pair(nameof(BrickDependencyLayer), nameof(BrickScope)),
        Pair(nameof(BrickDependencyLayer), nameof(BrickSeverity)),
        Pair(nameof(BrickDependencyLayer), nameof(BrickViolationKind)),
        Pair(nameof(BrickDependencyLayer), nameof(BrickViolationState)),
        Pair(nameof(BrickDependencyStrength), nameof(BrickScope)),
        Pair(nameof(BrickElementKind), nameof(BrickElementOrigin)),
        Pair(nameof(BrickElementKind), nameof(BrickElementSource)),
        Pair(nameof(BrickElementOrigin), nameof(BrickElementSource)),
        Pair(nameof(BrickEnforcementMode), nameof(BrickPermissionDefault)),
        Pair(nameof(BrickEnforcementMode), nameof(BrickPolicyImportMode)),
        Pair(nameof(BrickEvidenceLevel), nameof(BrickReflectionConfidence)),
        Pair(nameof(BrickEvidenceLevel), nameof(BrickScope)),
        Pair(nameof(BrickEvidenceLevel), nameof(BrickSeverity)),
        Pair(nameof(BrickEvidenceLevel), nameof(BrickViolationKind)),
        Pair(nameof(BrickEvidenceLevel), nameof(BrickViolationState)),
        Pair(nameof(BrickGovernanceArea), nameof(BrickGovernanceAreaStatus)),
        Pair(nameof(BrickGovernanceArea), nameof(BrickGovernanceRequirementStatus)),
        Pair(nameof(BrickGovernanceAreaStatus), nameof(BrickGovernanceRequirementStatus)),
        Pair(nameof(BrickPermissionDefault), nameof(BrickPolicyImportMode)),
        Pair(nameof(BrickRemediationKind), nameof(BrickRemediationRisk)),
        Pair(nameof(BrickRoadmapItemStatus), nameof(BrickRoadmapStage)),
        Pair(nameof(BrickRoadmapItemStatus), nameof(BrickRoadmapStageStatus)),
        Pair(nameof(BrickRoadmapStage), nameof(BrickRoadmapStageStatus)),
        Pair(nameof(BrickScope), nameof(BrickSeverity)),
        Pair(nameof(BrickScope), nameof(BrickViolationKind)),
        Pair(nameof(BrickScope), nameof(BrickViolationState))
    };

    [Theory]
    [MemberData(nameof(RuleSemanticCases))]
    public void RuleSemanticCombinationsPreserveDecisionScopeAndSeverity(
        BrickDecision decision,
        BrickScope scope,
        BrickSeverity severity)
    {
        var rule = new BrickRule(
            RuleId.From($"rule-{decision}-{scope}-{severity}"),
            "Rule semantic combination",
            RoleId.From("Source"),
            RoleId.From("Target"),
            decision,
            scope,
            severity);

        Assert.Equal(decision, rule.Decision);
        Assert.Equal(scope, rule.Scope);
        Assert.Equal(severity, rule.Severity);
    }

    [Theory]
    [MemberData(nameof(RuleProposalCases))]
    public void RuleProposalCombinationsPreserveDecisionSeverityAndLifecycle(
        BrickDecision decision,
        BrickSeverity severity,
        BrickRuleLifecycleState lifecycleState)
    {
        var proposal = new BrickRuleProposal(
            $"proposal-{decision}-{severity}-{lifecycleState}",
            "Rule proposal combination",
            "Review the proposed rule before promotion.",
            BrickRoleSelector.From("Source"),
            BrickRoleSelector.From("Target"),
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            decision,
            severity,
            CompleteEvidence(),
            lifecycleState);

        Assert.Equal(decision, proposal.SuggestedDecision);
        Assert.Equal(severity, proposal.SuggestedSeverity);
        Assert.Equal(lifecycleState, proposal.LifecycleState);
        Assert.True(proposal.IsAdvisory);
        Assert.False(proposal.CanBreakBuild);
    }

    [Theory]
    [MemberData(nameof(DependencyEvidenceCases))]
    public void DependencyEvidenceCombinationsPreserveScopeLayerStrengthAndEvidence(
        BrickScope scope,
        BrickDependencyLayer layer,
        BrickDependencyStrength strength,
        BrickEvidenceLevel evidenceLevel)
    {
        var dependency = new BrickDependency(
            SampleBrickModel.Type("Billing.Application.DependencySource"),
            SampleBrickModel.Type("Billing.Infrastructure.DependencyTarget"),
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            scope,
            layer,
            strength,
            evidenceLevel);
        var attribute = new DependencyAttribute(
            "dependency-combination",
            "Source",
            "Target",
            BrickDependencyKinds.TypeReference,
            scope,
            layer,
            strength,
            evidenceLevel);

        Assert.Equal(scope, dependency.Scope);
        Assert.Equal(layer, dependency.Layer);
        Assert.Equal(strength, dependency.Strength);
        Assert.Equal(evidenceLevel, dependency.EvidenceLevel);
        Assert.Equal(scope, attribute.Scope);
        Assert.Equal(layer, attribute.Layer);
        Assert.Equal(strength, attribute.Strength);
        Assert.Equal(evidenceLevel, attribute.EvidenceLevel);
    }

    [Theory]
    [MemberData(nameof(ViolationProjectionCases))]
    public void ViolationProjectionCombinationsPreserveKindStateSeverityScopeLayerAndEvidence(
        BrickViolationKind kind,
        BrickViolationState state,
        BrickSeverity severity,
        BrickScope scope,
        BrickDependencyLayer layer,
        BrickEvidenceLevel evidenceLevel)
    {
        var violation = new BrickViolation(
            kind,
            SampleBrickModel.Type("Billing.Application.ViolationSource"),
            "Violation projection combination",
            severity,
            state,
            RuleId.From("XMoleculesBricks0001"),
            "Projection rule",
            SampleBrickModel.Type("Billing.Infrastructure.ViolationTarget"),
            new[] { RoleId.From("Source") },
            new[] { RoleId.From("Target") },
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            scope,
            layer,
            evidenceLevel,
            state == BrickViolationState.Active ? null : state.ToString());

        Assert.Equal(kind, violation.Kind);
        Assert.Equal(state, violation.State);
        Assert.Equal(severity, violation.Severity);
        Assert.Equal(scope, violation.Scope);
        Assert.Equal(layer, violation.DependencyLayer);
        Assert.Equal(evidenceLevel, violation.EvidenceLevel);
    }

    [Theory]
    [MemberData(nameof(PolicyDefaultCases))]
    public void PolicyProfileAndAttributeDefaultsPreservePermissionAndEnforcement(
        BrickPermissionDefault permissionDefault,
        BrickEnforcementMode enforcementMode)
    {
        var policy = new BrickPolicy(
            BrickPolicyId.From($"policy-{permissionDefault}-{enforcementMode}"),
            "Policy default combination",
            Array.Empty<BrickPolicyImport>(),
            Array.Empty<BrickRule>(),
            permissionDefault,
            enforcementMode);
        var profile = new BrickProfile(
            $"profile-{permissionDefault}-{enforcementMode}",
            "Profile default combination",
            Array.Empty<BrickRolePack>(),
            Array.Empty<BrickRule>(),
            permissionDefault,
            enforcementMode);
        var attribute = new PolicyAttribute(
            $"attribute-{permissionDefault}-{enforcementMode}",
            "Policy attribute default combination",
            permissionDefault,
            enforcementMode);

        Assert.Equal(permissionDefault, policy.DefaultDecision);
        Assert.Equal(enforcementMode, policy.Enforcement);
        Assert.Equal(permissionDefault, profile.DefaultDecision);
        Assert.Equal(enforcementMode, profile.Enforcement);
        Assert.Equal(permissionDefault, attribute.DefaultDecision);
        Assert.Equal(enforcementMode, attribute.Enforcement);
        Assert.Equal(permissionDefault, profile.ToPolicy().DefaultDecision);
        Assert.Equal(enforcementMode, profile.ToPolicy().Enforcement);
    }

    [Theory]
    [MemberData(nameof(ReflectionEvidenceCases))]
    public void ReflectionEvidenceCombinationsPreserveConfidenceAndProjectedDependencyEvidence(
        BrickEvidenceLevel evidenceLevel,
        BrickReflectionConfidence confidence)
    {
        var access = new BrickReflectionAccess(
            SampleBrickModel.Type("Billing.Application.ReflectionAccess"),
            SampleBrickModel.Type("Billing.Infrastructure.ReflectedType"),
            "Activator.CreateInstance",
            confidence,
            "Legacy plugin activation.",
            evidenceLevel);
        var dependency = access.ToDependency();

        Assert.Equal(confidence, access.Confidence);
        Assert.Equal(evidenceLevel, access.EvidenceLevel);
        Assert.Equal(BrickDependencyLayer.Runtime, dependency.Layer);
        Assert.Equal(BrickDependencyStrength.Inferred, dependency.Strength);
        Assert.Equal(evidenceLevel, dependency.EvidenceLevel);
    }

    [Theory]
    [InlineData(BrickViolationKind.RequiredDependency, BrickDecision.Require, BrickSeverity.Warning, BrickEvidenceLevel.ConfigurationDeclared)]
    [InlineData(BrickViolationKind.DependencyRule, BrickDecision.Deny, BrickSeverity.Error, BrickEvidenceLevel.CompilerConfirmed)]
    public void AiViolationCommentCombinationsProjectDecisionSeverityAndEvidence(
        BrickViolationKind kind,
        BrickDecision expectedDecision,
        BrickSeverity severity,
        BrickEvidenceLevel evidenceLevel)
    {
        var violation = new BrickViolation(
            kind,
            SampleBrickModel.Type("Billing.Application.AiCommentSource"),
            "AI comment projection combination",
            severity,
            BrickViolationState.Active,
            RuleId.From("XMoleculesBricks0001"),
            "AI comment rule",
            SampleBrickModel.Type("Billing.Infrastructure.AiCommentTarget"),
            evidenceLevel: evidenceLevel);
        var option = new BrickRemediationOption(
            "introduce-contract",
            BrickRemediationKind.IntroduceContract,
            "Introduce a stable contract.",
            "Use when a direct implementation dependency is the problem.",
            BrickRemediationRisk.Low,
            isPreferred: true);
        var comment = new BrickAiViolationComment(
            violation,
            "Problem summary",
            "Architectural reason",
            new[] { option },
            option,
            new[] { "Move implementation behind an interface." },
            "Suppress only with owner and expiry.");

        Assert.Equal(expectedDecision, comment.Decision);
        Assert.Equal(severity, comment.Severity);
        Assert.Equal(evidenceLevel, comment.EvidenceLevel);
        Assert.Equal(evidenceLevel != BrickEvidenceLevel.Unknown, comment.IsTraceableToDeterministicEvidence);
    }

    [Theory]
    [MemberData(nameof(ElementClassificationCases))]
    public void ElementClassificationCombinationsPreserveKindOriginAndSource(
        BrickElementKind kind,
        BrickElementOrigin origin,
        BrickElementSource source)
    {
        var element = new BrickElement(
            BrickElementId.From($"element-{kind}-{origin}-{source}"),
            kind,
            "Element classification combination",
            "Billing",
            "Billing.Application",
            $"Billing.Application.{kind}",
            origin,
            source);
        var selector = new BrickElementSelector(kind, "Billing.Application.*", "Billing");

        Assert.Equal(kind, element.Kind);
        Assert.Equal(origin, element.Origin);
        Assert.Equal(source, element.Source);
        Assert.True(selector.Matches(element));
    }

    [Theory]
    [MemberData(nameof(RoleAssignmentCases))]
    public void RoleAssignmentCombinationsPreserveModeSourcePrecedenceAndBehavior(
        BrickAssignmentMode mode,
        BrickAssignmentSource source,
        BrickAssignmentSpecificity specificity,
        BrickAssignmentAuthority authority,
        BrickAssignmentBehavior behavior)
    {
        var precedence = new BrickAssignmentPrecedence(specificity, authority);
        var assignment = new BrickRoleAssignment(
            new BrickElementSelector(BrickElementKind.Type, "Billing.Application.*", "Billing"),
            RoleId.From($"Billing.{mode}.{source}"),
            mode,
            source,
            precedence,
            behavior,
            "Role assignment enum combination.");

        Assert.Equal(mode, assignment.Mode);
        Assert.Equal(source, assignment.Source);
        Assert.Equal(specificity, assignment.Precedence.Specificity);
        Assert.Equal(authority, assignment.Precedence.Authority);
        Assert.Equal(behavior, assignment.Behavior);
    }

    [Theory]
    [MemberData(nameof(PolicyImportCases))]
    public void PolicyImportCombinationsPreserveModeDefaultAndEnforcement(
        BrickPolicyImportMode importMode,
        BrickPermissionDefault permissionDefault,
        BrickEnforcementMode enforcementMode)
    {
        var import = new BrickPolicyImport(BrickPolicyId.From($"policy-import-{importMode}"), importMode);
        var policy = new BrickPolicy(
            BrickPolicyId.From($"policy-{importMode}-{permissionDefault}-{enforcementMode}"),
            "Policy import combination",
            new[] { import },
            Array.Empty<BrickRule>(),
            permissionDefault,
            enforcementMode);

        Assert.Equal(importMode, policy.Imports.Single().Mode);
        Assert.Equal(permissionDefault, policy.DefaultDecision);
        Assert.Equal(enforcementMode, policy.Enforcement);
    }

    [Theory]
    [MemberData(nameof(GovernanceStatusCases))]
    public void GovernanceStatusCombinationsPreserveAreaRequirementAndResolvedStatus(
        BrickGovernanceArea area,
        BrickGovernanceRequirementStatus requirementStatus,
        bool requirementRequired,
        bool addSatisfiedCompanion,
        BrickGovernanceAreaStatus expectedStatus)
    {
        var requirement = new BrickGovernanceRequirement("primary", area, "Primary", requirementRequired);
        var requirements = new List<BrickGovernanceRequirement> { requirement };
        var results = new List<BrickGovernanceRequirementResult>
        {
            new(requirement, requirementStatus, "Primary evidence.")
        };

        if (addSatisfiedCompanion)
        {
            var companion = new BrickGovernanceRequirement("companion", area, "Companion", required: true);
            requirements.Add(companion);
            results.Add(new BrickGovernanceRequirementResult(companion, BrickGovernanceRequirementStatus.Satisfied));
        }

        var definition = new BrickGovernanceAreaDefinition(area, $"{area} governance", "Governance area.", requirements);
        var assessment = new BrickGovernanceAreaAssessment(definition, results);

        Assert.Equal(area, definition.Area);
        Assert.Equal(requirementStatus, results[0].Status);
        Assert.Equal(expectedStatus, assessment.Status);
    }

    [Theory]
    [MemberData(nameof(ConformanceStatusCases))]
    public void ConformanceStatusCombinationsPreserveLevelCapabilityAndResolvedStatus(
        BrickConformanceLevel level,
        BrickConformanceCapabilityStatus capabilityStatus,
        bool capabilityRequired,
        bool addSatisfiedCompanion,
        BrickConformanceLevelStatus expectedStatus)
    {
        var capability = new BrickConformanceCapability("primary", "Primary", capabilityRequired);
        var capabilities = new List<BrickConformanceCapability> { capability };
        var results = new List<BrickConformanceCapabilityResult>
        {
            new(capability, capabilityStatus, "Capability evidence.")
        };

        if (addSatisfiedCompanion)
        {
            var companion = new BrickConformanceCapability("companion", "Companion", required: true);
            capabilities.Add(companion);
            results.Add(new BrickConformanceCapabilityResult(companion, BrickConformanceCapabilityStatus.Satisfied));
        }

        var definition = new BrickConformanceLevelDefinition(level, $"{level} conformance", "Conformance level.", capabilities);
        var assessment = new BrickConformanceLevelAssessment(definition, results);

        Assert.Equal(level, definition.Level);
        Assert.Equal(capabilityStatus, results[0].Status);
        Assert.Equal(expectedStatus, assessment.Status);
    }

    [Theory]
    [MemberData(nameof(RoadmapStatusCases))]
    public void RoadmapStatusCombinationsPreserveStageItemAndResolvedStatus(
        BrickRoadmapStage stage,
        BrickRoadmapItemStatus itemStatus,
        bool itemRequired,
        bool addCompletedCompanion,
        BrickRoadmapStageStatus expectedStatus)
    {
        var item = new BrickRoadmapItem("primary", "Primary", itemRequired);
        var items = new List<BrickRoadmapItem> { item };
        var results = new List<BrickRoadmapItemResult>
        {
            new(item, itemStatus, "Roadmap evidence.")
        };

        if (addCompletedCompanion)
        {
            var companion = new BrickRoadmapItem("companion", "Companion", required: true);
            items.Add(companion);
            results.Add(new BrickRoadmapItemResult(companion, BrickRoadmapItemStatus.Completed));
        }

        var definition = new BrickRoadmapStageDefinition(stage, $"{stage} roadmap", "Roadmap stage.", items, Array.Empty<BrickRoadmapItem>());
        var assessment = new BrickRoadmapStageAssessment(definition, results);

        Assert.Equal(stage, definition.Stage);
        Assert.Equal(itemStatus, results[0].Status);
        Assert.Equal(expectedStatus, assessment.Status);
    }

    [Theory]
    [MemberData(nameof(BenchmarkStatusCases))]
    public void BenchmarkCombinationsPreserveSubjectBudgetAndComparisonStatus(
        BrickBenchmarkSubject subject,
        bool hasBaseline,
        long baselineTicks,
        long currentTicks,
        bool hasBudget,
        long budgetTicks,
        BrickBenchmarkStatus expectedBudgetStatus,
        BrickBenchmarkComparisonStatus expectedComparisonStatus)
    {
        var budget = hasBudget ? new BrickBenchmarkBudget(TimeSpan.FromTicks(budgetTicks)) : null;
        var benchmarkCase = new BrickBenchmarkCase(
            $"benchmark-{subject}",
            $"{subject} benchmark",
            subject,
            operationCount: 1,
            budget);
        var current = new BrickBenchmarkResult(
            benchmarkCase,
            iterations: 1,
            totalOperations: 1,
            TimeSpan.FromTicks(currentTicks));
        var baseline = hasBaseline
            ? new BrickBenchmarkResult(benchmarkCase, iterations: 1, totalOperations: 1, TimeSpan.FromTicks(baselineTicks))
            : null;
        var comparison = BrickBenchmarkComparison.Compare(baseline, current);

        Assert.Equal(subject, current.Case.Subject);
        Assert.Equal(expectedBudgetStatus, current.Status);
        Assert.Equal(subject, comparison.Subject);
        Assert.Equal(expectedComparisonStatus, comparison.Status);
    }

    [Theory]
    [MemberData(nameof(AiRunConfigurationCases))]
    public void AiRunConfigurationCombinationsPreserveModeFormatAndEmission(
        BrickAiMode mode,
        BrickAiCommentFormat format,
        bool allowRuleProposal,
        bool expectedEmitComments,
        bool expectedEmitMarkdown,
        bool expectedEmitJson,
        bool expectedCanCreateRuleProposals)
    {
        var trustBoundary = new BrickAiTrustBoundary(
            mode,
            format,
            allowRuleProposal,
            allowAutoEnforcement: false,
            allowSilentPolicyMutation: false);
        var configuration = new BrickAiRunConfiguration(trustBoundary, "artifacts/ai", "artifacts/ai/proposals.json");

        Assert.Equal(mode, configuration.TrustBoundary.Mode);
        Assert.Equal(format, configuration.TrustBoundary.CommentFormat);
        Assert.Equal(expectedEmitComments, configuration.ShouldEmitComments);
        Assert.Equal(expectedEmitMarkdown, configuration.ShouldEmitMarkdown);
        Assert.Equal(expectedEmitJson, configuration.ShouldEmitJson);
        Assert.Equal(expectedCanCreateRuleProposals, configuration.CanCreateRuleProposals);
    }

    [Theory]
    [InlineData(BrickRemediationKind.IntroduceContract, BrickRemediationRisk.Low)]
    [InlineData(BrickRemediationKind.MoveElement, BrickRemediationRisk.Medium)]
    [InlineData(BrickRemediationKind.ChangeDependencyDirection, BrickRemediationRisk.High)]
    public void RemediationOptionCombinationsPreserveKindAndRisk(
        BrickRemediationKind kind,
        BrickRemediationRisk risk)
    {
        var option = new BrickRemediationOption(
            $"remediation-{kind}-{risk}",
            kind,
            "Remediation combination.",
            "Use when this remediation fits the architecture boundary.",
            risk,
            isPreferred: risk == BrickRemediationRisk.Low);

        Assert.Equal(kind, option.Kind);
        Assert.Equal(risk, option.Risk);
        Assert.Equal(risk == BrickRemediationRisk.Low, option.IsPreferred);
    }

    [Fact]
    public void PreviouslyMissingApiEnumPairsAreCoveredByCombinationUseCases()
    {
        var expectedPairs = new[]
        {
            Pair(nameof(BrickAiCommentFormat), nameof(BrickAiMode)),
            Pair(nameof(BrickAssignmentAuthority), nameof(BrickAssignmentBehavior)),
            Pair(nameof(BrickAssignmentAuthority), nameof(BrickAssignmentMode)),
            Pair(nameof(BrickAssignmentAuthority), nameof(BrickAssignmentSource)),
            Pair(nameof(BrickAssignmentAuthority), nameof(BrickAssignmentSpecificity)),
            Pair(nameof(BrickAssignmentBehavior), nameof(BrickAssignmentMode)),
            Pair(nameof(BrickAssignmentBehavior), nameof(BrickAssignmentSource)),
            Pair(nameof(BrickAssignmentBehavior), nameof(BrickAssignmentSpecificity)),
            Pair(nameof(BrickAssignmentMode), nameof(BrickAssignmentSource)),
            Pair(nameof(BrickAssignmentMode), nameof(BrickAssignmentSpecificity)),
            Pair(nameof(BrickAssignmentSource), nameof(BrickAssignmentSpecificity)),
            Pair(nameof(BrickBenchmarkComparisonStatus), nameof(BrickBenchmarkStatus)),
            Pair(nameof(BrickBenchmarkComparisonStatus), nameof(BrickBenchmarkSubject)),
            Pair(nameof(BrickBenchmarkStatus), nameof(BrickBenchmarkSubject)),
            Pair(nameof(BrickConformanceCapabilityStatus), nameof(BrickConformanceLevel)),
            Pair(nameof(BrickConformanceCapabilityStatus), nameof(BrickConformanceLevelStatus)),
            Pair(nameof(BrickConformanceLevel), nameof(BrickConformanceLevelStatus)),
            Pair(nameof(BrickDecision), nameof(BrickRuleLifecycleState)),
            Pair(nameof(BrickDecision), nameof(BrickScope)),
            Pair(nameof(BrickDecision), nameof(BrickSeverity)),
            Pair(nameof(BrickDependencyLayer), nameof(BrickScope)),
            Pair(nameof(BrickDependencyLayer), nameof(BrickSeverity)),
            Pair(nameof(BrickDependencyLayer), nameof(BrickViolationKind)),
            Pair(nameof(BrickDependencyLayer), nameof(BrickViolationState)),
            Pair(nameof(BrickDependencyStrength), nameof(BrickScope)),
            Pair(nameof(BrickElementKind), nameof(BrickElementOrigin)),
            Pair(nameof(BrickElementKind), nameof(BrickElementSource)),
            Pair(nameof(BrickElementOrigin), nameof(BrickElementSource)),
            Pair(nameof(BrickEnforcementMode), nameof(BrickPermissionDefault)),
            Pair(nameof(BrickEnforcementMode), nameof(BrickPolicyImportMode)),
            Pair(nameof(BrickEvidenceLevel), nameof(BrickReflectionConfidence)),
            Pair(nameof(BrickEvidenceLevel), nameof(BrickScope)),
            Pair(nameof(BrickEvidenceLevel), nameof(BrickSeverity)),
            Pair(nameof(BrickEvidenceLevel), nameof(BrickViolationKind)),
            Pair(nameof(BrickEvidenceLevel), nameof(BrickViolationState)),
            Pair(nameof(BrickGovernanceArea), nameof(BrickGovernanceAreaStatus)),
            Pair(nameof(BrickGovernanceArea), nameof(BrickGovernanceRequirementStatus)),
            Pair(nameof(BrickGovernanceAreaStatus), nameof(BrickGovernanceRequirementStatus)),
            Pair(nameof(BrickPermissionDefault), nameof(BrickPolicyImportMode)),
            Pair(nameof(BrickRemediationKind), nameof(BrickRemediationRisk)),
            Pair(nameof(BrickRoadmapItemStatus), nameof(BrickRoadmapStage)),
            Pair(nameof(BrickRoadmapItemStatus), nameof(BrickRoadmapStageStatus)),
            Pair(nameof(BrickRoadmapStage), nameof(BrickRoadmapStageStatus)),
            Pair(nameof(BrickScope), nameof(BrickSeverity)),
            Pair(nameof(BrickScope), nameof(BrickViolationKind)),
            Pair(nameof(BrickScope), nameof(BrickViolationState))
        };

        var missing = expectedPairs.Except(CoveredApiPairs, StringComparer.Ordinal).ToArray();

        Assert.Empty(missing);
    }

    public static IEnumerable<object[]> RuleSemanticCases()
    {
        yield return new object[] { BrickDecision.Deny, BrickScope.Type, BrickSeverity.Error };
        yield return new object[] { BrickDecision.Require, BrickScope.Namespace, BrickSeverity.Warning };
        yield return new object[] { BrickDecision.Allow, BrickScope.Assembly, BrickSeverity.Info };
    }

    public static IEnumerable<object[]> RuleProposalCases()
    {
        yield return new object[] { BrickDecision.Deny, BrickSeverity.Error, BrickRuleLifecycleState.Candidate };
        yield return new object[] { BrickDecision.Require, BrickSeverity.Warning, BrickRuleLifecycleState.Observing };
        yield return new object[] { BrickDecision.Allow, BrickSeverity.Info, BrickRuleLifecycleState.Deprecated };
    }

    public static IEnumerable<object[]> DependencyEvidenceCases()
    {
        yield return new object[] { BrickScope.Type, BrickDependencyLayer.Static, BrickDependencyStrength.Direct, BrickEvidenceLevel.CompilerConfirmed };
        yield return new object[] { BrickScope.Namespace, BrickDependencyLayer.Configuration, BrickDependencyStrength.Inferred, BrickEvidenceLevel.ConfigurationDeclared };
        yield return new object[] { BrickScope.Assembly, BrickDependencyLayer.Visibility, BrickDependencyStrength.Indirect, BrickEvidenceLevel.AnalyzerInferred };
        yield return new object[] { BrickScope.Global, BrickDependencyLayer.Runtime, BrickDependencyStrength.Inferred, BrickEvidenceLevel.RuntimeInferred };
    }

    public static IEnumerable<object[]> ViolationProjectionCases()
    {
        yield return new object[] { BrickViolationKind.DependencyRule, BrickViolationState.Active, BrickSeverity.Error, BrickScope.Type, BrickDependencyLayer.Static, BrickEvidenceLevel.CompilerConfirmed };
        yield return new object[] { BrickViolationKind.RequiredDependency, BrickViolationState.Active, BrickSeverity.Warning, BrickScope.Namespace, BrickDependencyLayer.Configuration, BrickEvidenceLevel.ConfigurationDeclared };
        yield return new object[] { BrickViolationKind.Suppression, BrickViolationState.ExpiredSuppression, BrickSeverity.Info, BrickScope.Assembly, BrickDependencyLayer.Visibility, BrickEvidenceLevel.AnalyzerInferred };
        yield return new object[] { BrickViolationKind.Baseline, BrickViolationState.Baselined, BrickSeverity.Info, BrickScope.Global, BrickDependencyLayer.Runtime, BrickEvidenceLevel.RuntimeInferred };
    }

    public static IEnumerable<object[]> PolicyDefaultCases()
    {
        yield return new object[] { BrickPermissionDefault.Allow, BrickEnforcementMode.Disabled };
        yield return new object[] { BrickPermissionDefault.Allow, BrickEnforcementMode.Document };
        yield return new object[] { BrickPermissionDefault.Deny, BrickEnforcementMode.Analyze };
        yield return new object[] { BrickPermissionDefault.Deny, BrickEnforcementMode.Enforce };
    }

    public static IEnumerable<object[]> ReflectionEvidenceCases()
    {
        yield return new object[] { BrickEvidenceLevel.Unknown, BrickReflectionConfidence.Low };
        yield return new object[] { BrickEvidenceLevel.AnalyzerInferred, BrickReflectionConfidence.Medium };
        yield return new object[] { BrickEvidenceLevel.CompilerConfirmed, BrickReflectionConfidence.High };
    }

    public static IEnumerable<object[]> ElementClassificationCases()
    {
        yield return new object[] { BrickElementKind.Type, BrickElementOrigin.Source, BrickElementSource.Code };
        yield return new object[] { BrickElementKind.DependencyRegistration, BrickElementOrigin.Metadata, BrickElementSource.Configuration };
        yield return new object[] { BrickElementKind.ExternalReference, BrickElementOrigin.External, BrickElementSource.Import };
        yield return new object[] { BrickElementKind.Unknown, BrickElementOrigin.Unknown, BrickElementSource.Unknown };
    }

    public static IEnumerable<object[]> RoleAssignmentCases()
    {
        yield return new object[] { BrickAssignmentMode.DirectAttribute, BrickAssignmentSource.SourceAttribute, BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.Direct, BrickAssignmentBehavior.Apply };
        yield return new object[] { BrickAssignmentMode.ExternalConfiguration, BrickAssignmentSource.PolicyFile, BrickAssignmentSpecificity.Namespace, BrickAssignmentAuthority.External, BrickAssignmentBehavior.Suppress };
        yield return new object[] { BrickAssignmentMode.Convention, BrickAssignmentSource.Convention, BrickAssignmentSpecificity.Convention, BrickAssignmentAuthority.Derived, BrickAssignmentBehavior.Apply };
        yield return new object[] { BrickAssignmentMode.AliasMapping, BrickAssignmentSource.AliasMapping, BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.Alias, BrickAssignmentBehavior.Apply };
    }

    public static IEnumerable<object[]> PolicyImportCases()
    {
        yield return new object[] { BrickPolicyImportMode.Import, BrickPermissionDefault.Allow, BrickEnforcementMode.Document };
        yield return new object[] { BrickPolicyImportMode.Extend, BrickPermissionDefault.Deny, BrickEnforcementMode.Analyze };
        yield return new object[] { BrickPolicyImportMode.Override, BrickPermissionDefault.Deny, BrickEnforcementMode.Enforce };
        yield return new object[] { BrickPolicyImportMode.Disable, BrickPermissionDefault.Allow, BrickEnforcementMode.Disabled };
        yield return new object[] { BrickPolicyImportMode.Narrow, BrickPermissionDefault.Deny, BrickEnforcementMode.Enforce };
    }

    public static IEnumerable<object[]> GovernanceStatusCases()
    {
        yield return new object[] { BrickGovernanceArea.PolicyOwnership, BrickGovernanceRequirementStatus.Satisfied, true, false, BrickGovernanceAreaStatus.Compliant };
        yield return new object[] { BrickGovernanceArea.ExceptionHandling, BrickGovernanceRequirementStatus.Missing, true, false, BrickGovernanceAreaStatus.NonCompliant };
        yield return new object[] { BrickGovernanceArea.RolePackEvolution, BrickGovernanceRequirementStatus.Missing, true, true, BrickGovernanceAreaStatus.Partial };
        yield return new object[] { BrickGovernanceArea.CompatibilityExpectations, BrickGovernanceRequirementStatus.NotApplicable, false, false, BrickGovernanceAreaStatus.Compliant };
    }

    public static IEnumerable<object[]> ConformanceStatusCases()
    {
        yield return new object[] { BrickConformanceLevel.Marking, BrickConformanceCapabilityStatus.Satisfied, true, false, BrickConformanceLevelStatus.Achieved };
        yield return new object[] { BrickConformanceLevel.StaticValidation, BrickConformanceCapabilityStatus.Missing, true, false, BrickConformanceLevelStatus.NotStarted };
        yield return new object[] { BrickConformanceLevel.RuntimeAwareAnalysis, BrickConformanceCapabilityStatus.Missing, true, true, BrickConformanceLevelStatus.Partial };
        yield return new object[] { BrickConformanceLevel.IntegrationAndAugmentation, BrickConformanceCapabilityStatus.NotApplicable, false, false, BrickConformanceLevelStatus.Achieved };
    }

    public static IEnumerable<object[]> RoadmapStatusCases()
    {
        yield return new object[] { BrickRoadmapStage.V1, BrickRoadmapItemStatus.Completed, true, false, BrickRoadmapStageStatus.Complete };
        yield return new object[] { BrickRoadmapStage.V1_1, BrickRoadmapItemStatus.Missing, true, false, BrickRoadmapStageStatus.NotStarted };
        yield return new object[] { BrickRoadmapStage.V1_2, BrickRoadmapItemStatus.Missing, true, true, BrickRoadmapStageStatus.Partial };
        yield return new object[] { BrickRoadmapStage.V2, BrickRoadmapItemStatus.NotRequired, false, false, BrickRoadmapStageStatus.Complete };
    }

    public static IEnumerable<object[]> BenchmarkStatusCases()
    {
        yield return new object[] { BrickBenchmarkSubject.RuleEvaluation, false, 0L, 100L, true, 200L, BrickBenchmarkStatus.WithinBudget, BrickBenchmarkComparisonStatus.NoBaseline };
        yield return new object[] { BrickBenchmarkSubject.RoleResolution, true, 100L, 100L, true, 200L, BrickBenchmarkStatus.WithinBudget, BrickBenchmarkComparisonStatus.Stable };
        yield return new object[] { BrickBenchmarkSubject.PolicyComposition, true, 100L, 80L, true, 200L, BrickBenchmarkStatus.WithinBudget, BrickBenchmarkComparisonStatus.Improved };
        yield return new object[] { BrickBenchmarkSubject.ViolationProjection, true, 100L, 120L, true, 110L, BrickBenchmarkStatus.OverBudget, BrickBenchmarkComparisonStatus.Regressed };
        yield return new object[] { BrickBenchmarkSubject.RuntimeDependencyEvaluation, false, 0L, 100L, false, 0L, BrickBenchmarkStatus.NotBudgeted, BrickBenchmarkComparisonStatus.NoBaseline };
        yield return new object[] { BrickBenchmarkSubject.ReportSerialization, true, 100L, 95L, false, 0L, BrickBenchmarkStatus.NotBudgeted, BrickBenchmarkComparisonStatus.Stable };
    }

    public static IEnumerable<object[]> AiRunConfigurationCases()
    {
        yield return new object[] { BrickAiMode.Off, BrickAiCommentFormat.Markdown, false, false, false, false, false };
        yield return new object[] { BrickAiMode.Explain, BrickAiCommentFormat.Markdown, false, true, true, false, false };
        yield return new object[] { BrickAiMode.Explain, BrickAiCommentFormat.Json, false, true, false, true, false };
        yield return new object[] { BrickAiMode.SuggestRules, BrickAiCommentFormat.Both, true, true, true, true, true };
    }

    private static BrickRuleProposalEvidence CompleteEvidence() =>
        new(
            "Source role depends on target role.",
            new[] { "Application depends on contract." },
            new[] { "Application depends on infrastructure implementation." },
            new[] { "Generated code may require a separate rule." },
            new[] { "Billing.Application" },
            "One team must move direct implementation dependencies behind contracts.");

    private static string Pair(string left, string right) =>
        string.CompareOrdinal(left, right) <= 0
            ? $"{left} + {right}"
            : $"{right} + {left}";
}
