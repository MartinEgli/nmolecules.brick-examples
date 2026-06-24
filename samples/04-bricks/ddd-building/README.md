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
| `DddAttributeOnlyConfigurationExample.cs` | Shows the same DDD slice configured only through attributes: role markers on types and rules on the assembly. |

## Suggested Learning Path

1. Start with `DddBrickRoles.cs` and inspect the custom role markers.
2. Read `DddBuildingSample.cs` and map each DDD building block to a role.
3. Inspect `DddBrickRules.Assembly.cs` to see which dependencies the sample wants to forbid or require.
4. Read `DddAttributeOnlyConfigurationExample.cs` to see the attribute-only configuration path.
5. Read `DddBrickPolicyExample.cs` to see how the same idea can also become a policy object for tools.

## Attribute-Only Configuration

The attribute-only path is the lightest way to start:

- put role-marker attributes such as `[DddAggregateRoot]`, `[DddValueObject]` and `[DddRepository]` on the relevant types
- put `[assembly: Rule(...)]` declarations in one assembly-level file
- let analyzers or supporting tools read the attributes from compiled metadata

This is useful when a team wants the DDD model to stay close to the code and does not yet need external policy files.

## Design Rule

The sample keeps the domain model independent from infrastructure:

- domain code can expose repository contracts
- application services can depend on domain and repository contracts
- infrastructure implementations must not be pulled into aggregates or value objects
