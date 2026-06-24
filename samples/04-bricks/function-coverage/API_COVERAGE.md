# Bricks API Coverage Checklist

Coverage means every public Bricks function family has at least one buildable example or one intentional diagnostic example in this repository. The examples are grouped by user-facing purpose rather than by internal source file.

| API family | Covered by | User-facing scenario |
|---|---|---|
| Attribute markers: `RoleAttribute`, `RoleAliasAttribute`, `PolicyAttribute`, `PolicyImportAttribute`, `RoleCombinationAttribute`, `RuleAttribute`, `DependencyAttribute`, `RuleFilterAttribute`, member-cardinality attributes | `domain-language-kits/`, `ddd-building/`, `member-contracts/`, `rule-filters/`, `violations/` | Consumers mark roles, aliases, policies, imports, role combinations, rules, dependencies, filters and member contracts directly in code. |
| DDD role modeling with Bricks | `ddd-building/` | Consumers can build aggregate, entity, value object, repository, factory, domain service and application service roles as a focused DDD slice through role attributes plus `[assembly: Policy]`, `[assembly: PolicyImport]`, `[assembly: RoleCombination]`, `[assembly: Rule]` and `[assembly: Dependency]`. |
| Typed IDs and messages: `RoleId`, `RuleId`, `RuleMessage`, `RuleMessageBuilder`, `RuleFilter` variants | `PolicyAndResolutionExamples.cs`, `rule-filters/` | Teams use stable identifiers and readable diagnostics instead of raw strings spread through code. |
| Core model: roles, dimensions, elements, dependencies, source locations, rules, policies, violations | `SampleBrickModel.cs`, `PolicyAndResolutionExamples.cs` | Architecture facts become explicit and machine-readable. |
| Policy documents and JSON policy loading | `PolicyAndResolutionExamples.cs` | A team can keep rules in versioned policy files and validate schema compatibility. |
| Policy composition and imports | `PolicyAndResolutionExamples.cs` | Platform defaults can be imported, narrowed or overridden by consumer policies. |
| Role assignment, role resolution, precedence, conflicts and role-combination rules | `PolicyAndResolutionExamples.cs`, `ConfigurationExamples.cs` | Teams can combine direct attributes, aliases, policy files and conventions while seeing conflicts. |
| Rule evaluation | `PolicyAndResolutionExamples.cs` | Deterministic enforcement produces violations from observed dependencies and resolved roles. |
| Member-cardinality evaluation | `member-contracts/use-cases/` | Custom role ecosystems can require exactly one, all, count-based or exclusive member markers. |
| Adoption, suppressions, baselines and violation-state projection | `RuntimeAndExportExamples.cs` | Legacy violations can be accepted or suppressed with owner, justification and expiry. |
| Reports, JSON and SARIF | `RuntimeAndExportExamples.cs` | CI, dashboards and security tooling can consume normalized Bricks output. |
| Exports: role map, dependency graph and resolution trace | `RuntimeAndExportExamples.cs` | Architecture tooling can visualize roles, dependencies and resolution decisions. |
| `BrickJsonFile` helpers | `RuntimeAndExportExamples.cs` | CLI-style tools can save reports and exports without hand-written file glue. |
| Visibility model and friend assemblies | `RuntimeAndExportExamples.cs` | Internal visibility exceptions can be reviewed like architecture dependencies. |
| Runtime wiring and DI registration model | `RuntimeAndExportExamples.cs` | Composition-root dependencies become visible to policy checks. |
| Runtime activation model | `RuntimeAndExportExamples.cs` | Plugin and activator patterns are explicit instead of invisible runtime behavior. |
| Reflection model and confidence policy | `RuntimeAndExportExamples.cs` | Reflection access can be justified and reviewed with evidence confidence. |
| Governance reports | `GovernanceAndReadinessExamples.cs` | Architecture owners can track policy ownership, exceptions, evolution and compatibility. |
| Conformance reports | `GovernanceAndReadinessExamples.cs` | Teams can see which Bricks maturity level they have reached. |
| Roadmap reports | `GovernanceAndReadinessExamples.cs` | Delivery status can be discussed by stage and required item. |
| Dependency coverage reports | `GovernanceAndReadinessExamples.cs` | Architects can see whether static, runtime and configuration dependencies are observable. |
| Built-in role packs and profiles | `GovernanceAndReadinessExamples.cs`, `ConfigurationExamples.cs` | Consumers can start from Layered, Onion, Hexagonal or CQRS defaults. |
| Configuration source precedence and resolver | `ConfigurationExamples.cs` | Users can predict whether generated defaults, packages, policy files, MSBuild, analyzer config or source annotations win. |
| Attribute-role bridge and DDD bridge | `ConfigurationExamples.cs` | Existing nMolecules DDD attributes can be translated to Bricks roles. |
| Diagnostic ID governance | `PolicyAndResolutionExamples.cs` | Rule IDs remain stable and discoverable by range. |
| Benchmark cases, runner, reports and comparison reports | `BenchmarkAndAiExamples.cs` | CI can detect feedback-loop regressions for central Bricks operations. |
| AI-assisted violation comments, remediation options, rule proposals and trust boundaries | `BenchmarkAndAiExamples.cs`, `ai-governance/` | AI can explain deterministic findings and propose analyzer extensions without silently changing policy or enforcement. |
| AI governance for analyzer extension workflows | `ai-governance/` | Architects can require human review, analyzer fixtures, benchmark guardrails and explicit exception handling before AI-suggested rules become diagnostics. |

## Verification

The build command verifies that these examples compile against the current public Bricks API:

```powershell
dotnet build .\nMolecules.BrickExamples.slnx -v minimal
```

Intentional analyzer failures remain isolated in `Samples.Block04.Bricks.Violations.csproj`.
