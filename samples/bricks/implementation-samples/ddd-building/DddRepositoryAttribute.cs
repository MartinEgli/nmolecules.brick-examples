using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.Repository)]
internal sealed class DddRepositoryAttribute : RoleAttribute
{
    public DddRepositoryAttribute() : base(DddBrickRoles.Repository)
    {
    }
}
