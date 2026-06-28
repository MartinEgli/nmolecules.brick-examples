using NMolecules.Bricks;

namespace Samples.Bricks.Analyzers.GenericRoleCombinations;

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(GenericRoleNames.UserInterfaceEndpoint)]
public sealed class UserInterfaceEndpointAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(GenericRoleNames.DatabaseAdapter)]
public sealed class DatabaseAdapterAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(GenericRoleNames.VendorSdkAdapter)]
public sealed class VendorSdkAdapterAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(GenericRoleNames.CheckoutFeatureInternalApi)]
public sealed class CheckoutFeatureInternalApiAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(GenericRoleNames.BillingFeatureInternalApi)]
public sealed class BillingFeatureInternalApiAttribute : Attribute
{
}
