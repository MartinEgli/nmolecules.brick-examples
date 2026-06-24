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
/// The policy header comes from <see cref="PolicyAttribute"/>, dependency rules
/// come from assembly-level <see cref="RuleAttribute"/> declarations, and sample
/// dependency evidence comes from <see cref="DependencyAttribute"/>. Roles come
/// from custom role marker attributes such as <see cref="DddAggregateRootAttribute"/>
/// and <see cref="DddRepositoryAttribute"/>.
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
        var assembly = typeof(DddAttributeOnlyConfigurationExample).Assembly;
        var assemblyPolicies = assembly
            .GetCustomAttributes<PolicyAttribute>()
            .Where(policy => policy.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .OrderBy(policy => policy.Id, StringComparer.Ordinal)
            .ToArray();
        var assemblyRules = assembly
            .GetCustomAttributes<RuleAttribute>()
            .Where(rule => rule.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .OrderBy(rule => rule.Id, StringComparer.Ordinal)
            .ToArray();
        var assemblyDependencies = assembly
            .GetCustomAttributes<DependencyAttribute>()
            .Where(dependency => dependency.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .OrderBy(dependency => dependency.Id, StringComparer.Ordinal)
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

        return new DddAttributeOnlyConfiguration(assemblyPolicies, assemblyRules, assemblyDependencies, roleAssignments);
    }

    public static BrickPolicy BuildPolicyFromAssemblyAttributes()
    {
        var configuration = ReadConfigurationFromAttributes();
        var policy = configuration.Policies.FirstOrDefault();
        var rules = configuration.Rules
            .Select(ToBrickRule)
            .ToArray();

        return new BrickPolicy(
            policy?.PolicyId ?? BrickPolicyId.From("sample-ddd-attribute-policy"),
            string.IsNullOrWhiteSpace(policy?.Name) ? "Sample DDD policy from assembly attributes" : policy.Name,
            imports: null,
            rules: rules,
            defaultDecision: policy?.DefaultDecision ?? BrickPermissionDefault.Allow,
            enforcement: policy?.Enforcement ?? BrickEnforcementMode.Analyze);
    }

    public static IReadOnlyList<BrickViolation> EvaluateExampleDependency()
    {
        var configuration = ReadConfigurationFromAttributes();
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
        var dependencies = configuration.Dependencies
            .Select(ToBrickDependency)
            .ToArray();

        return BrickRuleEvaluator.Evaluate(BuildPolicyFromAssemblyAttributes(), dependencies, roles);
    }

    public static IReadOnlyList<string> ExplainConfiguration()
    {
        var configuration = ReadConfigurationFromAttributes();
        var lines = new List<string>
        {
            "DDD Bricks configuration is read only from attributes.",
            "Assembly policies:"
        };

        foreach (var policy in configuration.Policies)
        {
            lines.Add($"- {policy.Id}: {policy.Name} ({policy.DefaultDecision}, {policy.Enforcement})");
        }

        lines.Add("Assembly rules:");
        foreach (var rule in configuration.Rules)
        {
            lines.Add($"- {rule.Id}: {rule.SourceRole} -> {rule.TargetRole} ({rule.Mode})");
        }

        lines.Add("Assembly dependencies:");
        foreach (var dependency in configuration.Dependencies)
        {
            lines.Add($"- {dependency.Id}: {dependency.Source} -> {dependency.Target} ({dependency.Kind})");
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

    private static BrickDependency ToBrickDependency(DependencyAttribute attribute) =>
        new(
            Type(attribute.Source),
            Type(attribute.Target),
            attribute.KindId,
            attribute.Scope,
            attribute.Layer,
            attribute.Strength,
            attribute.EvidenceLevel);

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

}

internal sealed class DddAttributeOnlyConfiguration
{
    public DddAttributeOnlyConfiguration(
        IEnumerable<PolicyAttribute> policies,
        IEnumerable<RuleAttribute> rules,
        IEnumerable<DependencyAttribute> dependencies,
        IEnumerable<DddTypeRoleAssignment> roleAssignments)
    {
        Policies = (policies ?? Enumerable.Empty<PolicyAttribute>()).ToArray();
        Rules = (rules ?? Enumerable.Empty<RuleAttribute>()).ToArray();
        Dependencies = (dependencies ?? Enumerable.Empty<DependencyAttribute>()).ToArray();
        RoleAssignments = (roleAssignments ?? Enumerable.Empty<DddTypeRoleAssignment>()).ToArray();
    }

    public IReadOnlyList<PolicyAttribute> Policies { get; }
    public IReadOnlyList<RuleAttribute> Rules { get; }
    public IReadOnlyList<DependencyAttribute> Dependencies { get; }
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
