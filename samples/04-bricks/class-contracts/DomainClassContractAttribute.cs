using NMolecules.Bricks;

namespace Samples.Block04.Bricks.ClassContracts;


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[RoleAlias(ClassContractRoles.DomainClass)]
public sealed class DomainClassContractAttribute : RoleAttribute
{
    public DomainClassContractAttribute() : base(ClassContractRoles.DomainClass)
    {
    }
}
