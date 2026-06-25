# Attribute-Only Policy Cases

This project demonstrates multiple policies declared with assembly-level attributes.

It uses:

- `[assembly: Policy]`
- `[assembly: Rule]`
- `[assembly: Dependency]`
- role attributes on concrete types

Because `RuleAttribute` and `DependencyAttribute` do not carry an owning policy ID, the sample groups rules and dependencies by ID prefix and tests the correlation trade-off.
