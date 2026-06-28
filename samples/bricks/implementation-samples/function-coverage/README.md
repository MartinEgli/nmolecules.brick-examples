# Bricks Function Coverage Examples

This section turns the Bricks API surface into practical examples. The examples are not only syntax samples; each one answers three questions:

- Motivation: why a team would use the function.
- User benefit: what the team gains in daily architecture work.
- API coverage: which Bricks function family is demonstrated.

## Coverage Matrix

For the full checklist, see `API_COVERAGE.md`. The machine-readable companion
contract is `coverage/bricks-api-family-coverage.json`; the example test
project verifies it against the public Bricks source tree and the sample paths.

| Example file | Motivation | User benefit | Bricks functions covered |
|---|---|---|---|
| `SampleBrickModel.cs` | Keep all examples grounded in one small billing domain. | Readers can compare examples without re-learning the model each time. | `BrickElement`, IDs, dependencies, role assignments, resolved roles, violations |
| `PolicyAndResolutionExamples.cs` | Explain how custom roles become enforceable architecture rules. | Teams can model their own vocabulary, role-combination hierarchy, reviewed exceptions and deterministic violations. | `BrickRoleDimension`, `BrickRole`, `BrickRule`, `BrickPolicy`, imports, aliases, role selectors, role resolver, policy composer, role-combination rules, suppression projection, rule evaluator, policy document validation, rule messages, rule filters, diagnostic-id governance |
| `RuntimeAndExportExamples.cs` | Show runtime and visibility dependencies that are invisible to plain type-reference checks. | Teams can include DI registrations, friend assemblies, reflection and runtime activation in reports. | visibility, runtime wiring, runtime activation, reflection, adoption/suppression/baseline projection, report JSON, SARIF, role-map export, dependency-graph export, resolution-trace export |
| `GovernanceAndReadinessExamples.cs` | Make maturity, roadmap and operational readiness measurable. | Architects can discuss Bricks rollout with evidence instead of opinions. | governance reports, conformance reports, roadmap reports, dependency coverage reports, built-in profile and role-pack catalogs |
| `BenchmarkAndAiExamples.cs` | Show how central Bricks operations are measured and explained safely. | Teams can guard feedback-loop performance and generate reviewable AI assistance without silent policy changes. | benchmark cases/results/reports, benchmark comparison, AI comments, markdown/JSON output, proposal queues, reviewed promotion, CI run configuration, AI trust boundary |
| `ConfigurationExamples.cs` | Explain how generated defaults, packages, policy files, MSBuild, analyzer config and source annotations resolve. | Users can predict which configuration value wins before debugging analyzer behavior. | configuration sources, precedence, resolver, attribute-role bridge, built-in DDD bridge |
| `ViolationExamples.cs` | Keep one clean violation example for every Bricks API family and enum variation family. | New Bricks APIs cannot be added without a concrete violation story and checked evidence path. | API-family violation catalog, enum-family violation catalog, public enum value violation coverage |

## Build

From `nmolecules.brick-examples`:

```powershell
dotnet build .\nMolecules.BrickExamples.slnx -v minimal
```

The default solution intentionally includes only the green examples. `Samples.Block04.Bricks.Violations.csproj` remains available for diagnostic exploration.
