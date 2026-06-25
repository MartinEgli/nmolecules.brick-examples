using NMolecules.Bricks;

namespace Samples.Block04.Bricks.NamespaceContracts;


public static class NamespaceContractCases
{
    public static NamespaceContractCaseResult EvaluateValid() =>
        Evaluate(Dependency(NamespaceContractNames.Application, NamespaceContractNames.Domain));

    public static NamespaceContractCaseResult EvaluateInvalid() =>
        Evaluate(Dependency(NamespaceContractNames.Application, NamespaceContractNames.Infrastructure));

    private static NamespaceContractCaseResult Evaluate(params BrickDependency[] dependencies)
    {
        var policy = new BrickPolicy(
            BrickPolicyId.From(NamespaceContractRules.PolicyId),
            "Namespace contract policy",
            imports: null,
            rules: new[]
            {
                new BrickRule(
                    RuleId.From(NamespaceContractRules.ApplicationNamespaceMustNotUseInfrastructure),
                    "Application namespace must not depend on infrastructure namespace.",
                    RoleId.From(NamespaceContractRoles.ApplicationNamespace),
                    RoleId.From(NamespaceContractRoles.InfrastructureNamespace),
                    BrickDecision.Deny,
                    BrickScope.Namespace,
                    BrickSeverity.Error),
                new BrickRule(
                    RuleId.From(NamespaceContractRules.ApplicationNamespaceRequiresDomain),
                    "Application namespace requires a domain namespace dependency.",
                    RoleId.From(NamespaceContractRoles.ApplicationNamespace),
                    RoleId.From(NamespaceContractRoles.DomainNamespace),
                    BrickDecision.Require,
                    BrickScope.Namespace,
                    BrickSeverity.Warning)
            },
            BrickPermissionDefault.Allow,
            BrickEnforcementMode.Analyze);
        var roles = new[]
        {
            Resolved(NamespaceContractNames.Application, NamespaceContractRoles.ApplicationNamespace),
            Resolved(NamespaceContractNames.Domain, NamespaceContractRoles.DomainNamespace),
            Resolved(NamespaceContractNames.Infrastructure, NamespaceContractRoles.InfrastructureNamespace)
        };

        return new NamespaceContractCaseResult(
            policy,
            dependencies,
            roles,
            BrickRuleEvaluator.Evaluate(policy, dependencies, roles));
    }

    private static BrickDependency Dependency(string sourceNamespace, string targetNamespace) =>
        new(
            Element(sourceNamespace),
            Element(targetNamespace),
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            BrickScope.Namespace,
            BrickDependencyLayer.Static,
            BrickDependencyStrength.Direct,
            BrickEvidenceLevel.AnalyzerInferred);

    private static BrickResolvedRoles Resolved(string namespaceName, string role)
    {
        var element = Element(namespaceName);
        var assignment = new BrickRoleAssignment(
            new BrickElementSelector(element.Kind, element.Id.Value, element.AssemblyName),
            RoleId.From(role),
            BrickAssignmentMode.ExternalConfiguration,
            BrickAssignmentSource.PolicyFile,
            new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Namespace, BrickAssignmentAuthority.External),
            BrickAssignmentBehavior.Apply);

        return new BrickResolvedRoles(element, new[] { assignment }, new[] { assignment }, null, null);
    }

    private static BrickElement Element(string namespaceName) =>
        new(
            BrickElementId.From("namespace:" + namespaceName),
            BrickElementKind.Namespace,
            namespaceName,
            "Samples.Block04.Bricks.NamespaceContracts",
            namespaceName,
            namespaceName);
}
