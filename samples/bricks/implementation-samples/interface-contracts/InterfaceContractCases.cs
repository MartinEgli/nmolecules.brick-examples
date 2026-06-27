using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.InterfaceContracts;


public static class InterfaceContractCases
{
    private static readonly Type[] ContractTypes =
    {
        typeof(ValidInterfaceConsumerSample),
        typeof(InvalidInterfaceConsumerSample),
        typeof(IRepositoryContractSample),
        typeof(IExternalGatewayContractSample)
    };

    public static InterfaceContractCaseResult Evaluate()
    {
        var policy = new BrickPolicy(
            BrickPolicyId.From(InterfaceContractRules.PolicyId),
            "Interface contract policy",
            imports: null,
            rules: new[]
            {
                new BrickRule(
                    RuleId.From(InterfaceContractRules.ApplicationRequiresRepositoryInterface),
                    "Application class requires a repository interface contract.",
                    RoleId.From(InterfaceContractRoles.ApplicationClass),
                    RoleId.From(InterfaceContractRoles.RepositoryInterface),
                    BrickDecision.Require,
                    BrickScope.Type,
                    BrickSeverity.Warning),
                new BrickRule(
                    RuleId.From(InterfaceContractRules.ApplicationMustNotUseExternalGatewayInterface),
                    "Application class must not depend on an external gateway interface.",
                    RoleId.From(InterfaceContractRoles.ApplicationClass),
                    RoleId.From(InterfaceContractRoles.ExternalGatewayInterface),
                    BrickDecision.Deny,
                    BrickScope.Type,
                    BrickSeverity.Error)
            },
            BrickPermissionDefault.Allow,
            BrickEnforcementMode.Analyze);
        var dependencies = new[]
        {
            Dependency(typeof(ValidInterfaceConsumerSample), typeof(IRepositoryContractSample)),
            Dependency(typeof(InvalidInterfaceConsumerSample), typeof(IExternalGatewayContractSample))
        };
        var roles = ContractTypes.SelectMany(ResolveRoles).ToArray();

        return new InterfaceContractCaseResult(
            policy,
            dependencies,
            roles,
            BrickRuleEvaluator.Evaluate(policy, dependencies, roles));
    }

    private static IEnumerable<BrickResolvedRoles> ResolveRoles(Type type)
    {
        var element = Element(type);
        return type.GetCustomAttributes<RoleAttribute>(inherit: false)
            .Select(role =>
            {
                var assignment = new BrickRoleAssignment(
                    new BrickElementSelector(element.Kind, element.Id.Value, element.AssemblyName),
                    role.Id,
                    BrickAssignmentMode.DirectAttribute,
                    BrickAssignmentSource.SourceAttribute,
                    new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.Direct),
                    BrickAssignmentBehavior.Apply);

                return new BrickResolvedRoles(element, new[] { assignment }, new[] { assignment }, null, null);
            });
    }

    private static BrickDependency Dependency(Type source, Type target) =>
        new(
            Element(source),
            Element(target),
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            BrickScope.Type,
            BrickDependencyLayer.Static,
            BrickDependencyStrength.Direct,
            BrickEvidenceLevel.CompilerConfirmed);

    private static BrickElement Element(Type type) =>
        new(
            BrickElementId.From("type:" + type.FullName),
            BrickElementKind.Type,
            type.Name,
            type.Assembly.GetName().Name,
            type.Namespace,
            type.FullName);
}
