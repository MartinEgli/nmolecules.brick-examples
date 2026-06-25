# Exclusive-Choice Member Contract

This project demonstrates `RequireExclusiveChoiceAttribute`.

| Sample | Purpose |
| --- | --- |
| `XorLeftValidSample` | Chooses the left marker only. |
| `XorRightValidSample` | Chooses the right marker only. |
| `XorBothInvalidSample` | Chooses both markers; one violation is expected. |
| `XorNoneInvalidSample` | Chooses no marker; one violation is expected. |

Use this pattern when a type must choose exactly one member shape from two alternatives.
