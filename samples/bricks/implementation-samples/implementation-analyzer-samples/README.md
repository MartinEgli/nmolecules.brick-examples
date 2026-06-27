# Brick Implementation Analyzer Samples

This area is for analyzer samples that validate custom Brick implementations.

Use it when a team creates its own Bricks vocabulary, for example:

- custom role attributes derived from `RoleAttribute`
- custom rule catalogs and policy IDs
- domain-language kits such as billing, ordering or payments
- specialized dependency attributes with stricter semantics

## Difference To `analyzer-samples/`

`../../analyzer-samples/` checks ordinary Bricks usage mistakes.

This folder checks mistakes made while implementing a reusable Brick vocabulary or domain-language kit. Examples belong here when they answer: "Did we implement our custom Brick language correctly?"

## Planned Diagnostic Themes

| Theme | Example violation |
| --- | --- |
| Role attribute implementation | A custom role attribute forwards an empty role name. |
| Policy catalog implementation | A rule references a policy ID that is not declared by the kit. |
| Dependency attribute implementation | A specialized dependency attribute forwards the wrong dependency kind. |
| Domain-language kit consistency | A published role has no matching sample, rule or explanation. |
