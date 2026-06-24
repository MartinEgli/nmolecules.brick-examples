# Source and Target Case Examples

These small projects show how Bricks evaluates policies across different
source and target element kinds.

## Motivation

Architecture rules are often explained with type-to-type dependencies, but real
systems need more shapes: a method can call infrastructure, a composition root
can register an external service, a namespace can leak into another namespace,
and an assembly can depend on a package or another assembly.

## Projects

| Project | Focus | Covered element kinds |
|---|---|---|
| `type-cases` | Source and target roles declared through attributes on classes, interfaces and structs. | `Type` |
| `member-registration-cases` | Method-level source dependencies and composition-root registrations. | `Member`, `Type`, `DependencyRegistration`, `ExternalReference` |
| `namespace-assembly-external-cases` | Package and boundary examples that are larger than one type. | `Namespace`, `Assembly`, `ExternalReference` |

## User Benefit

- Developers see which source and target shape a rule is meant to protect.
- Analyzer authors get small fixtures for semantic model output.
- Architects can discuss rule intent without reading a large production system.
- Teams can start with type-level checks and grow toward namespace, assembly,
  member and external-reference checks without changing the Bricks mental model.

## How The Examples Relate

Every project follows the same flow:

1. Create a `BrickPolicy`.
2. Create source and target `BrickElement` values.
3. Assign source and target roles.
4. Create observed `BrickDependency` facts.
5. Run `BrickRuleEvaluator.Evaluate(policy, dependencies, roles)`.

The only thing that changes between projects is the element kind and scope.
