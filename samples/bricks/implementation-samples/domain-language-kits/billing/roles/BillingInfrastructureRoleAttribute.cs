using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


    /// <summary>
    /// Infrastructure marker for technical adapters and implementations.
    /// </summary>
    /// <remarks>
    /// This uses the same alias-style approach as
    /// <see cref="BillingDomainRoleAttribute"/>. The effective role comes from
    /// <see cref="RoleAliasAttribute"/>, not from a direct
    /// <see cref="RoleAttribute"/> base class.
    /// </remarks>
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Interface |
        AttributeTargets.Struct)]
    [RoleAlias(BillingRoles.Infrastructure)]
    public sealed class BillingInfrastructureRoleAttribute : Attribute
    {
    }
