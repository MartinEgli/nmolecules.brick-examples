# Only-One Member Contract

This project demonstrates `RequireExactlyOneMemberAttribute`.

| Sample | Purpose |
| --- | --- |
| `OnlyOneValidSample` | Exactly one member carries `OnlyOneMarkerAttribute`; no violation is expected. |
| `OnlyOneMissingSample` | No member carries the marker; one member-cardinality violation is expected. |
| `OnlyOneTooManySample` | Two members carry the marker; one member-cardinality violation is expected. |

Use this pattern when a role marker needs one and only one configured member, for example a single identity member.
