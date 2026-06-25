namespace Samples.Block04.Bricks;


public sealed class MemberContractUseCaseExpectation
{
    public MemberContractUseCaseExpectation(
        Type sampleType,
        string contractName,
        bool isAnalyzerBacked,
        bool shouldBeValid,
        int expectedAnalyzerViolationCount,
        string purpose)
    {
        SampleType = sampleType;
        ContractName = contractName;
        IsAnalyzerBacked = isAnalyzerBacked;
        ShouldBeValid = shouldBeValid;
        ExpectedAnalyzerViolationCount = expectedAnalyzerViolationCount;
        Purpose = purpose;
    }

    public Type SampleType { get; }

    public string ContractName { get; }

    public bool IsAnalyzerBacked { get; }

    public bool ShouldBeValid { get; }

    public int ExpectedAnalyzerViolationCount { get; }

    public string Purpose { get; }
}
