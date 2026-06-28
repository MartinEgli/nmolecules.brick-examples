using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.Entity)]
[RequireExactlyOneMember(typeof(DddIdentifierAttribute))]
internal sealed class DddEntityAttribute : RoleAttribute
{
    public DddEntityAttribute() : base(DddBrickRoles.Entity)
    {
    }
}
