# Policy Variant Cases

These small projects show the main ways a team can define and combine Bricks
policies.

## Variant Plan

| Project | Policy style | Source/target variation | What it proves |
|---|---|---|---|
| `attribute-only-policy-cases` | `[assembly: Policy]`, `[assembly: Rule]`, `[assembly: Dependency]`, role attributes | Class, interface, struct, namespace, external reference | Attribute-only works when policy, rule and dependency IDs follow a clear convention. Multiple policies in one assembly need either a convention or a future explicit policy correlation field. |
| `attribute-multi-policy-cases` | Class-scoped `[Policy]`, `[PolicyImport]`, `[Rule]`, `[Dependency]` | Type and namespace dependencies across platform/product/team policies | Multiple attribute policies can be composed without extra rule/dependency policy IDs when each policy has a dedicated definition class. |
| `code-policy-cases` | C# `BrickPolicy` objects | Type, member, registration, external reference | Direct code policies are best when test fixtures need full object control. |
| `json-policy-cases` | JSON policy document | Namespace, assembly, type alias, external assignment | External policy files can carry rules, imports, aliases and external assignments. |
| `composed-multi-policy-cases` | Several policies combined with `Import`, `Extend`, `Narrow`, `Override`, `Disable` | Type, namespace, assembly | Teams can keep platform, product and team policies separate and compose them in one project. |

## Attribute-Only Correlation Decision

Current attribute-only metadata carries IDs on the policy, rule and dependency
attributes, but `RuleAttribute` and `DependencyAttribute` do not carry an
explicit `PolicyId`. That is enough for a single policy per assembly or for a
strict naming convention such as `ATTR-ORDERS-*`.

When multiple independent attribute policies live in the same assembly, tools
need one of these approaches:

- convention-based grouping by ID prefix
- one assembly file per policy with an agreed scanner convention
- a future explicit policy correlation field on rule and dependency attributes

The sample keeps the current API unchanged and demonstrates the convention-based
approach so the trade-off is visible in tests.

The `attribute-multi-policy-cases` sample shows the alternative: put each policy
on its own definition class. Then the owner type is the correlation boundary,
so rules, imports and dependencies do not need a repeated policy ID.
