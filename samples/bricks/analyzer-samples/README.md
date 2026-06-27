# Brick Analyzer Samples

These projects are diagnostic fixtures. They intentionally contain invalid Bricks metadata or invalid dependency declarations so analyzers can be inspected in Visual Studio, VS Code and command-line builds.

## Projects

| Folder | Diagnostic focus |
| --- | --- |
| `brick-policy-violations/` | Invalid role/rule/dependency declarations and forbidden dependency examples for Bricks usage. |

## Rule

Analyzer samples are intentionally outside `nMolecules.BrickExamples.slnx`. A failing diagnostic in this area is expected behavior, not a broken green sample.
