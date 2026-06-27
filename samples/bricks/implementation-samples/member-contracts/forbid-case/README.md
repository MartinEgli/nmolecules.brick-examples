# Forbidden-Member Contract

This project documents the concept-only `ForbidMemberAttribute` pattern.

| Sample | Purpose |
| --- | --- |
| `NotValidSample` | Does not contain the forbidden member marker. |
| `NotInvalidSample` | Contains `ForbiddenMarkerAttribute`; a future analyzer should report it. |

The current core evaluator does not enforce this attribute yet. The sample captures the intended negative member constraint.
