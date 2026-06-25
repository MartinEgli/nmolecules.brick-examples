using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.NamespaceAssemblyExternalCases;


/// <summary>
/// Demonstrates namespace, assembly and external-reference source/target pairs.
/// </summary>
public static class NamespaceAssemblyExternalSourceTargetCases
{
    /// <summary>
    /// Builds rules for larger architectural boundaries.
    /// </summary>
    public static BrickPolicy BuildPolicy() =>
        new(
            BrickPolicyId.From(NamespaceAssemblyExternalRules.PolicyId),
            "Namespace, assembly and external reference cases",
            imports: null,
            rules: new[]
            {
                new BrickRule(
                    RuleId.From(NamespaceAssemblyExternalRules.DomainNamespaceMustNotUseInfrastructureNamespace),
                    "Domain namespace must not depend on infrastructure namespace",
                    RoleId.From(NamespaceAssemblyExternalRoles.DomainNamespace),
                    RoleId.From(NamespaceAssemblyExternalRoles.InfrastructureNamespace),
                    BrickDecision.Deny,
                    BrickScope.Namespace,
                    BrickSeverity.Error),
                new BrickRule(
                    RuleId.From(NamespaceAssemblyExternalRules.DomainAssemblyMustNotUseExternalPackage),
                    "Domain assembly must not depend on external package",
                    RoleId.From(NamespaceAssemblyExternalRoles.DomainAssembly),
                    RoleId.From(NamespaceAssemblyExternalRoles.ExternalPackage),
                    BrickDecision.Deny,
                    BrickScope.Assembly,
                    BrickSeverity.Error),
                new BrickRule(
                    RuleId.From(NamespaceAssemblyExternalRules.ApplicationAssemblyRequiresContractsAssembly),
                    "Application assembly must depend on contracts assembly",
                    RoleId.From(NamespaceAssemblyExternalRoles.ApplicationAssembly),
                    RoleId.From(NamespaceAssemblyExternalRoles.ContractsAssembly),
                    BrickDecision.Require,
                    BrickScope.Assembly,
                    BrickSeverity.Warning)
            },
            defaultDecision: BrickPermissionDefault.Allow,
            enforcement: BrickEnforcementMode.Analyze);

    /// <summary>
    /// Creates namespace-to-namespace and assembly-to-external violations plus
    /// an assembly-to-assembly dependency that satisfies the requirement.
    /// </summary>
    public static NamespaceAssemblyExternalCaseResult Evaluate()
    {
        var domainNamespace = Element(
            "namespace:Samples.Orders.Domain",
            BrickElementKind.Namespace,
            "Samples.Orders.Domain",
            "Samples.Orders.Domain",
            "Samples.Orders.Domain",
            "Samples.Orders.Domain");
        var infrastructureNamespace = Element(
            "namespace:Samples.Orders.Infrastructure",
            BrickElementKind.Namespace,
            "Samples.Orders.Infrastructure",
            "Samples.Orders.Infrastructure",
            "Samples.Orders.Infrastructure",
            "Samples.Orders.Infrastructure");
        var domainAssembly = Element(
            "assembly:Samples.Orders.Domain",
            BrickElementKind.Assembly,
            "Samples.Orders.Domain",
            "Samples.Orders.Domain",
            null,
            "Samples.Orders.Domain");
        var applicationAssembly = Element(
            "assembly:Samples.Orders.Application",
            BrickElementKind.Assembly,
            "Samples.Orders.Application",
            "Samples.Orders.Application",
            null,
            "Samples.Orders.Application");
        var contractsAssembly = Element(
            "assembly:Samples.Orders.Contracts",
            BrickElementKind.Assembly,
            "Samples.Orders.Contracts",
            "Samples.Orders.Contracts",
            null,
            "Samples.Orders.Contracts");
        var jsonPackage = Element(
            "external:Newtonsoft.Json",
            BrickElementKind.ExternalReference,
            "Newtonsoft.Json",
            "Newtonsoft.Json",
            "Newtonsoft.Json",
            "Newtonsoft.Json");
        var dependencies = new[]
        {
            Dependency(domainNamespace, infrastructureNamespace, BrickScope.Namespace, BrickDependencyLayer.Static),
            Dependency(domainAssembly, jsonPackage, BrickScope.Assembly, BrickDependencyLayer.Static),
            Dependency(applicationAssembly, contractsAssembly, BrickScope.Assembly, BrickDependencyLayer.Static)
        };
        var roles = new[]
        {
            Resolved(domainNamespace, NamespaceAssemblyExternalRoles.DomainNamespace),
            Resolved(infrastructureNamespace, NamespaceAssemblyExternalRoles.InfrastructureNamespace),
            Resolved(domainAssembly, NamespaceAssemblyExternalRoles.DomainAssembly),
            Resolved(applicationAssembly, NamespaceAssemblyExternalRoles.ApplicationAssembly),
            Resolved(contractsAssembly, NamespaceAssemblyExternalRoles.ContractsAssembly),
            Resolved(jsonPackage, NamespaceAssemblyExternalRoles.ExternalPackage)
        };
        var violations = BrickRuleEvaluator.Evaluate(BuildPolicy(), dependencies, roles);

        return new NamespaceAssemblyExternalCaseResult(
            new[]
            {
                domainNamespace,
                infrastructureNamespace,
                domainAssembly,
                applicationAssembly,
                contractsAssembly,
                jsonPackage
            },
            dependencies,
            roles,
            violations);
    }

    private static BrickDependency Dependency(
        BrickElement source,
        BrickElement target,
        BrickScope scope,
        BrickDependencyLayer layer) =>
        new(
            source,
            target,
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            scope,
            layer,
            BrickDependencyStrength.Direct,
            BrickEvidenceLevel.AnalyzerInferred);

    private static BrickElement Element(
        string id,
        BrickElementKind kind,
        string displayName,
        string? assemblyName,
        string? namespaceName,
        string? fullName) =>
        new(
            BrickElementId.From(id),
            kind,
            displayName,
            assemblyName,
            namespaceName,
            fullName);

    private static BrickResolvedRoles Resolved(BrickElement element, string role)
    {
        var assignment = new BrickRoleAssignment(
            new BrickElementSelector(element.Kind, element.Id.Value, element.AssemblyName),
            RoleId.From(role),
            BrickAssignmentMode.DirectAttribute,
            BrickAssignmentSource.SourceAttribute,
            new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.Direct),
            BrickAssignmentBehavior.Apply);

        return new BrickResolvedRoles(element, new[] { assignment }, new[] { assignment }, null, null);
    }
}
