namespace Samples.Block04.Bricks;


    /// <summary>
    /// Domain policy whose type name intentionally contains both "Legacy" and
    /// "Contract" so it can be used to explain how required and excluded source
    /// name filters interact.
    /// </summary>
    [BillingDomainRole]
    public sealed class LegacyContractLifecyclePolicy
    {
    }
