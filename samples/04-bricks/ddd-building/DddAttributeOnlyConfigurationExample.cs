using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;

/// <summary>
/// Shows the same DDD slice configured only through attributes.
/// </summary>
/// <remarks>
/// The roles come from custom role marker attributes such as
/// <see cref="DddAggregateRootAttribute"/> and <see cref="DddRepositoryAttribute"/>.
/// The rules come from assembly-level <see cref="RuleAttribute"/> declarations in
/// <c>DddBrickRules.Assembly.cs</c>. The example creates a <see cref="BrickPolicy"/>
/// from those attributes so the evaluator can run, but it does not configure any
/// rule manually in code.
/// </remarks>
internal static class DddAttributeOnlyConfigurationExample
{
    private static readonly Type[] DddTypes =
    {
        typeof(Contract),
        typeof(ContractLine),
        typeof(ContractId),
        typeof(CustomerId),
        typeof(Money),
        typeof(IContractRepository),
        typeof(ContractFactory),
        typeof(ContractPricingPolicy),
        typeof(ContractApplicationService),
        typeof(InMemoryContractRepository)
    };

    public static DddAttributeOnlyConfiguration ReadConfigurationFromAttributes()
    {
        var assemblyRules = typeof(DddAttributeOnlyConfigurationExample)
            .Assembly
            .GetCustomAttributes<RuleAttribute>()
            .Where(rule => rule.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .OrderBy(rule => rule.Id, StringComparer.Ordinal)
            .ToArray();

        var roleAssignments = DddTypes
            .Select(type => new DddTypeRoleAssignment(
                type.Name,
                type.GetCustomAttributes<RoleAttribute>(inherit: false)
                    .Select(attribute => attribute.Id)
                    .OrderBy(role => role.Value, StringComparer.Ordinal)
                    .ToArray()))
            .OrderBy(assignment => assignment.TypeName, StringComparer.Ordinal)
            .ToArray();

        return new DddAttributeOnlyConfiguration(assemblyRules, roleAssignments);
    }

    public static BrickPolicy BuildPolicyFromAssemblyAttributes()
    {
        var rules = ReadConfigurationFromAttributes()
            .Rules
            .Select(ToBrickRule)
            .ToArray();

        return new BrickPolicy(
            BrickPolicyId.From("sample-ddd-attribute-policy"),
            "Sample DDD policy from assembly attributes",
            imports: null,
            rules: rules,
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

        return BrickRuleEvaluator.Evaluate(BuildPolicyFromAssemblyAttributes(), dependencies, roles);
    }

    public static IReadOnlyList<string> ExplainConfiguration()
    {
        var configuration = ReadConfigurationFromAttributes();
        var lines = new List<string>
        {
            "DDD Bricks configuration is read only from attributes.",
            "Assembly rules:"
        };

        foreach (var rule in configuration.Rules)
        {
            lines.Add($"- {rule.Id}: {rule.SourceRole} -> {rule.TargetRole} ({rule.Mode})");
        }

        lines.Add("Type roles:");
        foreach (var assignment in configuration.RoleAssignments)
        {
            var roles = assignment.Roles.Count == 0
                ? "<none>"
                : string.Join(", ", assignment.Roles.Select(role => role.Value));
            lines.Add($"- {assignment.TypeName}: {roles}");
        }

        return lines;
    }

    private static BrickRule ToBrickRule(RuleAttribute attribute) =>
        new(
            attribute.RuleId,
            string.IsNullOrWhiteSpace(attribute.Message) ? attribute.Id : attribute.Message,
            attribute.SourceRoleId,
            attribute.TargetRoleId,
            attribute.Mode == RuleMode.RequireDependency ? BrickDecision.Require : BrickDecision.Deny,
            BrickScope.Type,
            attribute.Mode == RuleMode.RequireDependency ? BrickSeverity.Warning : BrickSeverity.Error);

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

internal sealed class DddAttributeOnlyConfiguration
{
    public DddAttributeOnlyConfiguration(
        IEnumerable<RuleAttribute> rules,
        IEnumerable<DddTypeRoleAssignment> roleAssignments)
    {
        Rules = (rules ?? Enumerable.Empty<RuleAttribute>()).ToArray();
        RoleAssignments = (roleAssignments ?? Enumerable.Empty<DddTypeRoleAssignment>()).ToArray();
    }

    public IReadOnlyList<RuleAttribute> Rules { get; }
    public IReadOnlyList<DddTypeRoleAssignment> RoleAssignments { get; }
}

internal sealed class DddTypeRoleAssignment
{
    public DddTypeRoleAssignment(string typeName, IEnumerable<RoleId> roles)
    {
        TypeName = typeName ?? string.Empty;
        Roles = (roles ?? Enumerable.Empty<RoleId>()).ToArray();
    }

    public string TypeName { get; }
    public IReadOnlyList<RoleId> Roles { get; }
}
