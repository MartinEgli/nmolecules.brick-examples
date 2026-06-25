using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.DomainService)]
internal sealed class DddDomainServiceAttribute : RoleAttribute
{
    public DddDomainServiceAttribute() : base(DddBrickRoles.DomainService)
    {
    }
}
