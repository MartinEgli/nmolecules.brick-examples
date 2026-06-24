using NMolecules.Bricks;

namespace Samples.Block04.Bricks
{
    /// <summary>
    /// Central catalog of role identifiers used by the billing brick model.
    /// </summary>
    public static class BillingRoles
    {
        /// <summary>
        /// Domain role identifier as raw string for attribute usage.
        /// </summary>
        public const string Domain = "Billing.Domain";

        /// <summary>
        /// Domain role identifier as typed wrapper for regular code.
        /// </summary>
        public static RoleId DomainId => RoleId.From(Domain);

        /// <summary>
        /// Application role identifier as raw string for attribute usage.
        /// </summary>
        public const string Application = "Billing.Application";

        /// <summary>
        /// Application role identifier as typed wrapper for regular code.
        /// </summary>
        public static RoleId ApplicationId => RoleId.From(Application);

        /// <summary>
        /// Infrastructure role identifier as raw string for attribute usage.
        /// </summary>
        public const string Infrastructure = "Billing.Infrastructure";

        /// <summary>
        /// Infrastructure role identifier as typed wrapper for regular code.
        /// </summary>
        public static RoleId InfrastructureId => RoleId.From(Infrastructure);
    }
}
