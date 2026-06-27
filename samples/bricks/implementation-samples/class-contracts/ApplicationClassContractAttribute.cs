using NMolecules.Bricks;

namespace Samples.Block04.Bricks.ClassContracts;


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[RoleAlias(ClassContractRoles.ApplicationClass)]
public sealed class ApplicationClassContractAttribute : RoleAttribute
{
    public ApplicationClassContractAttribute() : base(ClassContractRoles.ApplicationClass)
    {
    }
}
