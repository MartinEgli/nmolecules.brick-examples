using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public static class AttributeMultiRuleIds
{
    public const string ApplicationMustNotUseInfrastructure = "ATTR-MULTI-001";
    public const string ApplicationNamespaceMustNotUseInfrastructureNamespace = "ATTR-MULTI-002";
    public const string ApplicationRequiresDomain = "ATTR-MULTI-003";
    public const string ExperimentalRule = "ATTR-MULTI-999";
}
