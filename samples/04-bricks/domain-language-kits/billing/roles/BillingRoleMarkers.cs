using NMolecules.Bricks;

namespace Samples.Block04.Bricks
{
    /// <summary>
    /// Domain marker for billing components.
    /// </summary>
    /// <remarks>
    /// This is an alias-style marker. The attribute itself does not inherit from
    /// <see cref="RoleAttribute"/>; instead, <see cref="RoleAliasAttribute"/>
    /// maps this custom marker to the effective role identifier.
    ///
    /// Use this pattern when you want a highly readable, domain-specific marker
    /// such as <c>[BillingDomainRole]</c> instead of the generic
    /// <c>[Role("Billing.Domain")]</c>.
    /// </remarks>
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Interface |
        AttributeTargets.Struct)]
    [RoleAlias(BillingRoles.Domain)]
    public sealed class BillingDomainRoleAttribute : Attribute
    {
    }

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

    /// <summary>
    /// Application marker for orchestration and use-case components.
    /// </summary>
    /// <remarks>
    /// This keeps the consumer-side syntax short and expressive while still
    /// resolving to the underlying <c>Billing.Application</c> role.
    ///
    /// If you want to place roles directly on target types without a custom
    /// marker, you would use <see cref="RoleAttribute"/> on the target type
    /// itself instead.
    /// </remarks>
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Interface |
        AttributeTargets.Struct)]
    [RoleAlias(BillingRoles.Application)]
    public sealed class BillingApplicationRoleAttribute : Attribute
    {
    }
}
