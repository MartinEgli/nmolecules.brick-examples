using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


[AttributeUsage(AttributeTargets.Struct)]
[RoleAlias(AttributeRoles.ValueObject)]
public sealed class AttributeValueObjectAttribute : RoleAttribute
{
    public AttributeValueObjectAttribute() : base(AttributeRoles.ValueObject)
    {
    }
}
