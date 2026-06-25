using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.TypeCases;


[AttributeUsage(AttributeTargets.Interface)]
[RoleAlias(TypeCaseRoles.RepositoryContract)]
public sealed class SampleRepositoryContractAttribute : RoleAttribute
{
    public SampleRepositoryContractAttribute() : base(TypeCaseRoles.RepositoryContract)
    {
    }
}
