using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[RoleAlias(DddBrickRoles.AggregateRoot)]
[RequireExactlyOneMember(typeof(DddIdentifierAttribute))]
internal sealed class DddAggregateRootAttribute : RoleAttribute
{
    public DddAggregateRootAttribute() : base(DddBrickRoles.AggregateRoot)
    {
    }
}
