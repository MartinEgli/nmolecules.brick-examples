namespace Samples.Block04.Bricks;


public static class MemberContractUseCaseCatalog
{
    public static IReadOnlyList<MemberContractUseCaseExpectation> Cases { get; } =
        new MemberContractUseCaseExpectation[]
        {
            Backed<OnlyOneValidSample>("RequireExactlyOneMember", shouldBeValid: true, violations: 0, "exactly one marker is accepted"),
            Backed<OnlyOneMissingSample>("RequireExactlyOneMember", shouldBeValid: false, violations: 1, "missing marker is rejected"),
            Backed<OnlyOneTooManySample>("RequireExactlyOneMember", shouldBeValid: false, violations: 1, "more than one marker is rejected"),

            Backed<ExactlyTwoValidSample>("RequireMemberCount", shouldBeValid: true, violations: 0, "exact count is accepted"),
            Backed<ExactlyTwoTooFewSample>("RequireMemberCount", shouldBeValid: false, violations: 1, "too few markers are rejected"),
            Backed<ExactlyTwoTooManySample>("RequireMemberCount", shouldBeValid: false, violations: 1, "too many markers are rejected"),

            Backed<AndAPlusBValidSample>("RequireAllMembers", shouldBeValid: true, violations: 0, "one marker for each required member kind is accepted"),
            Backed<AndAPlusBDuplicateMarkersValidSample>("RequireAllMembers", shouldBeValid: true, violations: 0, "duplicates are accepted because this contract requires at least one of each marker kind"),
            Backed<AndAPlusBMissingAInvalidSample>("RequireAllMembers", shouldBeValid: false, violations: 1, "missing A marker is rejected"),
            Backed<AndAPlusBMissingBInvalidSample>("RequireAllMembers", shouldBeValid: false, violations: 1, "missing B marker is rejected"),
            Backed<AndAPlusBMissingBothInvalidSample>("RequireAllMembers", shouldBeValid: false, violations: 2, "missing both marker kinds produces one violation per missing marker kind"),

            Backed<XorLeftValidSample>("RequireExclusiveChoice", shouldBeValid: true, violations: 0, "left choice alone is accepted"),
            Backed<XorRightValidSample>("RequireExclusiveChoice", shouldBeValid: true, violations: 0, "right choice alone is accepted"),
            Backed<XorBothInvalidSample>("RequireExclusiveChoice", shouldBeValid: false, violations: 1, "both choices together are rejected"),
            Backed<XorNoneInvalidSample>("RequireExclusiveChoice", shouldBeValid: false, violations: 1, "no choice is rejected"),

            Backed<TwoToFourValidTwoSample>("RequireMemberRange", shouldBeValid: true, violations: 0, "lower range bound is accepted"),
            Backed<TwoToFourValidFourSample>("RequireMemberRange", shouldBeValid: true, violations: 0, "upper range bound is accepted"),
            Backed<TwoToFourTooFewSample>("RequireMemberRange", shouldBeValid: false, violations: 1, "below range is rejected"),
            Backed<TwoToFourTooManySample>("RequireMemberRange", shouldBeValid: false, violations: 1, "above range is rejected"),

            Backed<NotValidSample>("ForbidMember", shouldBeValid: true, violations: 0, "absence of forbidden marker is accepted"),
            Backed<NotInvalidSample>("ForbidMember", shouldBeValid: false, violations: 1, "presence of forbidden marker is rejected"),

            Backed<RequiredNamedIdentifierValidSample>("RequireNamedMembers", shouldBeValid: true, violations: 0, "all configured marker names are present"),
            Backed<RequiredNamedIdentifierMissingYInvalidSample>("RequireNamedMembers", shouldBeValid: false, violations: 1, "missing configured marker name is rejected"),

            Backed<UniqueNamedIdentifierValidSample>("RequireUniqueNamedMember", shouldBeValid: true, violations: 0, "different marker names are accepted"),
            Backed<UniqueNamedIdentifierDuplicateNameInvalidSample>("RequireUniqueNamedMember", shouldBeValid: false, violations: 1, "duplicate marker name is rejected"),
            Backed<UniqueNamedIdentifierDuplicateUnnamedInvalidSample>("RequireUniqueNamedMember", shouldBeValid: false, violations: 1, "duplicate unnamed marker is rejected"),
        };

    private static MemberContractUseCaseExpectation Backed<TSample>(
        string contractName,
        bool shouldBeValid,
        int violations,
        string purpose) =>
        new(typeof(TSample), contractName, isAnalyzerBacked: true, shouldBeValid, violations, purpose);

}
