using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[RoleAlias(DddBrickRoles.ValueObject)]
internal sealed class DddValueObjectAttribute : RoleAttribute
{
    public DddValueObjectAttribute() : base(DddBrickRoles.ValueObject)
    {
    }
}
