# AI Governance Collaboration

This sample shows how Bricks can work with AI without handing architectural
authority to AI.

## Motivation

Architecture teams increasingly ask AI agents to review code, explain analyzer
diagnostics and suggest improvements. That is useful only when the source of
truth stays deterministic: Bricks policies, analyzer diagnostics, tests,
benchmarks and reviewed governance records decide what is valid.

## User Benefit

- Developers get clearer remediation guidance for deterministic Bricks findings.
- Architects see repeated drift as candidate rule proposals instead of isolated comments.
- Analyzer maintainers receive positive examples, negative examples, false-positive risks and rollout notes before creating a diagnostic.
- Governance owners can prove that AI did not silently mutate policy, suppress violations or enable build-breaking enforcement.

## Sample Flow

1. Bricks or an analyzer produces a deterministic violation.
2. AI creates an explanatory `BrickAiCommentDocument` with remediation options.
3. The same document can be rendered as JSON or Markdown for CI, IDE or PR adapters.
4. AI may create a `BrickRuleProposal` when repeated findings suggest a new analyzer rule.
5. The proposal is persisted in a `BrickRuleProposalQueue`.
6. A human review records a `BrickRuleProposalReview`; only the review workflow can promote the proposal into a deterministic `BrickRule`.
7. Analyzer tests cover positive fixtures, negative fixtures and false-positive risks.
8. Benchmark reports guard the feedback loop before the rule becomes a warning or enforced diagnostic.
9. A `BrickGovernanceReport` records that deterministic enforcement, human review, exception handling and compatibility checks are satisfied.

## Attribute Policy Flow

`AiGovernanceCollaborationSample.CreateAttributePolicyReviewPacket()` shows the
same governance flow when the policy is declared through attributes:

- `[assembly: Policy(...)]` defines the policy header.
- `[assembly: PolicyImport(...)]` imports platform defaults.
- `[assembly: RoleCombination(...)]` declares incompatible role combinations.
- `[assembly: Rule(...)]` declares dependency rules.
- `[assembly: Dependency(...)]` provides deterministic example evidence.
- Role marker attributes such as `[DddAggregateRoot]` and `[DddInfrastructure]`
  assign roles to code elements.

AI can use that attribute policy as source material for comments and analyzer
coverage proposals, but the attributes, analyzer fixtures and build output stay
the authoritative sources.

## Important Boundary

AI is advisory in this sample. It can explain, cluster and propose. It cannot:

- activate enforcement without review
- mutate policy silently
- create suppressions or baselines silently
- replace analyzer fixtures or benchmark evidence
- convert an assumption into accepted architecture knowledge

## Code

- `AiGovernanceCollaborationSample.cs` creates the review packet, AI comments, Markdown output, analyzer-extension proposal, proposal queue, reviewed promotion result, governance report and serialized architecture-review artifact.
- `CreateAttributePolicyReviewPacket()` derives the same AI review shape from the DDD attribute policy in `../ddd-building/DddBrickRules.Assembly.cs`.
- `../function-coverage/BenchmarkAndAiExamples.cs` supplies the benchmark artifact, CI configuration, proposal queue and reviewed-promotion examples used as guardrails.

## Recommended AgentSkills Usage

For current work in this repository:

- Use `dotnet-engineering` for C#, tests, analyzers, benchmarks and performance work.
- Use `software-architecture` for Bricks design, Clean Architecture boundaries and Clean AI workflow boundaries.
- Use `domain-driven-design` when Bricks roles are used to model aggregates, value objects, repositories, factories and services.
- Use `mournival-architecture` when an AI-generated rule proposal or architecture claim needs governance acceptance.
- Use `skill-author` when creating or refining a dedicated xMolecules/nMolecules skill.

The AgentSkills superrepo already has the right general-purpose skills, but a
dedicated `xmolecules-nmolecules` skill is still recommended because this
workspace now has recurring project-specific rules: Bricks concepts, analyzer
extension workflow, benchmark gates, TDD expectations, sample coverage matrices,
submodule updates and provenance-preserving documentation.
