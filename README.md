# nmolecules.brick-examples

Standalone example repository for nMolecules Bricks.

## Source Provenance

The initial content was split out from the existing superrepo sample block:

- Source repository path: `nmolecules.examples/samples/bricks`
- Target repository path: `samples/bricks`
- Reason: keep Bricks examples usable as a focused repository while preserving the original teaching material and file layout.

The copied sample block demonstrates custom role and rule modeling with `NMolecules.Bricks`.

The later `function-coverage` examples are new material in this repository. They are derived from:

- the public API in `nmolecules/src/nMolecules.Bricks`
- behavioral expectations from `nmolecules/tests/nMolecules.Bricks.Test`
- the existing billing examples copied from `nmolecules.examples/samples/bricks`

## Repository Layout

| Path | Purpose |
|---|---|
| `samples/bricks/analyzer-samples` | Projects that exist to trigger and inspect analyzer diagnostics. |
| `samples/bricks/implementation-samples` | Green Bricks implementation samples for roles, rules, policies, contracts and domain-language kits. |
| `samples/bricks/implementation-samples/implementation-analyzer-samples` | Reserved area for analyzer diagnostics that validate custom Brick implementations and domain-language kits. |
| `samples/bricks/docs` | Consumer sample roadmap copied with the source block |
| `coverage` | Machine-readable Bricks coverage contracts, including the enum variation matrix, public API family coverage matrix and violation-example gates. |

## Bricks Function Coverage

The focused coverage entry point is:

- `samples/bricks/implementation-samples/function-coverage/README.md`

That section maps Bricks function families to concrete, buildable examples:

- public API family coverage for every Bricks source area, including Dependencies, Rules, Profiles, Policies, Runtime, Reports and AI
- deterministic role and policy enforcement
- DDD slice construction with aggregate, entity, value object, repository, factory and service roles, including marker-based aggregate/entity identifier contracts
- role conflict resolution and policy composition
- attribute-only, code, JSON and composed multi-policy variants
- member cardinality contracts
- rule messages and rule filters
- policy, adoption, report and export JSON
- SARIF report output
- runtime wiring, runtime activation, reflection and friend assembly visibility
- governance, conformance, roadmap and dependency coverage reports
- benchmark and benchmark comparison reports
- AI-assisted explanations and rule proposals with explicit trust boundaries
- AI governance workflow for attribute-policy driven analyzer-extension proposals, fixtures and benchmark guardrails
- source/target case projects for type, member, namespace, assembly, dependency-registration and external-reference checks
- configuration precedence and DDD attribute-to-role bridge examples
- clean violation examples for every public API family and every enum variation
  family, including runtime-backed families that do not have analyzer
  diagnostics yet

The executable coverage contracts are:

- `coverage/bricks-api-family-coverage.json`
- `coverage/bricks-enum-variation-matrix.json`

`tests/Samples.Block04.Bricks.Tests` verifies that every public Bricks API
source area has sample evidence, every listed sample path exists, every public
enum value appears in the enum variation matrix, and the coverage checklist
stays synchronized. It also verifies that
`function-coverage/ViolationExamples.cs` contains a concrete violation example
for every public API family, every enum variation family and every public enum
value. `EnumDependencyCombinationTests` additionally verifies the public Bricks
types where multiple enum dimensions are coupled in one API surface.

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

`samples/bricks/analyzer-samples/brick-policy-violations/Samples.Block04.Bricks.Violations.csproj` is expected to emit analyzer diagnostics. Use it to inspect invalid rule declarations and forbidden dependency examples; do not treat that project as part of the green build path.
