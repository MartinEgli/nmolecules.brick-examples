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
        ruleId == AttributeMultiRuleIds.ApplicationNamespaceMustNotUseInfrastructureNamespace
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

[Policy(
    id: AttributeMultiPolicyIds.PlatformPolicy,
    name: "Attribute multi platform policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]
[Rule(
    id: AttributeMultiRuleIds.ApplicationMustNotUseInfrastructure,
    sourceRole: AttributeMultiRoles.ApplicationType,
    targetRole: AttributeMultiRoles.InfrastructureType,
    mode: RuleMode.ForbidDependency,
    message: "Application types must not use infrastructure types.")]
public sealed class AttributeMultiPlatformPolicyDefinition
{
}

[Policy(
    id: AttributeMultiPolicyIds.ProductPolicy,
    name: "Attribute multi product policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]
[PolicyImport(AttributeMultiPolicyIds.PlatformPolicy, BrickPolicyImportMode.Narrow)]
[Rule(
    id: AttributeMultiRuleIds.ApplicationMustNotUseInfrastructure,
    sourceRole: AttributeMultiRoles.ApplicationType,
    targetRole: AttributeMultiRoles.InfrastructureType,
    mode: RuleMode.ForbidDependency,
    message: "Application types must not use infrastructure types as product errors.")]
[Rule(
    id: AttributeMultiRuleIds.ApplicationNamespaceMustNotUseInfrastructureNamespace,
    sourceRole: AttributeMultiRoles.ApplicationNamespace,
    targetRole: AttributeMultiRoles.InfrastructureNamespace,
    mode: RuleMode.ForbidDependency,
    message: "Application namespace must not use infrastructure namespace.")]
public sealed class AttributeMultiProductPolicyDefinition
{
}

[Policy(
    id: AttributeMultiPolicyIds.TeamPolicy,
    name: "Attribute multi team policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Enforce)]
[PolicyImport(AttributeMultiPolicyIds.ProductPolicy, BrickPolicyImportMode.Import)]
[PolicyImport(AttributeMultiPolicyIds.ExperimentalPolicy, BrickPolicyImportMode.Disable)]
[Rule(
    id: AttributeMultiRuleIds.ApplicationRequiresDomain,
    sourceRole: AttributeMultiRoles.ApplicationType,
    targetRole: AttributeMultiRoles.DomainType,
    mode: RuleMode.RequireDependency,
    message: "Application types require domain types.")]
[Dependency(
    id: AttributeMultiDependencyIds.ApplicationToDomain,
    source: "Catalog.Application.UpdatePrice",
    target: "Catalog.Domain.Price",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]
[Dependency(
    id: AttributeMultiDependencyIds.ApplicationToInfrastructure,
    source: "Catalog.Application.UpdatePrice",
    target: "Catalog.Infrastructure.SqlPriceRepository",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]
[Dependency(
    id: AttributeMultiDependencyIds.ApplicationNamespaceToInfrastructureNamespace,
    source: "Catalog.Application",
    target: "Catalog.Infrastructure",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Namespace,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.AnalyzerInferred)]
public sealed class AttributeMultiTeamPolicyDefinition
{
}

[Policy(
    id: AttributeMultiPolicyIds.ExperimentalPolicy,
    name: "Disabled attribute experimental policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]
[Rule(
    id: AttributeMultiRuleIds.ExperimentalRule,
    sourceRole: AttributeMultiRoles.ApplicationType,
    targetRole: AttributeMultiRoles.DomainType,
    mode: RuleMode.ForbidDependency,
    message: "Disabled experimental rule should not enter composition.")]
public sealed class AttributeMultiExperimentalPolicyDefinition
{
}

public sealed class AttributeMultiPolicyInventory
{
    public AttributeMultiPolicyInventory(IEnumerable<AttributeMultiPolicyDefinition> definitions)
    {
        Definitions = definitions.ToArray();
    }

    public IReadOnlyList<AttributeMultiPolicyDefinition> Definitions { get; }
}

public sealed class AttributeMultiPolicyDefinition
{
    public AttributeMultiPolicyDefinition(
        string ownerTypeName,
        BrickPolicy policy,
        IEnumerable<DependencyAttribute> dependencies)
    {
        OwnerTypeName = ownerTypeName;
        Policy = policy;
        Dependencies = dependencies.ToArray();
    }

    public string OwnerTypeName { get; }
    public BrickPolicy Policy { get; }
    public IReadOnlyList<DependencyAttribute> Dependencies { get; }
}

public sealed class AttributeMultiPolicyResult
{
    public AttributeMultiPolicyResult(
        AttributeMultiPolicyInventory inventory,
        BrickPolicyCompositionResult composition,
        IEnumerable<BrickDependency> dependencies,
        IEnumerable<BrickResolvedRoles> roles,
        IEnumerable<BrickViolation> violations,
        AttributeMultiPolicyCorrelationAssessment correlation)
    {
        Inventory = inventory;
        Composition = composition;
        Dependencies = dependencies.ToArray();
        Roles = roles.ToArray();
        Violations = violations.ToArray();
        Correlation = correlation;
    }

    public AttributeMultiPolicyInventory Inventory { get; }
    public BrickPolicyCompositionResult Composition { get; }
    public IReadOnlyList<BrickDependency> Dependencies { get; }
    public IReadOnlyList<BrickResolvedRoles> Roles { get; }
    public IReadOnlyList<BrickViolation> Violations { get; }
    public AttributeMultiPolicyCorrelationAssessment Correlation { get; }
}

public sealed class AttributeMultiPolicyCorrelationAssessment
{
    public AttributeMultiPolicyCorrelationAssessment(
        bool usesDefinitionTypeAsOwner,
        bool needsPolicyIdOnRuleAttributes,
        bool needsPolicyIdOnDependencyAttributes)
    {
        UsesDefinitionTypeAsOwner = usesDefinitionTypeAsOwner;
        NeedsPolicyIdOnRuleAttributes = needsPolicyIdOnRuleAttributes;
        NeedsPolicyIdOnDependencyAttributes = needsPolicyIdOnDependencyAttributes;
    }

    public bool UsesDefinitionTypeAsOwner { get; }
    public bool NeedsPolicyIdOnRuleAttributes { get; }
    public bool NeedsPolicyIdOnDependencyAttributes { get; }

    public static AttributeMultiPolicyCorrelationAssessment From(AttributeMultiPolicyInventory inventory)
    {
        var everyDefinitionHasPolicy = inventory.Definitions.All(definition => !definition.Policy.Id.IsEmpty);

        return new AttributeMultiPolicyCorrelationAssessment(
            usesDefinitionTypeAsOwner: everyDefinitionHasPolicy,
            needsPolicyIdOnRuleAttributes: false,
            needsPolicyIdOnDependencyAttributes: false);
    }
}

public static class AttributeMultiPolicyIds
{
    public const string PlatformPolicy = "ATTR-MULTI-PLATFORM";
    public const string ProductPolicy = "ATTR-MULTI-PRODUCT";
    public const string TeamPolicy = "ATTR-MULTI-TEAM";
    public const string ExperimentalPolicy = "ATTR-MULTI-EXPERIMENTAL";
}

public static class AttributeMultiRuleIds
{
    public const string ApplicationMustNotUseInfrastructure = "ATTR-MULTI-001";
    public const string ApplicationNamespaceMustNotUseInfrastructureNamespace = "ATTR-MULTI-002";
    public const string ApplicationRequiresDomain = "ATTR-MULTI-003";
    public const string ExperimentalRule = "ATTR-MULTI-999";
}

public static class AttributeMultiDependencyIds
{
    public const string ApplicationToDomain = "ATTR-MULTI-DEP-001";
    public const string ApplicationToInfrastructure = "ATTR-MULTI-DEP-002";
    public const string ApplicationNamespaceToInfrastructureNamespace = "ATTR-MULTI-DEP-003";
}

public static class AttributeMultiRoles
{
    public const string ApplicationType = "AttrMulti.Type.Application";
    public const string DomainType = "AttrMulti.Type.Domain";
    public const string InfrastructureType = "AttrMulti.Type.Infrastructure";
    public const string ApplicationNamespace = "AttrMulti.Namespace.Application";
    public const string InfrastructureNamespace = "AttrMulti.Namespace.Infrastructure";
}
