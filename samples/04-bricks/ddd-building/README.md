# Building A DDD Slice With Bricks

This sample shows how Bricks can be used to build the DDD part of an application when the existing DDD markers are not specific enough for a team.

## Motivation

DDD code usually starts with useful words: aggregate root, entity, value object, repository, factory, domain service and application service. Those words help people, but the compiler does not know what they mean.

Bricks lets a team turn those words into explicit architectural roles and rules:

- role aliases make domain vocabulary readable in code
- assembly rules describe allowed and forbidden dependencies
- policy objects make the same rules available for tools, reports and CI

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
| `DddBrickPolicyExample.cs` | Builds the same DDD slice as Bricks policy/model objects and evaluates a dependency. |

## Suggested Learning Path

1. Start with `DddBrickRoles.cs` and inspect the custom role markers.
2. Read `DddBuildingSample.cs` and map each DDD building block to a role.
3. Inspect `DddBrickRules.Assembly.cs` to see which dependencies the sample wants to forbid or require.
4. Read `DddBrickPolicyExample.cs` to see how the same idea becomes a policy object for tools.

## Design Rule

The sample keeps the domain model independent from infrastructure:

- domain code can expose repository contracts
- application services can depend on domain and repository contracts
- infrastructure implementations must not be pulled into aggregates or value objects
