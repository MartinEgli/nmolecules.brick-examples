# nmolecules.brick-examples

Standalone example repository for nMolecules Bricks.

## Source Provenance

The initial content was split out from the existing superrepo sample block:

- Source repository path: `nmolecules.examples/samples/04-bricks`
- Target repository path: `samples/04-bricks`
- Reason: keep Bricks examples usable as a focused repository while preserving the original teaching material and file layout.

The copied sample block demonstrates custom role and rule modeling with `NMolecules.Bricks`.

The later `function-coverage` examples are new material in this repository. They are derived from:

- the public API in `nmolecules/src/nMolecules.Bricks`
- behavioral expectations from `nmolecules/tests/nMolecules.Bricks.Test`
- the existing billing examples copied from `nmolecules.examples/samples/04-bricks`

## Repository Layout

| Path | Purpose |
|---|---|
| `samples/04-bricks/Samples.Block04.Bricks.csproj` | Green Bricks sample project |
| `samples/04-bricks/Samples.Block04.Bricks.Violations.csproj` | Intentional violation project for analyzer diagnostics |
| `samples/04-bricks/ai-governance` | AI collaboration sample for deterministic governance, attribute policies and analyzer-extension proposals |
| `samples/04-bricks/ddd-building` | Focused example for building a DDD slice with Bricks roles and rules |
| `samples/04-bricks/domain-language-kits/billing` | Billing role/rule vocabulary plus domain walkthrough |
| `samples/04-bricks/function-coverage` | Full Bricks function coverage examples with motivation, user benefit and API mapping |
| `samples/04-bricks/member-contracts` | Member contract modeling examples |
| `samples/04-bricks/policy-variant-cases` | Small projects for attribute-only, attribute multi-policy, code, JSON and composed multi-policy variants |
| `samples/04-bricks/rule-filters` | Rule filter and custom diagnostic message examples |
| `samples/04-bricks/source-target-cases` | Small projects for Type, Member, Namespace, Assembly, DependencyRegistration and ExternalReference source/target cases |
| `samples/04-bricks/docs` | Consumer sample roadmap copied with the source block |

## Bricks Function Coverage

The focused coverage entry point is:

- `samples/04-bricks/function-coverage/README.md`

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
