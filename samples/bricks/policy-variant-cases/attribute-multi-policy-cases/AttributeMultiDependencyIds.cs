using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public static class AttributeMultiDependencyIds
{
    public const string ApplicationToDomain = "ATTR-MULTI-DEP-001";
    public const string ApplicationToInfrastructure = "ATTR-MULTI-DEP-002";
    public const string ApplicationNamespaceToInfrastructureNamespace = "ATTR-MULTI-DEP-003";
    public const string AssemblyPrefix = "ATTR-MULTI-ASM-DEP-";
    public const string AssemblyTeamApplicationToDomain = AssemblyPrefix + "TEAM-001";
    public const string AssemblyTeamApplicationToInfrastructure = AssemblyPrefix + "TEAM-002";
    public const string AssemblyTeamApplicationNamespaceToInfrastructureNamespace = AssemblyPrefix + "TEAM-003";
}
