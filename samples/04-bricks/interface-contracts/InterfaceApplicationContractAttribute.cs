using NMolecules.Bricks;

namespace Samples.Block04.Bricks.InterfaceContracts;


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[RoleAlias(InterfaceContractRoles.ApplicationClass)]
public sealed class InterfaceApplicationContractAttribute : RoleAttribute
{
    public InterfaceApplicationContractAttribute() : base(InterfaceContractRoles.ApplicationClass)
    {
    }
}
