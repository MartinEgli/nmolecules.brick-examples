# Building A DDD Slice With Bricks

This sample shows how Bricks can be used to build the DDD part of an application when the existing DDD markers are not specific enough for a team.

## Motivation

DDD code usually starts with useful words: aggregate root, entity, value object, repository, factory, domain service and application service. Those words help people, but the compiler does not know what they mean.

Bricks lets a team turn those words into explicit architectural roles and rules:

- role aliases make domain vocabulary readable in code
- assembly policy imports and rules describe reusable defaults plus allowed and forbidden dependencies
- role-combination attributes describe which role families must not coexist on one element
- tools can derive policy objects from the same attribute metadata for reports and CI

## User Benefit

The benefit is practical:

- developers see the intended DDD role directly on each type
- architecture rules live next to the sample instead of in a slide deck
- teams can start with a small DDD subset and grow toward stronger policies
- the same vocabulary can later feed analyzers, reports, role maps and benchmarks

## Files

| File | Purpose |
|---|---|
| `DddBrickRoles.cs` | Defines DDD role constants and custom role-marker attributes. |
| `DddBrickRules.Assembly.cs` | Defines assembly-level `[assembly: Policy]`, `[assembly: PolicyImport]`, `[assembly: RoleCombination]`, `[assembly: Rule]` and `[assembly: Dependency]` declarations for the DDD slice. |
| `DddBuildingSample.cs` | Builds a small billing DDD model with aggregate, entity, value object, repository, factory and services. |
| `DddBrickPolicyExample.cs` | Keeps the historical policy entry point and evaluates the attribute-derived DDD policy. |
| `DddAttributeOnlyConfigurationExample.cs` | Builds and evaluates the DDD policy from attributes only: role markers on types plus `[assembly: Policy(...)]`, `[assembly: PolicyImport(...)]`, `[assembly: RoleCombination(...)]`, `[assembly: Rule(...)]` and `[assembly: Dependency(...)]`. |
| `DddCodePolicyExample.cs` | Keeps the former code-policy entry point but delegates to the assembly-attribute policy. |

## Suggested Learning Path

1. Start with `DddBrickRoles.cs` and inspect the custom role markers.
2. Read `DddBuildingSample.cs` and map each DDD building block to a role.
3. Inspect `DddBrickRules.Assembly.cs` to see which dependencies the sample wants to forbid or require.
4. Read `DddAttributeOnlyConfigurationExample.cs` to see how the policy is derived from `[assembly: Policy(...)]`, `[assembly: PolicyImport(...)]`, `[assembly: RoleCombination(...)]`, `[assembly: Rule(...)]` and `[assembly: Dependency(...)]` attributes only.
5. Read `DddCodePolicyExample.cs` to see how the former code-policy entry point now reuses the attribute policy.
6. Read `DddBrickPolicyExample.cs` when existing callers need the historical policy entry point.

## Policy Definition Styles

The DDD sample intentionally keeps the effective policy definition attribute-only.

### Attribute Policy

Use this when policy should live directly beside the source declarations:

- type roles are declared with attributes such as `[DddAggregateRoot]`, `[DddValueObject]` and `[DddRepository]`
- the policy header is declared with `[assembly: Policy(...)]`
- imported policy defaults are declared with `[assembly: PolicyImport(...)]`
- role compatibility is declared with `[assembly: RoleCombination(...)]`
- rules are declared with `[assembly: Rule(...)]`
- dependency evidence is declared with `[assembly: Dependency(...)]`
- analyzers and tools derive policy metadata from compiled attributes

This is the best fit for small teams, source-first governance and samples where the visible `[...]` declarations should be the policy.

## Attribute-Only Configuration

The attribute-only path is the lightest way to start:

- put role-marker attributes such as `[DddAggregateRoot]`, `[DddValueObject]` and `[DddRepository]` on the relevant types
- put `[assembly: Policy(...)]`, `[assembly: PolicyImport(...)]`, `[assembly: RoleCombination(...)]`, `[assembly: Rule(...)]` and `[assembly: Dependency(...)]` declarations in one assembly-level file
- let analyzers or supporting tools derive a policy from the compiled attribute metadata

This is useful when a team wants the DDD model and policy to stay close to the code and does not yet need external policy files. The attribute-only sample still creates a `BrickPolicy` object internally because `BrickRuleEvaluator` consumes policy objects, but policy header, imports, role combinations, configured rules and dependency evidence come from `[assembly: Policy(...)]`, `[assembly: PolicyImport(...)]`, `[assembly: RoleCombination(...)]`, `[assembly: Rule(...)]` and `[assembly: Dependency(...)]`; no additional rule is defined with `new BrickRule(...)` in that path.

## Design Rule

The sample keeps the domain model independent from infrastructure:

- domain code can expose repository contracts
- application services can depend on domain and repository contracts
- infrastructure implementations must not be pulled into aggregates or value objects
