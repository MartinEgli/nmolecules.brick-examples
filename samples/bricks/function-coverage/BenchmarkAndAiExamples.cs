using System;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.FunctionCoverage;

/// <summary>
/// Demonstrates feedback-loop performance budgets and AI-assisted explanations
/// that remain traceable to deterministic Bricks violations.
/// </summary>
internal static class BenchmarkAndAiExamples
{
    public static BrickBenchmarkReport BenchmarkCentralOperations()
    {
        var results = BrickBuiltInBenchmarkCases.All.Select(benchmarkCase =>
            BrickBenchmarkRunner.Run(
                benchmarkCase,
                () =>
                {
                    _ = PolicyAndResolutionExamples.EvaluateBillingArchitecture();
                },
                iterations: 1,
                clock: new FixedBenchmarkClock(TimeSpan.FromTicks(benchmarkCase.OperationCount * 2))));

        return new BrickBenchmarkReport(SampleBrickModel.GeneratedAt, results);
    }

    public static BrickBenchmarkComparisonReport CompareBenchmarkRuns()
    {
        var baseline = new BrickBenchmarkReport(
            SampleBrickModel.GeneratedAt.AddDays(-1),
            new[] { Result("rule-evaluation", 100), Result("report-serialization", 100) });
        var current = new BrickBenchmarkReport(
            SampleBrickModel.GeneratedAt,
            new[] { Result("rule-evaluation", 108), Result("report-serialization", 130), Result("role-resolution", 50) });
        var threshold = new BrickBenchmarkComparisonThreshold(
            maxAllowedSlowdownRatio: 0.10,
            minSignificantImprovementRatio: 0.05,
            "CI should fail only on meaningful feedback-loop regressions.");

        return BrickBenchmarkComparisonReport.Compare(baseline, current, threshold);
    }

    public static string SerializeBenchmarkArtifacts()
    {
        var benchmarkJson = BrickBenchmarkReportJsonSerializer.Serialize(BenchmarkCentralOperations());
        var comparisonJson = BrickBenchmarkComparisonReportJsonSerializer.Serialize(CompareBenchmarkRuns());
        return benchmarkJson + Environment.NewLine + comparisonJson;
    }

    public static BrickAiCommentDocument CreateAiReviewComments()
    {
        var source = SampleBrickModel.Type("Billing.Application.ContractApplicationService");
        var target = SampleBrickModel.Type("Billing.Infrastructure.SqlContractRepository");
        var violation = SampleBrickModel.Violation(source, target);
        var introduceContract = new BrickRemediationOption(
            "introduce-repository-contract",
            BrickRemediationKind.IntroduceContract,
            "Introduce IContractRepository in the application boundary.",
            "Use when application logic needs persistence behavior without owning SQL details.",
            BrickRemediationRisk.Low,
            isPreferred: true);
        var suppression = new BrickRemediationOption(
            "time-boxed-suppression",
            BrickRemediationKind.AddSuppression,
            "Add a reviewed suppression with owner and expiry.",
            "Use only when migration cannot be completed in the current slice.",
            BrickRemediationRisk.High,
            isPreferred: false);
        var comment = new BrickAiViolationComment(
            violation,
            "Application service depends on infrastructure implementation.",
            "Moving the dependency behind a contract keeps use-case orchestration testable and replaceable.",
            new[] { introduceContract, suppression },
            introduceContract,
            new[] { "Prefer constructor-injected contracts.", "Do not create suppressions automatically." },
            "Suppress only with owner, justification and expiry.");

        return new BrickAiCommentDocument(SampleBrickModel.GeneratedAt, new[] { comment });
    }

    public static BrickRuleProposal CreateRuleProposalForReview()
    {
        var evidence = new BrickRuleProposalEvidence(
            "Application services repeatedly create SQL repositories directly.",
            new[] { "ContractApplicationService -> SqlContractRepository" },
            new[] { "CompositionRoot -> SqlContractRepository" },
            new[] { "Generated DI wiring may look similar and should be excluded." },
            new[] { "Billing.Application", "Billing.Infrastructure" },
            "Move creation into the composition root and depend on an application contract.");

        return new BrickRuleProposal(
            "billing-no-app-to-sql",
            "Billing application must not create SQL repositories",
            "Application use cases stay independent from infrastructure details.",
            BrickRoleSelector.From("Billing.Application"),
            BrickRoleSelector.From("Billing.Infrastructure"),
            BrickDependencyKindId.From("ObjectCreation"),
            BrickDecision.Deny,
            BrickSeverity.Warning,
            evidence,
            BrickRuleLifecycleState.Candidate);
    }

    public static string SerializeAiArtifacts()
    {
        var boundary = new BrickAiTrustBoundary(
            BrickAiMode.SuggestRules,
            BrickAiCommentFormat.Both,
            allowRuleProposal: true,
            allowAutoEnforcement: false,
            allowSilentPolicyMutation: false);
        var comments = BrickAiCommentJsonSerializer.Serialize(CreateAiReviewComments());
        var proposal = CreateRuleProposalForReview();

        return $"{boundary.Mode}:{boundary.CommentFormat}:{proposal.LifecycleState}:{comments}";
    }

    private static BrickBenchmarkResult Result(string id, long elapsedPerOperationTicks)
    {
        var benchmarkCase = new BrickBenchmarkCase(
            id,
            id,
            BrickBenchmarkSubject.RuleEvaluation,
            operationCount: 1,
            new BrickBenchmarkBudget(TimeSpan.FromTicks(1000), "Example budget."));

        return new BrickBenchmarkResult(benchmarkCase, iterations: 1, totalOperations: 1, TimeSpan.FromTicks(elapsedPerOperationTicks));
    }

    private sealed class FixedBenchmarkClock : IBrickBenchmarkClock
    {
        private readonly TimeSpan _elapsed;

        public FixedBenchmarkClock(TimeSpan elapsed)
        {
            _elapsed = elapsed;
        }

        public TimeSpan Measure(Action operation)
        {
            operation();
            return _elapsed;
        }
    }
}
