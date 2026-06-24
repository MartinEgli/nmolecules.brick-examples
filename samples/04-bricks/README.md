# Block 04: Bricks

This block demonstrates custom role and rule modeling via `NMolecules.Bricks`.
It now also contains the Bricks-specific analyzer violations that used to be mixed into the analyzer workbench.
The long-term consumer-first target structure for this block is documented in `docs/consumer-sample-roadmap.md`.
The conceptual foundation for the Bricks package itself is documented in `../../../nmolecules/src/nMolecules.Bricks/docs/foundational-concept.md`.

## Why This Block Exists

Some teams need architecture roles that are specific to their domain language.
Bricks lets you define those roles and constraints without creating a custom analyzer for every naming pattern.

## What You Learn

- role assignment via `[Role]`
- custom marker aliases via `[RoleAlias]`
- generic dependency policies via `[Rule]`
- expressive alias markers such as `Billing*RoleAttribute`
- centralized role/rule constants (`BillingRoles`, `BillingRules`)
- typed role-id and rule-id access via `RoleId` and `RuleId` alongside attribute-safe string constants
- rule-message customization and dedicated rule-filter attributes
- sample rule declarations for every optional `Rule` field
- analyzer-enforced member-cardinality patterns for custom brick ecosystems:
  exactly one marker, required X+Y markers, fixed-count markers, and XOR markers

## Code Walkthrough

- `BricksSample.cs` is now only the small entry/index file for the sample block
- `Samples.Block04.Bricks.Violations.csproj` contains the intentionally broken custom-role rule declarations
- `domain-language-kits/billing/` contains the reusable billing role/rule vocabulary plus the domain, application, and infrastructure walkthrough
- `rule-filters/` contains concrete class and member names for `Message`, `Excluded*`, and `Required*` filter scenarios
- `member-contracts/use-cases/` groups the member-contract samples by use case: only one, exactly two, two to four, XOR, A + B, and not
- `docs/consumer-sample-roadmap.md` defines the recommended future sample layout for Bricks consumers
- `../../../nmolecules/src/nMolecules.Bricks/docs/foundational-concept.md` defines the package-level Bricks meta-model and current-vs-target boundary

Current analyzer-backed use cases are:
- only one marker
- exactly two markers
- XOR
- A + B

Concept samples that document future Bricks contract patterns, but are not analyzer-enforced yet:
- two to four markers
- not / forbidden marker

## Exercises

1. Add a second rule (`RequireDependency`) between application and domain roles.
2. Extend the use-case sample set with another range or exclusion pattern, for example "at least one of three markers".
3. Add a new custom alias attribute for a reporting role and annotate one type.
4. Build `Samples.Block04.Bricks.Violations.csproj` and inspect the invalid rule configuration plus the forbidden domain-to-infrastructure dependency.
