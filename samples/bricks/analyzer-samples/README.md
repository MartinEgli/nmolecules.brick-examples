# Brick Analyzer Samples

These projects are diagnostic fixtures. They intentionally contain invalid Bricks metadata or invalid dependency declarations so analyzers can be inspected in Visual Studio, VS Code and command-line builds.

## Projects

| Folder | Diagnostic focus | Expected build result |
| --- | --- | --- |
| `metadata-validation/` | Empty policy, role, rule, dependency and invalid member-contract metadata. | Passes with 14 `XMoleculesBricks0002` warnings. |
| `dependency-rule-evaluation/` | Forbid/require dependency rules in a DDD-like Orders use case. | Fails with `XMoleculesBricks0001` errors. |
| `generic-role-combinations/` | Generic, non-DDD role pairs such as UI-to-database, workflow-to-vendor-SDK, feature-internal, production-to-test and generated-to-policy boundaries. | Fails with 6 `XMoleculesBricks0001` errors. |
| `member-contract-validation/` | Direct and custom-attribute member contracts. | Fails with `XMoleculesBricks0003` to `XMoleculesBricks0010` errors. |
| `brick-policy-violations/` | Invalid role/rule/dependency declarations and forbidden dependency examples for Bricks usage. | Fails with integration-analyzer Bricks diagnostics. |

## Rule

Analyzer samples are intentionally outside `nMolecules.BrickExamples.slnx`. A failing diagnostic in this area is expected behavior, not a broken green sample.
