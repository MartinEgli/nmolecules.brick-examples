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
| `samples/bricks/Samples.Block04.Bricks.csproj` | Green Bricks sample project |
| `samples/bricks/Samples.Block04.Bricks.Violations.csproj` | Intentional violation project for analyzer diagnostics |
| `samples/bricks/ai-governance` | AI collaboration sample for deterministic governance, attribute policies and analyzer-extension proposals |
| `samples/bricks/ddd-building` | Focused example for building a DDD slice with Bricks roles and rules |
| `samples/bricks/domain-language-kits/billing` | Billing role/rule vocabulary plus domain walkthrough |
| `samples/bricks/function-coverage` | Full Bricks function coverage examples with motivation, user benefit and API mapping |
| `samples/bricks/class-contracts` | Class-scoped contract examples with valid and violation paths |
| `samples/bricks/interface-contracts` | Interface-targeted contract examples |
| `samples/bricks/namespace-contracts` | Namespace-level contract examples with modeled namespace role assignments |
| `samples/bricks/member-contracts` | Member contract modeling examples split into small projects |
| `samples/bricks/policy-variant-cases` | Small projects for attribute-only, attribute multi-policy, code, JSON and composed multi-policy variants |
| `samples/bricks/rule-filters` | Rule filter and custom diagnostic message examples |
| `samples/bricks/source-target-cases` | Small projects for Type, Member, Namespace, Assembly, DependencyRegistration and ExternalReference source/target cases |
| `samples/bricks/docs` | Consumer sample roadmap copied with the source block |

## Bricks Function Coverage

The focused coverage entry point is:

- `samples/bricks/function-coverage/README.md`

That section maps Bricks function families to concrete, buildable examples:

- deterministic role and policy enforcement
- DDD slice construction with aggregate, entity, value object, repository, factory and service roles
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
