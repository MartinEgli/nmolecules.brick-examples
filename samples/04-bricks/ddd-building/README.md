# Building A DDD Slice With Bricks

This sample shows how Bricks can be used to build the DDD part of an application when the existing DDD markers are not specific enough for a team.

## Motivation

DDD code usually starts with useful words: aggregate root, entity, value object, repository, factory, domain service and application service. Those words help people, but the compiler does not know what they mean.

Bricks lets a team turn those words into explicit architectural roles and rules:

- role aliases make domain vocabulary readable in code
- assembly rules describe allowed and forbidden dependencies
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
| `DddBrickRules.Assembly.cs` | Defines assembly-level Bricks rules for the DDD slice. |
| `DddBuildingSample.cs` | Builds a small billing DDD model with aggregate, entity, value object, repository, factory and services. |
| `DddBrickPolicyExample.cs` | Evaluates the attribute-derived DDD policy without defining extra rules in code. |
| `DddAttributeOnlyConfigurationExample.cs` | Builds and evaluates the DDD policy from attributes only: role markers on types and `[assembly: Rule(...)]` declarations on the assembly. |

## Suggested Learning Path

1. Start with `DddBrickRoles.cs` and inspect the custom role markers.
2. Read `DddBuildingSample.cs` and map each DDD building block to a role.
3. Inspect `DddBrickRules.Assembly.cs` to see which dependencies the sample wants to forbid or require.
4. Read `DddAttributeOnlyConfigurationExample.cs` to see how the policy is derived from `[assembly: Rule(...)]` attributes only.
5. Read `DddBrickPolicyExample.cs` to see the same attribute-derived policy used by the evaluator.

## Attribute-Only Configuration

The attribute-only path is the lightest way to start:

- put role-marker attributes such as `[DddAggregateRoot]`, `[DddValueObject]` and `[DddRepository]` on the relevant types
- put `[assembly: Rule(...)]` declarations in one assembly-level file
- let analyzers or supporting tools derive a policy from the compiled attribute metadata

This is useful when a team wants the DDD model and policy to stay close to the code and does not yet need external policy files. The sample still creates a `BrickPolicy` object internally because `BrickRuleEvaluator` consumes policy objects, but every configured rule in that policy comes from `[assembly: Rule(...)]`; no additional rule is defined with `new BrickRule(...)`.

## Design Rule

The sample keeps the domain model independent from infrastructure:

- domain code can expose repository contracts
- application services can depend on domain and repository contracts
- infrastructure implementations must not be pulled into aggregates or value objects
