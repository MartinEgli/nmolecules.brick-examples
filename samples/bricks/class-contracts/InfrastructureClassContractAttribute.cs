using NMolecules.Bricks;

namespace Samples.Block04.Bricks.ClassContracts;


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[RoleAlias(ClassContractRoles.InfrastructureClass)]
public sealed class InfrastructureClassContractAttribute : RoleAttribute
{
    public InfrastructureClassContractAttribute() : base(ClassContractRoles.InfrastructureClass)
    {
    }
}
