# Member Contracts

This area groups marker and member constraints by consumer use case instead of analyzer primitive.

Each use case is its own small project:

- `only-one-case`
- `exactly-two-case`
- `all-members-case`
- `exclusive-choice-case`
- `range-case`
- `forbid-case`

The projects intentionally mix analyzer-backed contracts that produce Roslyn diagnostics today and concept samples that document useful future patterns.

## Coverage Matrix

| Use case | Contract API | Backed today | Valid samples | Invalid samples | Notes |
| --- | --- | --- | --- | --- | --- |
| Exactly one member marker | `RequireExactlyOneMemberAttribute` | Yes | `OnlyOneValidSample` | `OnlyOneMissingSample`, `OnlyOneTooManySample` | Covers both lower and upper boundary failures. |
| Exact member count | `RequireMemberCountAttribute` | Yes | `ExactlyTwoValidSample` | `ExactlyTwoTooFewSample`, `ExactlyTwoTooManySample` | Shows that the configured count is exact, not a minimum. |
| All required marker kinds | `RequireAllMembersAttribute` | Yes | `AndAPlusBValidSample`, `AndAPlusBDuplicateMarkersValidSample` | `AndAPlusBMissingAInvalidSample`, `AndAPlusBMissingBInvalidSample`, `AndAPlusBMissingBothInvalidSample` | Shows at-least-one-per-kind semantics; duplicate markers are accepted. |
| Exclusive choice | `RequireExclusiveChoiceAttribute` | Yes | `XorLeftValidSample`, `XorRightValidSample` | `XorBothInvalidSample`, `XorNoneInvalidSample` | Covers both legal branches and both XOR failure modes. |
| Member count range | `RequireMemberRangeAttribute` | Concept only | `TwoToFourValidTwoSample`, `TwoToFourValidFourSample` | `TwoToFourTooFewSample`, `TwoToFourTooManySample` | Documents a useful future analyzer contract for lower/upper bounds. |
| Forbidden member marker | `ForbidMemberAttribute` | Concept only | `NotValidSample` | `NotInvalidSample` | Documents a useful future analyzer contract for negative member constraints. |

`catalog/MemberContractUseCaseCatalog` is the executable index for these examples. The tests assert all analyzer-backed cases against `BrickMemberCardinalityEvaluator` and keep the concept-only cases visible without pretending that they are implemented today.

Every member-contract project contains at least one invalid sample. Analyzer-backed projects also have concrete violation tests that assert `BrickViolationKind`, severity, state, rule name, source identity, and diagnostic message.
