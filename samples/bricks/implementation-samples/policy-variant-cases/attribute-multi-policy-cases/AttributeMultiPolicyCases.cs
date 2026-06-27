using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


/// <summary>
/// Demonstrates several policy definitions in one project where each policy is
/// defined by attributes on a dedicated policy definition class.
/// </summary>
public static class AttributeMultiPolicyCases
{
    private static readonly Type[] PolicyDefinitionTypes =
    {
        typeof(AttributeMultiPlatformPolicyDefinition),
        typeof(AttributeMultiProductPolicyDefinition),
        typeof(AttributeMultiTeamPolicyDefinition),
        typeof(AttributeMultiExperimentalPolicyDefinition)
    };

    /// <summary>
    /// Reads class-scoped policy, import, rule and dependency attributes and
    /// keeps their owner type as the correlation boundary.
    /// </summary>
    public static AttributeMultiPolicyInventory ReadInventory()
    {
        var definitions = PolicyDefinitionTypes
            .Select(ReadDefinition)
            .OrderBy(definition => definition.Policy.Id.Value, StringComparer.Ordinal)
            .ToArray();

        return new AttributeMultiPolicyInventory(definitions);
    }

    /// <summary>
    /// Composes the team policy from class-scoped attribute policies and
    /// evaluates the dependencies attached to the team policy definition.
    /// </summary>
    public static AttributeMultiPolicyResult EvaluateTeamPolicy()
    {
        var inventory = ReadInventory();
        var team = inventory.Definitions.Single(definition => definition.Policy.Id == BrickPolicyId.From(AttributeMultiPolicyIds.TeamPolicy));
        var catalog = inventory.Definitions
            .Where(definition => definition.Policy.Id != team.Policy.Id)
            .Select(definition => definition.Policy)
            .ToArray();
        var composition = BrickPolicyComposer.Compose(team.Policy, catalog);
        var dependencies = team.Dependencies.Select(ToDependency).ToArray();
        var roles = BuildResolvedRoles();

        return new AttributeMultiPolicyResult(
            inventory,
            composition,
            dependencies,
            roles,
            BrickRuleEvaluator.Evaluate(composition.Policy, dependencies, roles),
            AttributeMultiPolicyCorrelationAssessment.From(inventory));
    }

    /// <summary>
    /// Reads the assembly-scoped multiple policy attributes. This exists to show
    /// the trade-off of several policies in one assembly file: rules and
    /// dependencies need a convention because their attributes do not carry a
    /// native owning policy identifier.
    /// </summary>
    public static AttributeMultiAssemblyPolicyInventory ReadAssemblyInventory()
    {
        var assembly = typeof(AttributeMultiPolicyCases).Assembly;
        var policies = assembly.GetCustomAttributes<PolicyAttribute>()
            .Where(policy => policy.Id.StartsWith(AttributeMultiPolicyIds.AssemblyPrefix, StringComparison.Ordinal))
            .OrderBy(policy => policy.Id, StringComparer.Ordinal)
            .ToArray();
        var rules = assembly.GetCustomAttributes<RuleAttribute>()
            .Where(rule => rule.Id.StartsWith(AttributeMultiRuleIds.AssemblyPrefix, StringComparison.Ordinal))
            .OrderBy(rule => rule.Id, StringComparer.Ordinal)
            .ToArray();
        var dependencies = assembly.GetCustomAttributes<DependencyAttribute>()
            .Where(dependency => dependency.Id.StartsWith(AttributeMultiDependencyIds.AssemblyPrefix, StringComparison.Ordinal))
            .OrderBy(dependency => dependency.Id, StringComparer.Ordinal)
            .ToArray();

        return new AttributeMultiAssemblyPolicyInventory(policies, rules, dependencies);
    }

    /// <summary>
    /// Evaluates the assembly-scoped multiple policy sample by applying the
    /// documented ID-prefix convention.
    /// </summary>
    public static AttributeMultiAssemblyPolicyResult EvaluateAssemblyTeamPolicy()
    {
        var inventory = ReadAssemblyInventory();
        var header = inventory.Policies.Single(policy => policy.Id == AttributeMultiPolicyIds.AssemblyTeamPolicy);
        var policy = new BrickPolicy(
            header.PolicyId,
            header.Name,
            imports: null,
            inventory.Rules.Select(ToRule).ToArray(),
            header.DefaultDecision,
            header.Enforcement);
        var dependencies = inventory.Dependencies.Select(ToDependency).ToArray();
        var roles = BuildResolvedRoles();

        return new AttributeMultiAssemblyPolicyResult(
            inventory,
            policy,
            dependencies,
            roles,
            BrickRuleEvaluator.Evaluate(policy, dependencies, roles),
            AttributeMultiAssemblyPolicyCorrelationAssessment.From(inventory));
    }

