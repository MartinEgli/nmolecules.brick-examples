namespace Samples.Block04.Bricks
{
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

    /// <summary>
    /// Domain policy whose type name intentionally contains both "Legacy" and
    /// "Contract" so it can be used to explain how required and excluded source
    /// name filters interact.
    /// </summary>
    [BillingDomainRole]
    public sealed class LegacyContractLifecyclePolicy
    {
    }

    /// <summary>
    /// Infrastructure type whose name intentionally contains "Repository" so it
    /// can be used to explain <c>[RequiredTargetNameContains(..., "Repository")]</c>.
    /// </summary>
    [BillingInfrastructureRole]
    public sealed class InvoiceRepositoryGateway
    {
    }

    /// <summary>
    /// Infrastructure type whose name intentionally contains "Facade" so it can
    /// be used to explain <c>[ExcludedTargetNameContains(..., "Facade")]</c>.
    /// </summary>
    [BillingInfrastructureRole]
    public sealed class InvoiceFacadeGateway
    {
    }

    /// <summary>
    /// Small scenario index for the rule-filter examples in this file.
    /// These examples are naming-oriented on purpose: the assembly rules in
    /// <c>BillingBrickRules.Assembly.cs</c> reference the same tokens, and this
    /// file provides concrete class and member names that match them.
    /// </summary>
    internal static class BillingRuleFilterExamples
    {
        /*
         * Active sample mapping:
         *
         * - BILL-BRICK-001:
         *   shows the custom message field via the baseline rule message
         *   "Rule {rule}: {source} must not depend on {target} ({member})."
         *
        * - BILL-BRICK-002:
         *   [ExcludedSourceNameContains(BILL-BRICK-002, "Legacy")]
         *   -> LegacyContractLifecyclePolicy
         *
         * - BILL-BRICK-003:
         *   [ExcludedTargetNameContains(BILL-BRICK-003, "Facade")]
         *   -> InvoiceFacadeGateway
         *
         * - BILL-BRICK-004:
         *   [ExcludedMemberNameContains(BILL-BRICK-004, "Allow")]
         *   -> ContractLifecyclePolicy.AllowManualImport()
         *
         * - BILL-BRICK-005:
         *   [RequiredSourceNameContains(BILL-BRICK-005, "Contract")]
         *   -> ContractLifecyclePolicy and LegacyContractLifecyclePolicy
         *
         * - BILL-BRICK-006:
         *   [RequiredTargetNameContains(BILL-BRICK-006, "Repository")]
         *   -> InvoiceRepositoryGateway
         */
    }
}
