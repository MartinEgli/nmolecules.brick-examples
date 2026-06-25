using NMolecules.Bricks;

namespace Samples.Block04.Bricks.InterfaceContracts;


[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
[RoleAlias(InterfaceContractRoles.RepositoryInterface)]
public sealed class RepositoryInterfaceContractAttribute : RoleAttribute
{
    public RepositoryInterfaceContractAttribute() : base(InterfaceContractRoles.RepositoryInterface)
    {
    }
}
