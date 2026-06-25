# All-Members Contract

This project demonstrates `RequireAllMembersAttribute`.

| Sample | Purpose |
| --- | --- |
| `AndAPlusBValidSample` | Has one `MarkerAAttribute` member and one `MarkerBAttribute` member. |
| `AndAPlusBDuplicateMarkersValidSample` | Shows that duplicates are valid because the contract requires at least one of each marker kind. |
| `AndAPlusBMissingAInvalidSample` | Missing marker A; one violation is expected. |
| `AndAPlusBMissingBInvalidSample` | Missing marker B; one violation is expected. |
| `AndAPlusBMissingBothInvalidSample` | Missing both markers; two violations are expected. |

Use this pattern when a type must provide several required member categories.
