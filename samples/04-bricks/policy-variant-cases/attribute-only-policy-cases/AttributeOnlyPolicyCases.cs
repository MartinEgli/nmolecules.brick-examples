using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

[assembly: Policy(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributePolicyIds.OrdersPolicy,
    name: "Orders attribute policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]

[assembly: Policy(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributePolicyIds.PaymentsPolicy,
    name: "Payments attribute policy",
    defaultDecision: BrickPermissionDefault.Deny,
    enforcement: BrickEnforcementMode.Analyze)]

[assembly: Rule(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRuleIds.OrdersDomainMustNotUseInfrastructure,
    sourceRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.DomainAggregate,
    targetRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.InfrastructureAdapter,
    mode: RuleMode.ForbidDependency,
    message: "Orders aggregate must not use infrastructure directly.")]

[assembly: Rule(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRuleIds.OrdersHandlerRequiresRepositoryContract,
    sourceRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.ApplicationHandler,
    targetRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.RepositoryContract,
    mode: RuleMode.RequireDependency,
    message: "Orders handler must use a repository contract.")]

[assembly: Rule(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRuleIds.PaymentsNamespaceMustNotUseExternalGateway,
    sourceRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.PaymentNamespace,
    targetRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.ExternalPaymentGateway,
    mode: RuleMode.ForbidDependency,
    message: "Payment namespace must not depend on the external gateway package.")]

[assembly: Dependency(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeDependencyIds.OrderAggregateToSqlAdapter,
    source: nameof(Samples.Block04.Bricks.PolicyVariants.AttributeOnly.OrderAggregate),
    target: nameof(Samples.Block04.Bricks.PolicyVariants.AttributeOnly.SqlOrderAdapter),
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]

[assembly: Dependency(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeDependencyIds.OrderHandlerToRepositoryContract,
    source: nameof(Samples.Block04.Bricks.PolicyVariants.AttributeOnly.SubmitOrderHandler),
    target: nameof(Samples.Block04.Bricks.PolicyVariants.AttributeOnly.IOrderRepository),
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]

[assembly: Dependency(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeDependencyIds.PaymentNamespaceToGateway,
    source: "Samples.Payments.Application",
    target: "Stripe",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Namespace,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.AnalyzerInferred)]

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;

/// <summary>
/// Demonstrates attribute-only policy definitions, including the multi-policy
/// correlation question.
/// </summary>
public static class AttributeOnlyPolicyCases
{
    private static readonly Type[] Types =
    {
        typeof(OrderAggregate),
        typeof(SubmitOrderHandler),
        typeof(IOrderRepository),
        typeof(OrderId),
        typeof(SqlOrderAdapter)
    };

    /// <summary>
    /// Reads all policy, rule and dependency facts from attributes.
    /// </summary>
    public static AttributeOnlyPolicyInventory ReadInventory()
    {
        var assembly = typeof(AttributeOnlyPolicyCases).Assembly;
        var policies = assembly.GetCustomAttributes<PolicyAttribute>()
            .Where(policy => policy.Id.StartsWith("ATTR-", StringComparison.Ordinal))
            .OrderBy(policy => policy.Id, StringComparer.Ordinal)
            .ToArray();
        var rules = assembly.GetCustomAttributes<RuleAttribute>()
            .Where(rule => rule.Id.StartsWith("ATTR-", StringComparison.Ordinal))
            .OrderBy(rule => rule.Id, StringComparer.Ordinal)
            .ToArray();
        var dependencies = assembly.GetCustomAttributes<DependencyAttribute>()
            .Where(dependency => dependency.Id.StartsWith("ATTR-", StringComparison.Ordinal))
            .OrderBy(dependency => dependency.Id, StringComparer.Ordinal)
            .ToArray();
        var roleAssignments = Types.SelectMany(type =>
            type.GetCustomAttributes<RoleAttribute>(inherit: false)
                .Select(role => new AttributeRoleFact(type.Name, role.Id)))
            .OrderBy(fact => fact.TypeName, StringComparer.Ordinal)
            .ToArray();

        return new AttributeOnlyPolicyInventory(policies, rules, dependencies, roleAssignments);
    }

    /// <summary>
    /// Evaluates the orders policy by conventionally grouping `ATTR-ORDERS-*`
    /// rule and dependency IDs.
    /// </summary>
    public static AttributeOnlyPolicyResult EvaluateOrdersPolicy()
    {
        var inventory = ReadInventory();
        var policy = BuildPolicy(
            inventory,
            AttributePolicyIds.OrdersPolicy,
            AttributeRuleIds.OrdersPrefix);
        var dependencies = inventory.Dependencies
            .Where(dependency => dependency.Id.StartsWith(AttributeDependencyIds.OrdersPrefix, StringComparison.Ordinal))
            .Select(ToDependency)
            .ToArray();
        var roles = BuildResolvedRoles();

        return new AttributeOnlyPolicyResult(
            policy,
            dependencies,
            roles,
            BrickRuleEvaluator.Evaluate(policy, dependencies, roles),
            AnalyzeCorrelationNeed(inventory));
    }

    /// <summary>
    /// Evaluates the payments policy with a namespace source and external target.
    /// </summary>
    public static AttributeOnlyPolicyResult EvaluatePaymentsPolicy()
    {
        var inventory = ReadInventory();
        var policy = BuildPolicy(
            inventory,
            AttributePolicyIds.PaymentsPolicy,
            AttributeRuleIds.PaymentsPrefix);
        var dependencies = inventory.Dependencies
            .Where(dependency => dependency.Id.StartsWith(AttributeDependencyIds.PaymentsPrefix, StringComparison.Ordinal))
            .Select(ToDependency)
            .ToArray();
        var roles = BuildResolvedRoles();

        return new AttributeOnlyPolicyResult(
            policy,
            dependencies,
            roles,
            BrickRuleEvaluator.Evaluate(policy, dependencies, roles),
            AnalyzeCorrelationNeed(inventory));
    }

    public static AttributePolicyCorrelationAssessment AnalyzeCorrelationNeed(AttributeOnlyPolicyInventory inventory)
    {
        var hasSeveralPolicies = inventory.Policies.Count > 1;
        var rulesWithNativePolicyId = 0;
        var dependenciesWithNativePolicyId = 0;
        var canUseConvention = inventory.Rules.All(rule =>
                rule.Id.StartsWith(AttributeRuleIds.OrdersPrefix, StringComparison.Ordinal) ||
                rule.Id.StartsWith(AttributeRuleIds.PaymentsPrefix, StringComparison.Ordinal)) &&
            inventory.Dependencies.All(dependency =>
                dependency.Id.StartsWith(AttributeDependencyIds.OrdersPrefix, StringComparison.Ordinal) ||
                dependency.Id.StartsWith(AttributeDependencyIds.PaymentsPrefix, StringComparison.Ordinal));

        return new AttributePolicyCorrelationAssessment(
            hasSeveralPolicies,
            rulesWithNativePolicyId,
            dependenciesWithNativePolicyId,
            canUseConvention,
            hasSeveralPolicies && rulesWithNativePolicyId == 0 && dependenciesWithNativePolicyId == 0);
    }

    private static BrickPolicy BuildPolicy(
        AttributeOnlyPolicyInventory inventory,
        string policyId,
        string rulePrefix)
    {
        var header = inventory.Policies.Single(policy => policy.Id == policyId);
        var rules = inventory.Rules
            .Where(rule => rule.Id.StartsWith(rulePrefix, StringComparison.Ordinal))
            .Select(ToRule)
            .ToArray();

        return new BrickPolicy(
            header.PolicyId,
            header.Name,
            imports: null,
            rules,
            header.DefaultDecision,
            header.Enforcement);
    }

    private static BrickRule ToRule(RuleAttribute attribute) =>
        new(
            attribute.RuleId,
            string.IsNullOrWhiteSpace(attribute.Message) ? attribute.Id : attribute.Message,
            attribute.SourceRoleId,
            attribute.TargetRoleId,
            attribute.Mode == RuleMode.RequireDependency ? BrickDecision.Require : BrickDecision.Deny,
            attribute.Id.StartsWith(AttributeRuleIds.PaymentsPrefix, StringComparison.Ordinal)
                ? BrickScope.Namespace
                : BrickScope.Type,
            attribute.Mode == RuleMode.RequireDependency ? BrickSeverity.Warning : BrickSeverity.Error);

    private static BrickDependency ToDependency(DependencyAttribute attribute) =>
        new(
            Element(attribute.Source, attribute.Scope == BrickScope.Namespace ? BrickElementKind.Namespace : BrickElementKind.Type),
            Element(attribute.Target, TargetKind(attribute)),
            attribute.KindId,
            attribute.Scope,
            attribute.Layer,
            attribute.Strength,
            attribute.EvidenceLevel);

    private static BrickElementKind TargetKind(DependencyAttribute attribute)
    {
        if (attribute.Target == "Stripe")
        {
            return BrickElementKind.ExternalReference;
        }

        return BrickElementKind.Type;
    }

    private static IReadOnlyList<BrickResolvedRoles> BuildResolvedRoles()
    {
        var reflected = Types.SelectMany(type =>
        {
            var element = Element(type.Name, BrickElementKind.Type);
            return type.GetCustomAttributes<RoleAttribute>(inherit: false)
                .Select(role => Resolved(element, role.Id.Value));
        });
        var modeled = new[]
        {
            Resolved(Element("Samples.Payments.Application", BrickElementKind.Namespace), AttributeRoles.PaymentNamespace),
            Resolved(Element("Stripe", BrickElementKind.ExternalReference), AttributeRoles.ExternalPaymentGateway)
        };

        return reflected.Concat(modeled).ToArray();
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

    private static BrickElement Element(string name, BrickElementKind kind) =>
        new(
            BrickElementId.From(kind.ToString().ToLowerInvariant() + ":" + name),
            kind,
            name,
            "Samples.Block04.Bricks.PolicyVariants.AttributeOnly",
            NamespaceFor(kind, name),
            FullNameFor(kind, name));

    private static string NamespaceFor(BrickElementKind kind, string name) =>
        kind == BrickElementKind.Namespace ? name : "Samples.Block04.Bricks.PolicyVariants.AttributeOnly";

    private static string FullNameFor(BrickElementKind kind, string name) =>
        kind == BrickElementKind.Namespace || kind == BrickElementKind.ExternalReference
            ? name
            : "Samples.Block04.Bricks.PolicyVariants.AttributeOnly." + name;
}

public sealed class AttributeOnlyPolicyInventory
{
    public AttributeOnlyPolicyInventory(
        IEnumerable<PolicyAttribute> policies,
        IEnumerable<RuleAttribute> rules,
        IEnumerable<DependencyAttribute> dependencies,
        IEnumerable<AttributeRoleFact> roleAssignments)
    {
        Policies = policies.ToArray();
        Rules = rules.ToArray();
        Dependencies = dependencies.ToArray();
        RoleAssignments = roleAssignments.ToArray();
    }

    public IReadOnlyList<PolicyAttribute> Policies { get; }
    public IReadOnlyList<RuleAttribute> Rules { get; }
    public IReadOnlyList<DependencyAttribute> Dependencies { get; }
    public IReadOnlyList<AttributeRoleFact> RoleAssignments { get; }
}

public sealed class AttributeOnlyPolicyResult
{
    public AttributeOnlyPolicyResult(
        BrickPolicy policy,
        IEnumerable<BrickDependency> dependencies,
        IEnumerable<BrickResolvedRoles> roles,
        IEnumerable<BrickViolation> violations,
        AttributePolicyCorrelationAssessment correlation)
    {
        Policy = policy;
        Dependencies = dependencies.ToArray();
        Roles = roles.ToArray();
        Violations = violations.ToArray();
        Correlation = correlation;
    }

    public BrickPolicy Policy { get; }
    public IReadOnlyList<BrickDependency> Dependencies { get; }
    public IReadOnlyList<BrickResolvedRoles> Roles { get; }
    public IReadOnlyList<BrickViolation> Violations { get; }
    public AttributePolicyCorrelationAssessment Correlation { get; }
}

public sealed class AttributePolicyCorrelationAssessment
{
    public AttributePolicyCorrelationAssessment(
        bool hasSeveralPoliciesInAssembly,
        int rulesWithNativePolicyId,
        int dependenciesWithNativePolicyId,
        bool canUseIdConvention,
        bool wouldBenefitFromExplicitPolicyCorrelation)
    {
        HasSeveralPoliciesInAssembly = hasSeveralPoliciesInAssembly;
        RulesWithNativePolicyId = rulesWithNativePolicyId;
        DependenciesWithNativePolicyId = dependenciesWithNativePolicyId;
        CanUseIdConvention = canUseIdConvention;
        WouldBenefitFromExplicitPolicyCorrelation = wouldBenefitFromExplicitPolicyCorrelation;
    }

    public bool HasSeveralPoliciesInAssembly { get; }
    public int RulesWithNativePolicyId { get; }
    public int DependenciesWithNativePolicyId { get; }
    public bool CanUseIdConvention { get; }
    public bool WouldBenefitFromExplicitPolicyCorrelation { get; }
}

public readonly struct AttributeRoleFact
{
    public AttributeRoleFact(string typeName, RoleId role)
    {
        TypeName = typeName;
        Role = role;
    }

    public string TypeName { get; }
    public RoleId Role { get; }
}

public static class AttributePolicyIds
{
    public const string OrdersPolicy = "ATTR-ORDERS-POLICY";
    public const string PaymentsPolicy = "ATTR-PAYMENTS-POLICY";
}

public static class AttributeRuleIds
{
    public const string OrdersPrefix = "ATTR-ORDERS-";
    public const string PaymentsPrefix = "ATTR-PAYMENTS-";
    public const string OrdersDomainMustNotUseInfrastructure = "ATTR-ORDERS-001";
    public const string OrdersHandlerRequiresRepositoryContract = "ATTR-ORDERS-002";
    public const string PaymentsNamespaceMustNotUseExternalGateway = "ATTR-PAYMENTS-001";
}

public static class AttributeDependencyIds
{
    public const string OrdersPrefix = "ATTR-ORDERS-DEP-";
    public const string PaymentsPrefix = "ATTR-PAYMENTS-DEP-";
    public const string OrderAggregateToSqlAdapter = "ATTR-ORDERS-DEP-001";
    public const string OrderHandlerToRepositoryContract = "ATTR-ORDERS-DEP-002";
    public const string PaymentNamespaceToGateway = "ATTR-PAYMENTS-DEP-001";
}

public static class AttributeRoles
{
    public const string DomainAggregate = "Attr.Domain.Aggregate";
    public const string ApplicationHandler = "Attr.Application.Handler";
    public const string RepositoryContract = "Attr.Application.RepositoryContract";
    public const string ValueObject = "Attr.Domain.ValueObject";
    public const string InfrastructureAdapter = "Attr.Infrastructure.Adapter";
    public const string PaymentNamespace = "Attr.Namespace.PaymentApplication";
    public const string ExternalPaymentGateway = "Attr.External.PaymentGateway";
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(AttributeRoles.DomainAggregate)]
public sealed class AttributeDomainAggregateAttribute : RoleAttribute
{
    public AttributeDomainAggregateAttribute() : base(AttributeRoles.DomainAggregate)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(AttributeRoles.ApplicationHandler)]
public sealed class AttributeApplicationHandlerAttribute : RoleAttribute
{
    public AttributeApplicationHandlerAttribute() : base(AttributeRoles.ApplicationHandler)
    {
    }
}

[AttributeUsage(AttributeTargets.Interface)]
[RoleAlias(AttributeRoles.RepositoryContract)]
public sealed class AttributeRepositoryContractAttribute : RoleAttribute
{
    public AttributeRepositoryContractAttribute() : base(AttributeRoles.RepositoryContract)
    {
    }
}

[AttributeUsage(AttributeTargets.Struct)]
[RoleAlias(AttributeRoles.ValueObject)]
public sealed class AttributeValueObjectAttribute : RoleAttribute
{
    public AttributeValueObjectAttribute() : base(AttributeRoles.ValueObject)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(AttributeRoles.InfrastructureAdapter)]
public sealed class AttributeInfrastructureAdapterAttribute : RoleAttribute
{
    public AttributeInfrastructureAdapterAttribute() : base(AttributeRoles.InfrastructureAdapter)
    {
    }
}

[AttributeDomainAggregate]
public sealed class OrderAggregate
{
}

[AttributeApplicationHandler]
public sealed class SubmitOrderHandler
{
}

[AttributeRepositoryContract]
public interface IOrderRepository
{
}

[AttributeValueObject]
public readonly struct OrderId
{
}

[AttributeInfrastructureAdapter]
public sealed class SqlOrderAdapter
{
}
