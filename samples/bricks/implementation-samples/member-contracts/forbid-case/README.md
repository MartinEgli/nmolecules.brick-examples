# Forbidden-Member Contract

This project demonstrates the analyzer-backed `ForbidMemberAttribute` pattern
from `NMolecules.Bricks`.

| Sample | Purpose |
| --- | --- |
| `NotValidSample` | Does not contain the forbidden member marker. |
| `NotInvalidSample` | Contains `ForbiddenMarkerAttribute`; the analyzer reports it. |

The current core evaluator and analyzer enforce this negative member contract.
`NotInvalidSample` intentionally carries the forbidden marker so tests and
violation samples can demonstrate the rejected shape.
