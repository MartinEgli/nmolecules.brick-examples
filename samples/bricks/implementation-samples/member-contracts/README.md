# Member Contracts

This area groups marker and member constraints by consumer use case instead of analyzer primitive.

Each use case is its own small project:

- `only-one-case`
- `exactly-two-case`
- `all-members-case`
- `exclusive-choice-case`
- `range-case`
- `forbid-case`
- `required-named-case`
- `unique-named-case`

The projects intentionally cover every shipped member-contract family with
valid and invalid examples that are evaluated by the Bricks runtime and analyzer
tests.

## Coverage Matrix

| Use case | Contract API | Backed today | Valid samples | Invalid samples | Notes |
| --- | --- | --- | --- | --- | --- |
| Exactly one member marker | `RequireExactlyOneMemberAttribute` | Yes | `OnlyOneValidSample` | `OnlyOneMissingSample`, `OnlyOneTooManySample` | Covers both lower and upper boundary failures. |
| Exact member count | `RequireMemberCountAttribute` | Yes | `ExactlyTwoValidSample` | `ExactlyTwoTooFewSample`, `ExactlyTwoTooManySample` | Shows that the configured count is exact, not a minimum. |
| All required marker kinds | `RequireAllMembersAttribute` | Yes | `AndAPlusBValidSample`, `AndAPlusBDuplicateMarkersValidSample` | `AndAPlusBMissingAInvalidSample`, `AndAPlusBMissingBInvalidSample`, `AndAPlusBMissingBothInvalidSample` | Shows at-least-one-per-kind semantics; duplicate markers are accepted. |
| Exclusive choice | `RequireExclusiveChoiceAttribute` | Yes | `XorLeftValidSample`, `XorRightValidSample` | `XorBothInvalidSample`, `XorNoneInvalidSample` | Covers both legal branches and both XOR failure modes. |
| Member count range | `RequireMemberRangeAttribute` | Yes | `TwoToFourValidTwoSample`, `TwoToFourValidFourSample` | `TwoToFourTooFewSample`, `TwoToFourTooManySample` | Covers inclusive lower and upper boundaries. |
| Forbidden member marker | `ForbidMemberAttribute` | Yes | `NotValidSample` | `NotInvalidSample` | Covers negative member-marker constraints. |
| Required named member marker | `RequireNamedMembersAttribute` | Yes | `RequiredNamedIdentifierValidSample` | `RequiredNamedIdentifierMissingYInvalidSample` | Requires configured logical names such as `X` and `Y` to be present. |
| Unique named member marker | `RequireUniqueNamedMemberAttribute` | Yes | `UniqueNamedIdentifierValidSample` | `UniqueNamedIdentifierDuplicateNameInvalidSample`, `UniqueNamedIdentifierDuplicateUnnamedInvalidSample` | Allows several logical names such as `X` and `Y`, but rejects duplicates per name. |

`catalog/MemberContractUseCaseCatalog` is the executable index for these examples. The tests assert every case against `BrickMemberCardinalityEvaluator`.

Every member-contract project contains at least one invalid sample. The tests
assert `BrickViolationKind`, severity, state, rule name, source identity, and
diagnostic message for analyzer-backed violations.
