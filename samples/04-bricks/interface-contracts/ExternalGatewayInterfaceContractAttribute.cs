using NMolecules.Bricks;

namespace Samples.Block04.Bricks.InterfaceContracts;


[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
[RoleAlias(InterfaceContractRoles.ExternalGatewayInterface)]
public sealed class ExternalGatewayInterfaceContractAttribute : RoleAttribute
{
    public ExternalGatewayInterfaceContractAttribute() : base(InterfaceContractRoles.ExternalGatewayInterface)
    {
    }
}
