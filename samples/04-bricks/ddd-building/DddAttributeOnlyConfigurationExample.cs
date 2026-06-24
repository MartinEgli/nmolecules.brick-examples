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
/// <c>DddBrickRules.Assembly.cs</c>. No <see cref="BrickPolicy"/> object is built here.
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
