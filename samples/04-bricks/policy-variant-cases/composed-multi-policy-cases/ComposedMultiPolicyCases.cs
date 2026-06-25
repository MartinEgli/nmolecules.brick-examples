using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.ComposedMultiPolicy;

/// <summary>
/// Demonstrates several policies in one project and shows how import modes
/// influence the final policy.
/// </summary>
public static class ComposedMultiPolicyCases
{
    public static ComposedPolicyCaseResult Evaluate()
    {
        var platform = PlatformPolicy();
        var product = ProductPolicy(platform.Id);
        var team = TeamPolicy(product.Id);
        var disabled = DisabledExperimentalPolicy();
        var result = BrickPolicyComposer.Compose(team, new[] { platform, product, disabled });

        var appService = Element("type:Catalog.Application.UpdatePrice", BrickElementKind.Type, "UpdatePrice", "Catalog.Application.UpdatePrice");
        var domain = Element("type:Catalog.Domain.Price", BrickElementKind.Type, "Price", "Catalog.Domain.Price");
        var sqlAdapter = Element("type:Catalog.Infrastructure.SqlPriceRepository", BrickElementKind.Type, "SqlPriceRepository", "Catalog.Infrastructure.SqlPriceRepository");
        var applicationNamespace = Element("namespace:Catalog.Application", BrickElementKind.Namespace, "Catalog.Application", "Catalog.Application");
        var infrastructureNamespace = Element("namespace:Catalog.Infrastructure", BrickElementKind.Namespace, "Catalog.Infrastructure", "Catalog.Infrastructure");

        var dependencies = new[]
        {
            Dependency(appService, domain, BrickScope.Type),
            Dependency(appService, sqlAdapter, BrickScope.Type),
            Dependency(applicationNamespace, infrastructureNamespace, BrickScope.Namespace)
        };
        var roles = new[]
        {
            Resolved(appService, ComposedPolicyRoles.ApplicationType),
            Resolved(domain, ComposedPolicyRoles.DomainType),
            Resolved(sqlAdapter, ComposedPolicyRoles.InfrastructureType),
            Resolved(applicationNamespace, ComposedPolicyRoles.ApplicationNamespace),
            Resolved(infrastructureNamespace, ComposedPolicyRoles.InfrastructureNamespace)
        };

        return new ComposedPolicyCaseResult(
            result,
            dependencies,
            roles,
            BrickRuleEvaluator.Evaluate(result.Policy, dependencies, roles));
    }

    public static BrickPolicy PlatformPolicy() =>
        new(
            BrickPolicyId.From(ComposedPolicyIds.PlatformPolicy),
            "Platform baseline policy",
            imports: null,
            rules: new[]
            {
                new BrickRule(
                    RuleId.From(ComposedPolicyIds.ApplicationMustNotUseInfrastructure),
                    "Application type must not use infrastructure type",
                    RoleId.From(ComposedPolicyRoles.ApplicationType),
                    RoleId.From(ComposedPolicyRoles.InfrastructureType),
                    BrickDecision.Deny,
                    BrickScope.Type,
                    BrickSeverity.Warning)
            },
            defaultDecision: BrickPermissionDefault.Allow,
            enforcement: BrickEnforcementMode.Analyze);

    public static BrickPolicy ProductPolicy(BrickPolicyId platformPolicyId) =>
        new(
            BrickPolicyId.From(ComposedPolicyIds.ProductPolicy),
            "Product policy extends platform",
            new[] { new BrickPolicyImport(platformPolicyId, BrickPolicyImportMode.Narrow) },
            new[]
            {
                new BrickRule(
                    RuleId.From(ComposedPolicyIds.ApplicationMustNotUseInfrastructure),
                    "Application type must not use infrastructure type as error",
                    RoleId.From(ComposedPolicyRoles.ApplicationType),
                    RoleId.From(ComposedPolicyRoles.InfrastructureType),
                    BrickDecision.Deny,
                    BrickScope.Type,
                    BrickSeverity.Error),
                new BrickRule(
                    RuleId.From(ComposedPolicyIds.ApplicationNamespaceMustNotUseInfrastructureNamespace),
                    "Application namespace must not use infrastructure namespace",
                    RoleId.From(ComposedPolicyRoles.ApplicationNamespace),
                    RoleId.From(ComposedPolicyRoles.InfrastructureNamespace),
                    BrickDecision.Deny,
                    BrickScope.Namespace,
                    BrickSeverity.Error)
            },
            defaultDecision: BrickPermissionDefault.Allow,
            enforcement: BrickEnforcementMode.Analyze);

