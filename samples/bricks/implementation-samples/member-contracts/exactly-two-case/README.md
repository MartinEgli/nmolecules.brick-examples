# Exact-Count Member Contract

This project demonstrates `RequireMemberCountAttribute`.

| Sample | Purpose |
| --- | --- |
| `ExactlyTwoValidSample` | Exactly two members carry `RepeatedMarkerAttribute`; no violation is expected. |
| `ExactlyTwoTooFewSample` | Only one member carries the marker; one violation is expected. |
| `ExactlyTwoTooManySample` | Three members carry the marker; one violation is expected. |

Use this pattern when a type must expose a fixed number of marked members.
