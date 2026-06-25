using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public static class AttributeMultiPolicyIds
{
    public const string PlatformPolicy = "ATTR-MULTI-PLATFORM";
    public const string ProductPolicy = "ATTR-MULTI-PRODUCT";
    public const string TeamPolicy = "ATTR-MULTI-TEAM";
    public const string ExperimentalPolicy = "ATTR-MULTI-EXPERIMENTAL";
    public const string AssemblyPlatformPolicy = "ATTR-MULTI-ASM-PLATFORM";
    public const string AssemblyProductPolicy = "ATTR-MULTI-ASM-PRODUCT";
    public const string AssemblyTeamPolicy = "ATTR-MULTI-ASM-TEAM";
    public const string AssemblyPrefix = "ATTR-MULTI-ASM-";
}