    private static AttributeMultiPolicyDefinition ReadDefinition(Type type)
    {
        var policy = type.GetCustomAttributes<PolicyAttribute>(inherit: false).Single();
        var imports = type.GetCustomAttributes<PolicyImportAttribute>(inherit: false)
            .Select(ToPolicyImport)
            .ToArray();
        var rules = type.GetCustomAttributes<RuleAttribute>(inherit: false)
            .Select(ToRule)
            .ToArray();
        var dependencies = type.GetCustomAttributes<DependencyAttribute>(inherit: false)
            .OrderBy(dependency => dependency.Id, StringComparer.Ordinal)
            .ToArray();

        return new AttributeMultiPolicyDefinition(
            type.Name,
            new BrickPolicy(
                policy.PolicyId,
                policy.Name,
                imports,
                rules,
                defaultDecision: policy.DefaultDecision,
                enforcement: policy.Enforcement),
            dependencies);
    }

    private static BrickPolicyImport ToPolicyImport(PolicyImportAttribute attribute) =>
        new(attribute.PolicyId, attribute.Mode);

    private static BrickRule ToRule(RuleAttribute attribute) =>
        new(
            attribute.RuleId,
            string.IsNullOrWhiteSpace(attribute.Message) ? attribute.Id : attribute.Message,
            attribute.SourceRoleId,
            attribute.TargetRoleId,
            attribute.Mode == RuleMode.RequireDependency ? BrickDecision.Require : BrickDecision.Deny,
            ScopeFor(attribute.Id),
            attribute.Id == AttributeMultiRuleIds.ApplicationMustNotUseInfrastructure ? BrickSeverity.Error : BrickSeverity.Warning);

    private static BrickScope ScopeFor(string ruleId) =>
        ruleId == AttributeMultiRuleIds.ApplicationNamespaceMustNotUseInfrastructureNamespace ||
        ruleId == AttributeMultiRuleIds.AssemblyProductApplicationNamespaceMustNotUseInfrastructureNamespace
            ? BrickScope.Namespace
            : BrickScope.Type;

    private static BrickDependency ToDependency(DependencyAttribute attribute) =>
        new(
            Element(attribute.Source, SourceKind(attribute.Scope)),
            Element(attribute.Target, TargetKind(attribute)),
            attribute.KindId,
            attribute.Scope,
            attribute.Layer,
            attribute.Strength,
            attribute.EvidenceLevel);

    private static BrickElementKind SourceKind(BrickScope scope) =>
        scope == BrickScope.Namespace ? BrickElementKind.Namespace : BrickElementKind.Type;

    private static BrickElementKind TargetKind(DependencyAttribute attribute)
    {
        if (attribute.Target.EndsWith(".Infrastructure", StringComparison.Ordinal))
        {
            return BrickElementKind.Namespace;
        }

        return BrickElementKind.Type;
    }

    private static IReadOnlyList<BrickResolvedRoles> BuildResolvedRoles()
    {
        var elements = new[]
        {
            Resolved(Element("Catalog.Application.UpdatePrice", BrickElementKind.Type), AttributeMultiRoles.ApplicationType),
            Resolved(Element("Catalog.Domain.Price", BrickElementKind.Type), AttributeMultiRoles.DomainType),
            Resolved(Element("Catalog.Infrastructure.SqlPriceRepository", BrickElementKind.Type), AttributeMultiRoles.InfrastructureType),
            Resolved(Element("Catalog.Application", BrickElementKind.Namespace), AttributeMultiRoles.ApplicationNamespace),
            Resolved(Element("Catalog.Infrastructure", BrickElementKind.Namespace), AttributeMultiRoles.InfrastructureNamespace)
        };

        return elements;
    }

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

    private static BrickElement Element(string fullName, BrickElementKind kind) =>
        new(
            BrickElementId.From(kind.ToString().ToLowerInvariant() + ":" + fullName),
            kind,
            DisplayName(fullName),
            "Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy",
            NamespaceOf(fullName, kind),
            fullName);

    private static string DisplayName(string fullName)
    {
        var lastDot = fullName.LastIndexOf('.');
        return lastDot < 0 ? fullName : fullName.Substring(lastDot + 1);
    }

    private static string NamespaceOf(string fullName, BrickElementKind kind)
    {
        if (kind == BrickElementKind.Namespace)
        {
            return fullName;
        }

        var lastDot = fullName.LastIndexOf('.');
        return lastDot < 0 ? string.Empty : fullName.Substring(0, lastDot);
    }
}
