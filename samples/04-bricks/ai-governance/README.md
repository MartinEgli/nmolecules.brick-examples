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
3. AI may create a `BrickRuleProposal` when repeated findings suggest a new analyzer rule.
4. The proposal stays in `Candidate` or `Observing` until human review accepts the evidence.
5. Analyzer tests cover positive fixtures, negative fixtures and false-positive risks.
6. Benchmark reports guard the feedback loop before the rule becomes a warning or enforced diagnostic.
7. A `BrickGovernanceReport` records that deterministic enforcement, human review, exception handling and compatibility checks are satisfied.

## Important Boundary

AI is advisory in this sample. It can explain, cluster and propose. It cannot:

- activate enforcement without review
- mutate policy silently
- create suppressions or baselines silently
- replace analyzer fixtures or benchmark evidence
- convert an assumption into accepted architecture knowledge

## Code

- `AiGovernanceCollaborationSample.cs` creates the review packet, AI comments, analyzer-extension proposal, governance report and serialized architecture-review artifact.
- `../function-coverage/BenchmarkAndAiExamples.cs` supplies the benchmark artifact used as a performance guardrail.

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
