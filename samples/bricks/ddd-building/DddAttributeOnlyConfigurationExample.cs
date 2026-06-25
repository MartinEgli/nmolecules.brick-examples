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
/// The policy header comes from <see cref="PolicyAttribute"/>, policy imports
/// come from <see cref="PolicyImportAttribute"/>, dependency rules come from
/// assembly-level <see cref="RuleAttribute"/> declarations, role combinations
/// come from <see cref="RoleCombinationAttribute"/>, and sample dependency
/// evidence comes from <see cref="DependencyAttribute"/>. Roles come from custom
/// role marker attributes such as <see cref="DddAggregateRootAttribute"/> and
/// <see cref="DddRepositoryAttribute"/>.
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
        var assemblyImports = assembly
            .GetCustomAttributes<PolicyImportAttribute>()
            .Where(import => import.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .OrderBy(import => import.Id, StringComparer.Ordinal)
            .ToArray();
        var assemblyRules = assembly
            .GetCustomAttributes<RuleAttribute>()
            .Where(rule => rule.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .OrderBy(rule => rule.Id, StringComparer.Ordinal)
            .ToArray();
        var assemblyRoleCombinations = assembly
            .GetCustomAttributes<RoleCombinationAttribute>()
            .Where(combination => !string.IsNullOrWhiteSpace(combination.Name))
            .OrderBy(combination => combination.Name, StringComparer.Ordinal)
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

        return new DddAttributeOnlyConfiguration(
            assemblyPolicies,
            assemblyImports,
            assemblyRules,
            assemblyRoleCombinations,
            assemblyDependencies,
            roleAssignments);
    }

    public static BrickPolicy BuildPolicyFromAssemblyAttributes()
    {
        var configuration = ReadConfigurationFromAttributes();
        var policy = configuration.Policies.FirstOrDefault();
        var imports = configuration.PolicyImports
            .Select(ToPolicyImport)
            .ToArray();
        var rules = configuration.Rules
            .Select(ToBrickRule)
            .ToArray();
        var combinations = configuration.RoleCombinations
            .Select(ToRoleCombinationRule)
            .ToArray();

        return new BrickPolicy(
            policy?.PolicyId ?? BrickPolicyId.From("sample-ddd-attribute-policy"),
            string.IsNullOrWhiteSpace(policy?.Name) ? "Sample DDD policy from assembly attributes" : policy.Name,
            imports,
            rules: rules,
            combinationRules: combinations,
            externalAssignments: null,
            aliases: null,
            defaultDecision: policy?.DefaultDecision ?? BrickPermissionDefault.Allow,
            enforcement: policy?.Enforcement ?? BrickEnforcementMode.Analyze);
    }

    public static IReadOnlyList<BrickViolation> EvaluateExampleDependency()
    {
        var configuration = ReadConfigurationFromAttributes();
        var aggregate = Type(nameof(Contract));
        var repository = Type(nameof(IContractRepository));
        var infrastructureRepository = Type(nameof(InMemoryContractRepository));
        var applicationService = Type(nameof(ContractApplicationService));

        var roles = new[]
        {
            Resolved(aggregate, DddBrickRoles.AggregateRoot),
            Resolved(repository, DddBrickRoles.Repository),
            Resolved(infrastructureRepository, DddBrickRoles.Infrastructure),
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

        lines.Add("Assembly policy imports:");
        foreach (var import in configuration.PolicyImports)
        {
            lines.Add($"- {import.Id}: {import.Mode}");
        }

        lines.Add("Assembly rules:");
        foreach (var rule in configuration.Rules)
        {
            lines.Add($"- {rule.Id}: {rule.SourceRole} -> {rule.TargetRole} ({rule.Mode})");
        }

        lines.Add("Assembly role combinations:");
        foreach (var combination in configuration.RoleCombinations)
        {
            lines.Add($"- {combination.Name}: {combination.LeftRoles} + {combination.RightRoles} ({combination.Kind})");
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

    private static BrickPolicyImport ToPolicyImport(PolicyImportAttribute attribute) =>
        new(attribute.PolicyId, attribute.Mode);

    private static BrickRule ToBrickRule(RuleAttribute attribute) =>
        new(
            attribute.RuleId,
            string.IsNullOrWhiteSpace(attribute.Message) ? attribute.Id : attribute.Message,
            attribute.SourceRoleId,
            attribute.TargetRoleId,
            attribute.Mode == RuleMode.RequireDependency ? BrickDecision.Require : BrickDecision.Deny,
            BrickScope.Type,
            attribute.Mode == RuleMode.RequireDependency ? BrickSeverity.Warning : BrickSeverity.Error);

    private static BrickRoleCombinationRule ToRoleCombinationRule(RoleCombinationAttribute attribute) =>
        new(
            attribute.Name,
            attribute.LeftRoleSelector,
            attribute.RightRoleSelector,
            attribute.Kind,
            attribute.Reason);

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
