using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.ClassContracts;


public static class ClassContractCases
{
    private static readonly Type[] ContractTypes =
    {
        typeof(ValidApplicationClassSample),
        typeof(InvalidApplicationClassSample),
        typeof(DomainClassSample),
        typeof(InfrastructureClassSample)
    };

    public static ClassContractCaseResult Evaluate()
    {
        var policy = new BrickPolicy(
            BrickPolicyId.From(ClassContractRules.PolicyId),
            "Class contract policy",
            imports: null,
            rules: new[]
            {
                new BrickRule(
                    RuleId.From(ClassContractRules.ApplicationMustNotUseInfrastructure),
                    "Application class must not depend on infrastructure class.",
                    RoleId.From(ClassContractRoles.ApplicationClass),
                    RoleId.From(ClassContractRoles.InfrastructureClass),
                    BrickDecision.Deny,
                    BrickScope.Type,
                    BrickSeverity.Error),
                new BrickRule(
                    RuleId.From(ClassContractRules.ApplicationRequiresDomain),
                    "Application class requires a domain class dependency.",
                    RoleId.From(ClassContractRoles.ApplicationClass),
                    RoleId.From(ClassContractRoles.DomainClass),
                    BrickDecision.Require,
                    BrickScope.Type,
                    BrickSeverity.Warning)
            },
            BrickPermissionDefault.Allow,
            BrickEnforcementMode.Analyze);
        var dependencies = new[]
        {
            Dependency(typeof(ValidApplicationClassSample), typeof(DomainClassSample)),
            Dependency(typeof(InvalidApplicationClassSample), typeof(InfrastructureClassSample))
        };
        var roles = ContractTypes.SelectMany(ResolveRoles).ToArray();

        return new ClassContractCaseResult(
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
