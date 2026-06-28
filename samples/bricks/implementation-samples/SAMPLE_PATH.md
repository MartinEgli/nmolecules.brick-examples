# Bricks Sample Path

Status: June 27, 2026

The Bricks samples are organized as a simple-to-complex teaching path. DDD is
one supported modeling style, not the center of the sample surface.

## 1. Getting Started

Use these first:

- `function-coverage/SampleBrickModel.cs`
- `class-contracts/`
- `interface-contracts/`

Purpose:

- create elements
- assign roles
- model simple contracts
- evaluate outcomes without analyzer failures

## 2. Role Modeling

Use these next:

- `function-coverage/PolicyAndResolutionExamples.cs`
- `domain-language-kits/`
- `generic-role-combinations/` in analyzer samples

Purpose:

- model direct roles
- model aliases
- model role combinations
- use logical architecture role names such as `UserInterfaceEndpoint`,
  `DatabaseAdapter`, `BusinessWorkflow`, `VendorSdkAdapter`,
  `ProductionRuntimeComponent`, `TestHarness`, `GeneratedClient` and
  `BusinessPolicy`

## 3. Dependency Rules

Use these after roles are clear:

- `source-target-cases/`
- `rule-filters/`
- `analyzer-samples/dependency-rule-evaluation/`

Purpose:

- model source and target shapes
- distinguish static, declared and runtime-facing dependency evidence
- prove pass and violation paths

## 4. Member Contracts

Use these when a role implies required members:

- `member-contracts/`
- `analyzer-samples/member-contract-validation/`

Purpose:

- require exactly one marker
- require all markers
- require a fixed marker count
- require an exclusive member choice

## 5. Policy Composition

Use these when teams need reusable rule sets:

- `policy-variant-cases/`
- `function-coverage/ConfigurationExamples.cs`
- `function-coverage/PolicyAndResolutionExamples.cs`

Purpose:

- compare attribute-only, code, JSON and composed policies
- understand import modes
- understand configuration source precedence

## 6. Runtime, Governance And AI Assistance

Use these after deterministic policy is understood:

- `function-coverage/RuntimeAndExportExamples.cs`
- `function-coverage/GovernanceAndReadinessExamples.cs`
- `function-coverage/BenchmarkAndAiExamples.cs`
- `ai-governance/`

Purpose:

- report runtime wiring, visibility and reflection evidence
- track rollout maturity and dependency observability
- use AI for explanation and proposals while deterministic policy stays source
  of truth

## 7. Completion And Coverage Contracts

Use these when changing the Bricks public API or sample corpus:

- `coverage/bricks-api-family-coverage.json`
- `coverage/bricks-enum-variation-matrix.json`
- `tests/Samples.Block04.Bricks.Tests`

Purpose:

- keep every public Bricks source area tied to sample evidence
- keep every public enum value tied to meaningful variation rows
- fail the sample coverage tests when Dependencies, Rules, Profiles or any
  other Bricks family gains a new public type without a documented sample path

## Analyzer-Backed vs Concept-Only

Analyzer-backed samples either build cleanly under the normal sample project or
are isolated in `samples/bricks/analyzer-samples` with expected diagnostics.

Runtime-backed and concept-boundary samples stay in implementation samples and
must not break the green build. If a concept becomes analyzer-backed, the
roundtrip must gain the corresponding pass or violation check.
