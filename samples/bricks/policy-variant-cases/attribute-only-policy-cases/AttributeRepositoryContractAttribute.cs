using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


[AttributeUsage(AttributeTargets.Interface)]
[RoleAlias(AttributeRoles.RepositoryContract)]
public sealed class AttributeRepositoryContractAttribute : RoleAttribute
{
    public AttributeRepositoryContractAttribute() : base(AttributeRoles.RepositoryContract)
    {
    }
}
