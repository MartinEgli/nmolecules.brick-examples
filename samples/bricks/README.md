# Bricks Samples

This folder is the entry point for all `NMolecules.Bricks` examples. The samples
are organized by consumer use case, not by an old numbered block name.

## Project Map

| Project or folder | What it demonstrates | Why it exists |
| --- | --- | --- |
| `Samples.Block04.Bricks.csproj` | The green base sample assembly with shared billing and DDD helper types. | Keeps reusable sample vocabulary in one place while specialized projects stay small. |
| `Samples.Block04.Bricks.Violations.csproj` | Intentionally invalid Bricks declarations. | Lets analyzer diagnostics be explored without breaking the normal solution build. |
| `class-contracts/` | Class-scoped role contracts with valid and violating dependencies. | Shows when a class role must or must not depend on another class role. |
| `interface-contracts/` | Interface-targeted role contracts. | Shows repository/external gateway contracts as interface dependency targets. |
| `namespace-contracts/` | Namespace-level contracts through modeled namespace role assignments. | Shows namespace rules without pretending C# supports namespace attributes. |
| `member-contracts/` | Member-cardinality contracts split into small case projects. | Shows exactly-one, exact-count, all-members, XOR, range and forbidden-marker patterns. |
| `source-target-cases/` | Type, member, namespace, assembly, registration and external-reference dependency shapes. | Shows how the same evaluator behaves at different architecture boundaries. |
| `policy-variant-cases/` | Attribute-only, attribute multi-policy, code, JSON and composed policy variants. | Helps teams choose an authoring style and understand policy-correlation trade-offs. |
| `ddd-building/` | DDD aggregate, entity, value object, repository, factory and service roles. | Demonstrates a realistic domain slice built with Bricks attributes and policies. |
| `domain-language-kits/` | A reusable billing domain role/rule vocabulary. | Shows how teams can package their own architecture language. |
| `rule-filters/` | Required/excluded source, target and member name filters. | Shows how rule filters narrow diagnostics and improve messages. |
| `function-coverage/` | Broad public API coverage examples. | Maps Bricks API families to buildable examples and user benefits. |
| `ai-governance/` | AI-assisted review packets and analyzer-extension proposals. | Shows how AI can help while deterministic policy remains the source of truth. |
| `docs/` | Consumer sample roadmap and historical layout notes. | Keeps planning material separate from executable examples. |

## Contract Examples

The contract examples are intentionally split by scope:

| Scope | Folder | Backing mechanism |
| --- | --- | --- |
| Class | `class-contracts/` | `RoleAttribute` on classes plus `BrickRuleEvaluator` |
| Interface | `interface-contracts/` | `RoleAttribute` on interfaces plus `BrickRuleEvaluator` |
| Namespace | `namespace-contracts/` | modeled `BrickElementKind.Namespace` assignments from policy metadata |
| Member | `member-contracts/` | member marker attributes plus `BrickMemberCardinalityEvaluator` |

Each contract area contains at least one valid path and one violation path.

## Verification

The normal solution includes only green projects:

```powershell
dotnet test .\nMolecules.BrickExamples.slnx --no-restore -v minimal
```

The violation project is intentionally kept out of the solution's green path.
