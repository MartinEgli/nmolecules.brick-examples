# Required Named Member Contract

This project demonstrates `RequireNamedMembersAttribute`.

| Sample | Purpose |
| --- | --- |
| `RequiredNamedIdentifierValidSample` | Declares both required logical identifiers `X` and `Y`. |
| `RequiredNamedIdentifierMissingYInvalidSample` | Declares only `X`; the analyzer reports the missing required `Y` marker name. |

Use this contract when a role needs specific named member slots. Combine it with `RequireUniqueNamedMemberAttribute` when each required slot must also appear at most once.