    public static BrickPolicy TeamPolicy(BrickPolicyId productPolicyId) =>
        new(
            BrickPolicyId.From(ComposedPolicyIds.TeamPolicy),
            "Team policy imports product and disables experiment",
            new[]
            {
                new BrickPolicyImport(productPolicyId, BrickPolicyImportMode.Import),
                new BrickPolicyImport(BrickPolicyId.From(ComposedPolicyIds.ExperimentalPolicy), BrickPolicyImportMode.Disable)
            },
            new[]
            {
                new BrickRule(
                    RuleId.From(ComposedPolicyIds.ApplicationRequiresDomain),
                    "Application type requires domain type",
                    RoleId.From(ComposedPolicyRoles.ApplicationType),
                    RoleId.From(ComposedPolicyRoles.DomainType),
                    BrickDecision.Require,
                    BrickScope.Type,
                    BrickSeverity.Warning)
            },
            new[]
            {
                new BrickRoleCombinationRule(
                    "Application type cannot be infrastructure type",
                    BrickRoleSelector.From(ComposedPolicyRoles.ApplicationType),
                    BrickRoleSelector.From(ComposedPolicyRoles.InfrastructureType),
                    BrickCombinationKind.Incompatible,
                    "A team role should not mix orchestration and infrastructure implementation.")
            },
            externalAssignments: null,
            aliases: null,
            defaultDecision: BrickPermissionDefault.Allow,
            enforcement: BrickEnforcementMode.Enforce);

    public static BrickPolicy DisabledExperimentalPolicy() =>
        new(
            BrickPolicyId.From(ComposedPolicyIds.ExperimentalPolicy),
            "Disabled experimental policy",
            imports: null,
            rules: new[]
            {
                new BrickRule(
                    RuleId.From(ComposedPolicyIds.ExperimentalRule),
                    "Experimental rule should not appear when disabled",
                    RoleId.From(ComposedPolicyRoles.ApplicationType),
                    RoleId.From(ComposedPolicyRoles.DomainType),
                    BrickDecision.Deny)
            },
            defaultDecision: BrickPermissionDefault.Allow,
            enforcement: BrickEnforcementMode.Analyze);

    private static BrickDependency Dependency(BrickElement source, BrickElement target, BrickScope scope) =>
        new(
            source,
            target,
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            scope,
            BrickDependencyLayer.Static,
            BrickDependencyStrength.Direct,
            BrickEvidenceLevel.CompilerConfirmed);

    private static BrickResolvedRoles Resolved(BrickElement element, string role)
    {
        var assignment = new BrickRoleAssignment(
            new BrickElementSelector(element.Kind, element.Id.Value, element.AssemblyName),
            RoleId.From(role),
            BrickAssignmentMode.ExternalConfiguration,
            BrickAssignmentSource.PolicyFile,
            new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.External),
            BrickAssignmentBehavior.Apply);

        return new BrickResolvedRoles(element, new[] { assignment }, new[] { assignment }, null, null);
    }

    private static BrickElement Element(string id, BrickElementKind kind, string displayName, string fullName) =>
        new(
            BrickElementId.From(id),
            kind,
            displayName,
            "Samples.Block04.Bricks.PolicyVariants.ComposedMultiPolicy",
            NamespaceOf(fullName),
            fullName);

    private static string NamespaceOf(string fullName)
    {
        var lastDot = fullName.LastIndexOf('.');
        return lastDot < 0 ? fullName : fullName.Substring(0, lastDot);
    }
}

public sealed class ComposedPolicyCaseResult
{
    public ComposedPolicyCaseResult(
        BrickPolicyCompositionResult composition,
        IEnumerable<BrickDependency> dependencies,
        IEnumerable<BrickResolvedRoles> roles,
        IEnumerable<BrickViolation> violations)
    {
        Composition = composition;
        Dependencies = dependencies.ToArray();
        Roles = roles.ToArray();
        Violations = violations.ToArray();
    }

    public BrickPolicyCompositionResult Composition { get; }
    public IReadOnlyList<BrickDependency> Dependencies { get; }
    public IReadOnlyList<BrickResolvedRoles> Roles { get; }
    public IReadOnlyList<BrickViolation> Violations { get; }
}

public static class ComposedPolicyIds
{
    public const string PlatformPolicy = "COMPOSED-PLATFORM";
    public const string ProductPolicy = "COMPOSED-PRODUCT";
    public const string TeamPolicy = "COMPOSED-TEAM";
    public const string ExperimentalPolicy = "COMPOSED-EXPERIMENTAL";
    public const string ApplicationMustNotUseInfrastructure = "COMPOSED-001";
    public const string ApplicationNamespaceMustNotUseInfrastructureNamespace = "COMPOSED-002";
    public const string ApplicationRequiresDomain = "COMPOSED-003";
    public const string ExperimentalRule = "COMPOSED-999";
}

public static class ComposedPolicyRoles
{
    public const string ApplicationType = "Composed.Type.Application";
    public const string DomainType = "Composed.Type.Domain";
    public const string InfrastructureType = "Composed.Type.Infrastructure";
    public const string ApplicationNamespace = "Composed.Namespace.Application";
    public const string InfrastructureNamespace = "Composed.Namespace.Infrastructure";
}
