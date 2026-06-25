using System;
using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;
using Samples.Block04.Bricks.DddBuilding;
using Samples.Block04.Bricks.FunctionCoverage;

namespace Samples.Block04.Bricks.AiGovernance;


/// <summary>
/// Shows a safe collaboration pattern where AI can explain deterministic Bricks
/// findings and propose analyzer improvements without becoming the source of
/// architectural truth.
/// </summary>
internal static class AiGovernanceCollaborationSample
{
    public static AiGovernanceReviewPacket CreateReviewPacket()
    {
        var trustBoundary = new BrickAiTrustBoundary(
            BrickAiMode.SuggestRules,
            BrickAiCommentFormat.Both,
            allowRuleProposal: true,
            allowAutoEnforcement: false,
            allowSilentPolicyMutation: false);

        return new AiGovernanceReviewPacket(
            trustBoundary,
            CreateDeterministicFindingComment(),
            SuggestAnalyzerExtension(),
            AssessAiGovernance());
    }

    public static AiAttributePolicyReviewPacket CreateAttributePolicyReviewPacket()
    {
        var attributeConfiguration = DddAttributeOnlyConfigurationExample.ReadConfigurationFromAttributes();
        var attributePolicy = DddAttributeOnlyConfigurationExample.BuildPolicyFromAssemblyAttributes();
        var attributeViolations = DddAttributeOnlyConfigurationExample.EvaluateExampleDependency();
        var comments = CreateCommentsFromAttributePolicyViolations(attributeViolations);

        return new AiAttributePolicyReviewPacket(
            attributePolicy.Id.Value,
            attributeConfiguration.Rules.Select(rule => rule.Id).ToArray(),
            attributeConfiguration.Dependencies.Select(dependency => dependency.Id).ToArray(),
            comments,
            SuggestAttributePolicyAnalyzerExtension(attributeConfiguration),
            AssessAiGovernance());
    }

    public static BrickAiCommentDocument CreateDeterministicFindingComment()
    {
        var source = SampleBrickModel.Type("Ordering.Domain.OrderAggregate");
        var target = SampleBrickModel.Type("Ordering.Infrastructure.SqlOrderRepository");
        var violation = SampleBrickModel.Violation(source, target);
        var moveDependency = new BrickRemediationOption(
            "move-persistence-behind-port",
            BrickRemediationKind.AddPortAndAdapter,
            "Introduce a repository port in the domain or application boundary and move SQL access behind an adapter.",
            "Use when domain logic needs persistence behavior but must not know SQL infrastructure details.",
            BrickRemediationRisk.Low,
            isPreferred: true);
        var adjustPolicy = new BrickRemediationOption(
            "review-policy-scope",
            BrickRemediationKind.AdjustPolicy,
            "Review whether the rule is too broad before changing policy.",
            "Use only when positive and negative examples show that the current rule blocks a legitimate architecture pattern.",
            BrickRemediationRisk.High,
            isPreferred: false);
        var comment = new BrickAiViolationComment(
            violation,
            "Domain aggregate reaches into infrastructure.",
            "The deterministic rule protects the domain model from persistence and framework coupling.",
            new[] { moveDependency, adjustPolicy },
            moveDependency,
            new[]
            {
                "Create or reuse an IOrderRepository contract outside infrastructure.",
                "Move SqlOrderRepository construction to the composition root.",
                "Do not add a suppression unless an owner, reason and expiry are reviewed."
            },
            "Suppressions require owner, rationale and expiry; AI must never create them automatically.");

        return new BrickAiCommentDocument(SampleBrickModel.GeneratedAt, new[] { comment });
    }

