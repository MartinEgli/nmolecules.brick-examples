using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.TypeCases;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(TypeCaseRoles.ApplicationService)]
public sealed class SampleApplicationServiceAttribute : RoleAttribute
{
    public SampleApplicationServiceAttribute() : base(TypeCaseRoles.ApplicationService)
    {
    }
}
