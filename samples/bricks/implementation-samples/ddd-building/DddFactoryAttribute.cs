using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.Factory)]
internal sealed class DddFactoryAttribute : RoleAttribute
{
    public DddFactoryAttribute() : base(DddBrickRoles.Factory)
    {
    }
}
