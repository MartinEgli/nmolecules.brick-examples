using NMolecules.Bricks;

namespace Samples.Block04.Bricks;

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
