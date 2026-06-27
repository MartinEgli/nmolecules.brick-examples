using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(AttributeRoles.InfrastructureAdapter)]
public sealed class AttributeInfrastructureAdapterAttribute : RoleAttribute
{
    public AttributeInfrastructureAdapterAttribute() : base(AttributeRoles.InfrastructureAdapter)
    {
    }
}
