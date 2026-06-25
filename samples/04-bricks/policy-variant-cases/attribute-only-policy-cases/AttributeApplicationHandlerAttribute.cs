using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(AttributeRoles.ApplicationHandler)]
public sealed class AttributeApplicationHandlerAttribute : RoleAttribute
{
    public AttributeApplicationHandlerAttribute() : base(AttributeRoles.ApplicationHandler)
    {
    }
}
