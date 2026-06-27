using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.TypeCases;


[AttributeUsage(AttributeTargets.Struct)]
[RoleAlias(TypeCaseRoles.ValueObject)]
public sealed class SampleValueObjectAttribute : RoleAttribute
{
    public SampleValueObjectAttribute() : base(TypeCaseRoles.ValueObject)
    {
    }
}
