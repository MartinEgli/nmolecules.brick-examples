# Attribute Multi-Policy Cases

This project demonstrates two ways to model several attribute policies.

| Variant | Purpose |
| --- | --- |
| Definition classes | Each policy has a dedicated owner type, so rules and dependencies are naturally correlated. |
| `AttributeMultiPolicyCases.Assembly.cs` | Several policies share one assembly attribute scope, so ID-prefix conventions are required. |

Use this sample to compare explicit definition classes with assembly-scoped policy declarations.
