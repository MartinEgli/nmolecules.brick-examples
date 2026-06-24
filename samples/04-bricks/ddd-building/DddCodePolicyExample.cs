using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;

/// <summary>
/// Defines the DDD sample policy directly as C# policy objects.
/// </summary>
/// <remarks>
/// Use this style when a tool, generator or test wants to compose policies in
/// code instead of reading them from source attributes. The policy definition
/// in this sample is intentionally made of C# object blocks using <c>{ ... }</c>
/// and does not use policy attributes such as <c>[assembly: Rule(...)]</c>.
/// </remarks>
internal static class DddCodePolicyExample
{
    public static BrickPolicy BuildPolicy()
    {
        var rules = new[]
        {
            new BrickRule(
                RuleId.From(DddBrickRules.DomainMustNotDependOnInfrastructure),
                "Aggregate root must not depend on infrastructure",
                RoleId.From(DddBrickRoles.AggregateRoot),
                RoleId.From(DddBrickRoles.Infrastructure),
                BrickDecision.Deny,
                BrickScope.Type,
                BrickSeverity.Error),
            new BrickRule(
                RuleId.From(DddBrickRules.ValueObjectMustNotDependOnRepository),
                "Value object must not depend on repository",
                RoleId.From(DddBrickRoles.ValueObject),
                RoleId.From(DddBrickRoles.Repository),
                BrickDecision.Deny,
                BrickScope.Type,
                BrickSeverity.Error),
            new BrickRule(
                RuleId.From(DddBrickRules.ApplicationServiceRequiresRepositoryContract),
                "Application service requires repository contract",
                RoleId.From(DddBrickRoles.ApplicationService),
                RoleId.From(DddBrickRoles.Repository),
                BrickDecision.Require,
                BrickScope.Type,
                BrickSeverity.Warning)
        };
        var combinationRules = new[]
        {
            new BrickRoleCombinationRule(
                "Domain model cannot be infrastructure",
                BrickRoleSelector.From("DDD.*"),
                BrickRoleSelector.From(DddBrickRoles.Infrastructure),
                BrickCombinationKind.Incompatible,
                "A DDD building block should not also own infrastructure responsibilities.")
        };

        return new BrickPolicy(
            BrickPolicyId.From("sample-ddd-code-policy"),
            "Sample DDD policy defined in code",
            imports: null,
            rules: rules,
            combinationRules: combinationRules,
            externalAssignments: null,
            aliases: null,
            defaultDecision: BrickPermissionDefault.Allow,
            enforcement: BrickEnforcementMode.Analyze);
    }

    public static IReadOnlyList<BrickViolation> EvaluateExampleDependency()
    {
        var aggregate = Type("Contract");
        var repository = Type("IContractRepository");
        var sqlRepository = Type("SqlContractRepository");
        var applicationService = Type("ContractApplicationService");

        var roles = new[]
        {
            Resolved(aggregate, DddBrickRoles.AggregateRoot),
            Resolved(repository, DddBrickRoles.Repository),
            Resolved(sqlRepository, DddBrickRoles.Infrastructure),
            Resolved(applicationService, DddBrickRoles.ApplicationService)
        };
        var dependencies = new[]
        {
            Dependency(aggregate, sqlRepository),
            Dependency(applicationService, repository)
        };

        return BrickRuleEvaluator.Evaluate(BuildPolicy(), dependencies, roles);
    }

    public static BrickRoleAssignment[] CreateAssignmentsFromDddAttributeNames()
    {
        var element = new BrickElement(
            BrickElementId.From("type:Samples.Contract"),
            BrickElementKind.Type,
            "Contract",
            "Samples.Block04.Bricks",
            "Samples.Block04.Bricks.DddBuilding",
            "Samples.Block04.Bricks.DddBuilding.Contract");

        return BrickBuiltInAttributeRoleBridges.Ddd
            .CreateAssignments(element, new[] { "AggregateRootAttribute", "EntityAttribute" })
            .ToArray();
    }

    private static BrickElement Type(string name) =>
        new(
            BrickElementId.From("type:" + name),
            BrickElementKind.Type,
            name,
            "Samples.Block04.Bricks",
            "Samples.Block04.Bricks.DddBuilding",
            "Samples.Block04.Bricks.DddBuilding." + name);

    private static BrickResolvedRoles Resolved(BrickElement element, string roleId)
    {
        var assignment = new BrickRoleAssignment(
            new BrickElementSelector(BrickElementKind.Type, element.Id.Value, element.AssemblyName),
            RoleId.From(roleId),
            BrickAssignmentMode.DirectAttribute,
            BrickAssignmentSource.SourceAttribute,
            new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.Direct),
            BrickAssignmentBehavior.Apply);

        return new BrickResolvedRoles(element, new[] { assignment }, new[] { assignment }, null, null);
    }

    private static BrickDependency Dependency(BrickElement source, BrickElement target) =>
        new(
            source,
            target,
            BrickDependencyKindId.From("TypeReference"),
            BrickScope.Type,
            BrickDependencyLayer.Static,
            BrickDependencyStrength.Direct,
            BrickEvidenceLevel.CompilerConfirmed);
}