    public static BrickRuleProposal SuggestAnalyzerExtension()
    {
        var evidence = new BrickRuleProposalEvidence(
            "Repeated reviews show AI agents can suggest new analyzer rules when the same architecture drift appears across multiple contexts.",
            new[]
            {
                "Ordering.Domain.OrderAggregate -> Ordering.Infrastructure.SqlOrderRepository",
                "Billing.Domain.ContractPolicy -> Billing.Infrastructure.SqlContractRepository"
            },
            new[]
            {
                "CompositionRoot -> Ordering.Infrastructure.SqlOrderRepository",
                "InfrastructureModule -> Billing.Infrastructure.SqlContractRepository"
            },
            new[]
            {
                "Generated dependency-injection wiring can look like direct construction.",
                "Migration adapters may temporarily bridge old and new module boundaries."
            },
            new[] { "Ordering", "Billing", "DDD building block samples" },
            "Start in observing mode, add analyzer test fixtures, then promote only after false positives are understood.");

        return new BrickRuleProposal(
            "ai-observed-domain-to-infrastructure-drift",
            "Domain roles should not depend on infrastructure roles",
            "AI can cluster repeated deterministic findings into a candidate analyzer rule, but humans must validate examples, risks and migration impact.",
            BrickRoleSelector.From("Domain"),
            BrickRoleSelector.From("Infrastructure"),
            BrickDependencyKindId.From("StaticReference"),
            BrickDecision.Deny,
            BrickSeverity.Warning,
            evidence,
            BrickRuleLifecycleState.Candidate);
    }

    public static BrickRuleProposal SuggestAttributePolicyAnalyzerExtension(
        DddAttributeOnlyConfiguration attributeConfiguration)
    {
        var evidence = new BrickRuleProposalEvidence(
            "The DDD sample declares policy, imports, role combinations, rules and dependency evidence through attributes only.",
            attributeConfiguration.Rules.Select(rule => $"{rule.Id}: {rule.SourceRole} -> {rule.TargetRole} ({rule.Mode})"),
            new[]
            {
                "CompositionRoot -> InMemoryContractRepository",
                $"{DddBrickRoles.ApplicationService} -> {DddBrickRoles.Repository}"
            },
            new[]
            {
                "Assembly-level dependency evidence may be incomplete while a team migrates from generated analyzer facts.",
                "Attribute aliases can intentionally map several domain-specific markers to one Bricks role."
            },
            new[] { DddBrickRules.DddPolicy, "samples/bricks/ddd-building" },
            "Keep the attribute policy as source material, add analyzer fixtures for each RuleAttribute, and promote diagnostics only after fixture review.");

        return new BrickRuleProposal(
            "ai-attribute-policy-fixtures",
            "Attribute-declared policies should have analyzer fixtures",
            "AI can inspect attribute-declared policies and propose missing analyzer coverage, but fixtures and deterministic diagnostics must be reviewed by maintainers.",
            BrickRoleSelector.From(DddBrickRoles.AggregateRoot),
            BrickRoleSelector.From(DddBrickRoles.Infrastructure),
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            BrickDecision.Deny,
            BrickSeverity.Warning,
            evidence,
            BrickRuleLifecycleState.Candidate);
    }

    public static BrickGovernanceReport AssessAiGovernance()
    {
        var areas = new[]
        {
            Area(
                BrickGovernanceArea.PolicyOwnership,
                "AI-assisted architecture governance",
                "AI output is reviewed as architecture input, not as accepted policy.",
                Requirement(
                    "ai-deterministic-source-of-truth",
                    BrickGovernanceArea.PolicyOwnership,
                    "Deterministic source of truth",
                    "Bricks rules, analyzer diagnostics and tests remain authoritative."),
                Requirement(
                    "ai-human-rule-review",
                    BrickGovernanceArea.PolicyOwnership,
                    "Human rule review",
                    "AI-generated rule proposals require architecture-owner review before promotion.")),
            Area(
                BrickGovernanceArea.ExceptionHandling,
                "AI exception handling",
                "AI must not hide policy failures through silent suppressions or baselines.",
                Requirement(
                    "ai-no-silent-suppression",
                    BrickGovernanceArea.ExceptionHandling,
                    "No silent suppression",
                    "Suppressions require owner, reason and expiry."),
                Requirement(
                    "ai-no-silent-baseline",
                    BrickGovernanceArea.ExceptionHandling,
                    "No silent baseline",
                    "Baseline entries are explicit migration decisions.")),
            Area(
                BrickGovernanceArea.CompatibilityExpectations,
                "Analyzer extension readiness",
                "Candidate analyzer extensions need fixtures, benchmarks and rollout evidence.",
                Requirement(
                    "ai-analyzer-test-corpus",
                    BrickGovernanceArea.CompatibilityExpectations,
                    "Analyzer test corpus",
                    "Positive, negative and false-positive fixtures exist before enabling diagnostics."),
                Requirement(
                    "ai-benchmark-guardrail",
                    BrickGovernanceArea.CompatibilityExpectations,
                    "Benchmark guardrail",
                    "Central analyzer and Bricks operations stay inside reviewed performance budgets."))
        };

        return new BrickGovernanceReport(
            SampleBrickModel.GeneratedAt,
            areas.Select(area =>
                new BrickGovernanceAreaAssessment(
                    area,
                    area.Requirements.Select(requirement =>
                        new BrickGovernanceRequirementResult(
                            requirement,
                            BrickGovernanceRequirementStatus.Satisfied,
                            "Covered by this AI governance collaboration sample.")))));
    }

