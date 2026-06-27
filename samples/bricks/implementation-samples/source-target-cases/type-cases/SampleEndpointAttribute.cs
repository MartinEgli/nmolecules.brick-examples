using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.TypeCases;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(TypeCaseRoles.Endpoint)]
public sealed class SampleEndpointAttribute : RoleAttribute
{
    public SampleEndpointAttribute() : base(TypeCaseRoles.Endpoint)
    {
    }
}
