namespace Samples.Block04.Bricks;

    /// <summary>
    /// Domain policy whose type name intentionally contains "Contract" so it can
    /// be used to explain <c>[RequiredSourceNameContains(..., "Contract")]</c>.
    /// </summary>
    [BillingDomainRole]
    public sealed class ContractLifecyclePolicy
    {
        /// <summary>
        /// Member name intentionally contains "Allow" so it can be used to
        /// explain <c>[ExcludedMemberNameContains(..., "Allow")]</c>.
        /// </summary>
        public void AllowManualImport()
        {
        }

        /// <summary>
        /// Same shape as <see cref="AllowManualImport"/> but without the
        /// exclusion token in the member name.
        /// </summary>
        public void EnforceInvoiceSync()
        {
        }
    }
