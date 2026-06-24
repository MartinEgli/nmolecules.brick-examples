# nmolecules.brick-examples

Standalone example repository for nMolecules Bricks.

## Source Provenance

The initial content was split out from the existing superrepo sample block:

- Source repository path: `nmolecules.examples/samples/04-bricks`
- Target repository path: `samples/04-bricks`
- Reason: keep Bricks examples usable as a focused repository while preserving the original teaching material and file layout.

The copied sample block demonstrates custom role and rule modeling with `NMolecules.Bricks`.

## Repository Layout

| Path | Purpose |
|---|---|
| `samples/04-bricks/Samples.Block04.Bricks.csproj` | Green Bricks sample project |
| `samples/04-bricks/Samples.Block04.Bricks.Violations.csproj` | Intentional violation project for analyzer diagnostics |
| `samples/04-bricks/domain-language-kits/billing` | Billing role/rule vocabulary plus domain walkthrough |
| `samples/04-bricks/member-contracts` | Member contract modeling examples |
| `samples/04-bricks/rule-filters` | Rule filter and custom diagnostic message examples |
| `samples/04-bricks/docs` | Consumer sample roadmap copied with the source block |

## Build

From this repository root inside the xMolecules superrepo:

```powershell
dotnet build .\nMolecules.BrickExamples.slnx -v minimal
```

The projects currently reference the local superrepo checkout:

- `../nmolecules/src/nMolecules.Bricks/nMolecules.Bricks.csproj`
- `../nmolecules-integrations/nmolecules-roslyn/src/nMolecules.Analyzers/...`

That keeps the examples aligned with the in-flight workspace sources. If this repository is cloned outside the superrepo, clone `nmolecules` and `nmolecules-integrations` as sibling directories or switch the references to package references.

## Intentional Violations

`Samples.Block04.Bricks.Violations.csproj` is expected to emit analyzer diagnostics. Use it to inspect invalid rule declarations and forbidden dependency examples; do not treat that project as part of the green build path.
