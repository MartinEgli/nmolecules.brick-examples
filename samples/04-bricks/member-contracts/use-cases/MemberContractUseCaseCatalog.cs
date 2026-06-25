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

            Concept<TwoToFourValidTwoSample>("RequireMemberRange", shouldBeValid: true, "lower range bound is accepted by the concept"),
            Concept<TwoToFourValidFourSample>("RequireMemberRange", shouldBeValid: true, "upper range bound is accepted by the concept"),
            Concept<TwoToFourTooFewSample>("RequireMemberRange", shouldBeValid: false, "below range is rejected by the concept"),
            Concept<TwoToFourTooManySample>("RequireMemberRange", shouldBeValid: false, "above range is rejected by the concept"),

            Concept<NotValidSample>("ForbidMember", shouldBeValid: true, "absence of forbidden marker is accepted by the concept"),
            Concept<NotInvalidSample>("ForbidMember", shouldBeValid: false, "presence of forbidden marker is rejected by the concept"),
        };

    private static MemberContractUseCaseExpectation Backed<TSample>(
        string contractName,
        bool shouldBeValid,
        int violations,
        string purpose) =>
        new(typeof(TSample), contractName, isAnalyzerBacked: true, shouldBeValid, violations, purpose);

    private static MemberContractUseCaseExpectation Concept<TSample>(
        string contractName,
        bool shouldBeValid,
        string purpose) =>
        new(typeof(TSample), contractName, isAnalyzerBacked: false, shouldBeValid, expectedAnalyzerViolationCount: -1, purpose);
}
