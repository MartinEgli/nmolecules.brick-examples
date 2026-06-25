using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.ApplicationService)]
internal sealed class DddApplicationServiceAttribute : RoleAttribute
{
    public DddApplicationServiceAttribute() : base(DddBrickRoles.ApplicationService)
    {
    }
}
