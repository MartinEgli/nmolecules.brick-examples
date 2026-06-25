using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.TypeCases;


[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(TypeCaseRoles.InfrastructureAdapter)]
public sealed class SampleInfrastructureAdapterAttribute : RoleAttribute
{
    public SampleInfrastructureAdapterAttribute() : base(TypeCaseRoles.InfrastructureAdapter)
    {
    }
}
