# Brick Implementation Samples

These samples show how to implement architecture rules with `NMolecules.Bricks`.
They are green examples and belong to the normal build and test path.

## Project Map

| Project or folder | What it demonstrates |
| --- | --- |
| `Samples.Block04.Bricks.csproj` | Shared green sample assembly and reusable billing helper types. |
| `class-contracts/` | Class-scoped role contracts with valid and violating evaluation cases. |
| `interface-contracts/` | Interface-targeted role contracts for repository and gateway abstractions. |
| `namespace-contracts/` | Namespace-level contracts through modeled namespace role assignments. |
| `member-contracts/` | Member-cardinality contracts split into small projects. |
| `source-target-cases/` | Type, member, namespace, assembly, registration and external-reference dependency shapes. |
| `policy-variant-cases/` | Attribute-only, attribute multi-policy, code, JSON and composed policy variants. |
| `enum-coverage-cases/` | Public Bricks enum values as attribute-only and code examples. |
| `ddd-building/` | A DDD slice built with aggregate, entity, value object, repository, factory and service roles. |
| `domain-language-kits/` | A reusable billing domain role/rule vocabulary. |
| `rule-filters/` | Required/excluded source, target and member name filters. |
| `function-coverage/` | Public API coverage examples with motivation and user benefit. |
| `ai-governance/` | AI-assisted review packets where deterministic policy stays source of truth. |
| `implementation-analyzer-samples/` | Diagnostics for custom Brick implementation and domain-language-kit mistakes. |

## Rule

Implementation samples should compile green. If a case demonstrates a violation, it should do so through an evaluator result or a dedicated analyzer sample project, not by breaking the normal solution build.
