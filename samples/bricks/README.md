# Bricks Samples

This folder separates Bricks examples by intent.

## Sample Groups

| Folder | Use when |
| --- | --- |
| `implementation-samples/` | You want to learn how to model architecture with `NMolecules.Bricks`: roles, rules, dependencies, policies, member contracts, DDD slices and domain-language kits. |
| `analyzer-samples/` | You want projects that intentionally produce analyzer diagnostics for Bricks usage mistakes. |
| `implementation-samples/implementation-analyzer-samples/` | You want analyzer samples for teams that implement their own Brick vocabulary or domain-language kit and need diagnostics for that implementation. |
| `docs/` | You want planning material and historical sample-roadmap notes. |

## Build Boundary

The green solution path includes `implementation-samples/` projects only. Analyzer samples are intentionally kept outside the green solution path because they are supposed to fail with diagnostics.

```powershell
dotnet test .\nMolecules.BrickExamples.slnx --no-restore -v minimal
```

## Diagnostic Boundary

Analyzer samples are not tutorials for correct domain modeling. They are fixtures for seeing diagnostics in Visual Studio, VS Code and command-line builds.

Implementation samples are the opposite: they should build green and explain useful Bricks modeling patterns.
