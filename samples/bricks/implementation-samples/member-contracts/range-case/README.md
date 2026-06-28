# Range Member Contract

This project demonstrates the analyzer-backed `RequireMemberRangeAttribute`
pattern from `NMolecules.Bricks`.

| Sample | Purpose |
| --- | --- |
| `TwoToFourValidTwoSample` | Lower boundary of the intended range. |
| `TwoToFourValidFourSample` | Upper boundary of the intended range. |
| `TwoToFourTooFewSample` | Below the intended range. |
| `TwoToFourTooManySample` | Above the intended range. |

The current core evaluator and analyzer enforce this inclusive range. Valid
samples sit on the lower and upper boundary; invalid samples sit outside the
range and are covered by the example test catalog.
