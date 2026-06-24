using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;

/// <summary>
/// Evaluates the DDD sample policy that is configured only through attributes.
/// </summary>
/// <remarks>
/// The policy source is limited to bracketed attributes such as
/// <c>[DddAggregateRoot]</c>, <c>[DddRepository]</c> and
/// <c>[assembly: Rule(...)]</c>. This class does not define additional rules in
/// code; it delegates policy construction to
/// <see cref="DddAttributeOnlyConfigurationExample"/>.
/// </remarks>
internal static class DddBrickPolicyExample
{
    public static BrickPolicy BuildPolicy()
    {
        return DddAttributeOnlyConfigurationExample.BuildPolicyFromAssemblyAttributes();
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