    public static string SerializeForArchitectureReview()
    {
        var packet = CreateReviewPacket();
        var attributePacket = CreateAttributePolicyReviewPacket();
        var aiComments = BrickAiCommentJsonSerializer.Serialize(packet.Comments);
        var attributeComments = BrickAiCommentJsonSerializer.Serialize(attributePacket.Comments);
        var governance = BrickGovernanceReportJsonSerializer.Serialize(packet.Governance);
        var benchmark = BrickBenchmarkReportJsonSerializer.Serialize(BenchmarkAndAiExamples.BenchmarkCentralOperations());

        return string.Join(
            Environment.NewLine,
            $"mode={packet.TrustBoundary.Mode}",
            $"canActivateWithoutReview={packet.TrustBoundary.CanActivateRuleWithoutReview}",
            $"proposal={packet.Proposal.ProposalId}:{packet.Proposal.LifecycleState}:{packet.Proposal.HasRequiredEvidence}",
            $"attributePolicy={attributePacket.PolicyId}",
            $"attributePolicyProposal={attributePacket.Proposal.ProposalId}:{attributePacket.Proposal.LifecycleState}:{attributePacket.Proposal.HasRequiredEvidence}",
            aiComments,
            attributeComments,
            governance,
            benchmark);
    }

    private static BrickAiCommentDocument CreateCommentsFromAttributePolicyViolations(
        IReadOnlyList<BrickViolation> violations)
    {
        var comments = violations.Select(violation =>
        {
            var introducePort = new BrickRemediationOption(
                "attribute-policy-introduce-port",
                BrickRemediationKind.AddPortAndAdapter,
                "Use the attribute-declared rule as the source of truth and move the dependency behind a DDD port.",
                "Use when the violation comes from an assembly-level RuleAttribute and the code should follow that policy.",
                BrickRemediationRisk.Low,
                isPreferred: true);
            var reviewRule = new BrickRemediationOption(
                "attribute-policy-review-rule",
                BrickRemediationKind.AdjustPolicy,
                "Review the RuleAttribute only if positive and negative fixtures show that the policy is wrong.",
                "Use when the attribute policy blocks a valid architecture case and analyzer fixtures prove it.",
                BrickRemediationRisk.High,
                isPreferred: false);

            return new BrickAiViolationComment(
                violation,
                "Attribute-declared policy produced a deterministic Bricks violation.",
                "The assembly-level RuleAttribute is reviewable code and remains the policy source for AI guidance.",
                new[] { introducePort, reviewRule },
                introducePort,
                new[]
                {
                    "Read the matching [assembly: Rule(...)] declaration before editing code.",
                    "Add or update analyzer fixtures for the attribute policy before promoting a diagnostic.",
                    "Keep AI output as a proposal until the architecture owner reviews the rule and examples."
                },
                "Do not suppress an attribute-policy violation without owner, rationale, expiry and fixture evidence.");
        });

        return new BrickAiCommentDocument(SampleBrickModel.GeneratedAt, comments);
    }

    private static BrickGovernanceAreaDefinition Area(
        BrickGovernanceArea area,
        string displayName,
        string description,
        params BrickGovernanceRequirement[] requirements) =>
        new BrickGovernanceAreaDefinition(area, displayName, description, requirements);

    private static BrickGovernanceRequirement Requirement(
        string id,
        BrickGovernanceArea area,
        string displayName,
        string rationale) =>
        new BrickGovernanceRequirement(id, area, displayName, required: true, rationale);
}
