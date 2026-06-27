using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


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
