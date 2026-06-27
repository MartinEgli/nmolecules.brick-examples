using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.Infrastructure)]
internal sealed class DddInfrastructureAttribute : RoleAttribute
{
    public DddInfrastructureAttribute() : base(DddBrickRoles.Infrastructure)
    {
    }
}
