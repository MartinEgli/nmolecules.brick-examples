using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.NamespaceAssemblyExternalCases;


/// <summary>Rule identifiers used by the namespace, assembly and external sample.</summary>
public static class NamespaceAssemblyExternalRules
{
    public const string PolicyId = "SOURCE-TARGET-NAMESPACE-ASSEMBLY-POLICY";
    public const string DomainNamespaceMustNotUseInfrastructureNamespace = "SOURCE-TARGET-NAMESPACE-001";
    public const string DomainAssemblyMustNotUseExternalPackage = "SOURCE-TARGET-ASSEMBLY-001";
    public const string ApplicationAssemblyRequiresContractsAssembly = "SOURCE-TARGET-ASSEMBLY-002";
}
