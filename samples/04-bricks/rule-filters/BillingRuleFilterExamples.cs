namespace Samples.Block04.Bricks;


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
