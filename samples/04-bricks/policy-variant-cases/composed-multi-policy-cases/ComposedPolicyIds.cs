using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.ComposedMultiPolicy;


public static class ComposedPolicyIds
{
    public const string PlatformPolicy = "COMPOSED-PLATFORM";
    public const string ProductPolicy = "COMPOSED-PRODUCT";
    public const string TeamPolicy = "COMPOSED-TEAM";
    public const string ExperimentalPolicy = "COMPOSED-EXPERIMENTAL";
    public const string ApplicationMustNotUseInfrastructure = "COMPOSED-001";
    public const string ApplicationNamespaceMustNotUseInfrastructureNamespace = "COMPOSED-002";
    public const string ApplicationRequiresDomain = "COMPOSED-003";
    public const string ExperimentalRule = "COMPOSED-999";
}
