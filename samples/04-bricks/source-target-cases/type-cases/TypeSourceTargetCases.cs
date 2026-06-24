using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.TypeCases;

/// <summary>
/// Demonstrates class, interface and struct role markers as type-level sources
/// and targets.
/// </summary>
public static class TypeSourceTargetCases
{
    private static readonly Type[] SampleTypes =
    {
        typeof(OrderEndpoint),
        typeof(SubmitOrderWorkflow),
        typeof(IOrderRepository),
        typeof(Money),
        typeof(SqlOrderRepository)
    };

    /// <summary>
    /// Builds a policy for common type-level source and target combinations.
    /// </summary>
    public static BrickPolicy BuildPolicy() =>
        new(
            BrickPolicyId.From(TypeCaseRules.PolicyId),
            "Type source and target cases",
            imports: null,
            rules: new[]
            {
                Rule(
                    TypeCaseRules.EndpointRequiresApplication,
                    "Endpoint must call an application service",
                    TypeCaseRoles.Endpoint,
                    TypeCaseRoles.ApplicationService,
                    BrickDecision.Require),
                Rule(
                    TypeCaseRules.ApplicationRequiresRepositoryContract,
                    "Application service must use a repository contract",
                    TypeCaseRoles.ApplicationService,
                    TypeCaseRoles.RepositoryContract,
                    BrickDecision.Require),
                Rule(
                    TypeCaseRules.ValueObjectMustNotUseInfrastructure,
                    "Value object must not depend on infrastructure",
                    TypeCaseRoles.ValueObject,
                    TypeCaseRoles.InfrastructureAdapter,
                    BrickDecision.Deny)
            },
            defaultDecision: BrickPermissionDefault.Allow,
            enforcement: BrickEnforcementMode.Analyze);

    /// <summary>
    /// Creates one satisfied class-to-class requirement, one satisfied
    /// class-to-interface requirement and one struct-to-class violation.
    /// </summary>
    public static TypeCaseResult Evaluate()
    {
        var elements = SampleTypes.ToDictionary(type => type.Name, Element);
        var dependencies = new[]
        {
            Dependency(elements[nameof(OrderEndpoint)], elements[nameof(SubmitOrderWorkflow)], BrickScope.Type),
            Dependency(elements[nameof(SubmitOrderWorkflow)], elements[nameof(IOrderRepository)], BrickScope.Type),
            Dependency(elements[nameof(Money)], elements[nameof(SqlOrderRepository)], BrickScope.Type)
        };
        var roles = ResolveRolesFromAttributes(SampleTypes);
        var violations = BrickRuleEvaluator.Evaluate(BuildPolicy(), dependencies, roles);

        return new TypeCaseResult(elements.Values, dependencies, roles, violations);
    }

    private static BrickRule Rule(
        string id,
        string name,
        string sourceRole,
        string targetRole,
        BrickDecision decision) =>
        new(
            RuleId.From(id),
            name,
            RoleId.From(sourceRole),
            RoleId.From(targetRole),
            decision,
            BrickScope.Type,
            decision == BrickDecision.Require ? BrickSeverity.Warning : BrickSeverity.Error);

    private static BrickDependency Dependency(BrickElement source, BrickElement target, BrickScope scope) =>
        new(
            source,
            target,
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            scope,
            BrickDependencyLayer.Static,
            BrickDependencyStrength.Direct,
            BrickEvidenceLevel.CompilerConfirmed);

    private static BrickElement Element(Type type) =>
        new(
            BrickElementId.From("type:" + type.FullName),
            BrickElementKind.Type,
            type.Name,
            type.Assembly.GetName().Name,
            type.Namespace,
            type.FullName);

    private static IReadOnlyList<BrickResolvedRoles> ResolveRolesFromAttributes(IEnumerable<Type> types) =>
        types.Select(type =>
        {
            var element = Element(type);
            var assignments = type
                .GetCustomAttributes<RoleAttribute>(inherit: false)
                .Select(role => Assignment(element, role.Id))
                .ToArray();

            return new BrickResolvedRoles(element, assignments, assignments, null, null);
        }).ToArray();

    private static BrickRoleAssignment Assignment(BrickElement element, RoleId roleId) =>
        new(
            new BrickElementSelector(element.Kind, element.Id.Value, element.AssemblyName),
            roleId,
            BrickAssignmentMode.DirectAttribute,
            BrickAssignmentSource.SourceAttribute,
            new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.Direct),
            BrickAssignmentBehavior.Apply);
}

/// <summary>
/// Evaluation output for the type source and target sample.
/// </summary>
public sealed class TypeCaseResult
{
    public TypeCaseResult(
        IEnumerable<BrickElement> elements,
        IEnumerable<BrickDependency> dependencies,
        IEnumerable<BrickResolvedRoles> roles,
        IEnumerable<BrickViolation> violations)
    {
        Elements = elements.ToArray();
        Dependencies = dependencies.ToArray();
        Roles = roles.ToArray();
        Violations = violations.ToArray();
    }

    public IReadOnlyList<BrickElement> Elements { get; }
    public IReadOnlyList<BrickDependency> Dependencies { get; }
    public IReadOnlyList<BrickResolvedRoles> Roles { get; }
    public IReadOnlyList<BrickViolation> Violations { get; }
}

/// <summary>Role identifiers used by the type-level sample.</summary>
public static class TypeCaseRoles
{
    public const string Endpoint = "Sample.Type.Endpoint";
    public const string ApplicationService = "Sample.Type.ApplicationService";
    public const string RepositoryContract = "Sample.Type.RepositoryContract";
    public const string ValueObject = "Sample.Type.ValueObject";
    public const string InfrastructureAdapter = "Sample.Type.InfrastructureAdapter";
}

/// <summary>Rule identifiers used by the type-level sample.</summary>
public static class TypeCaseRules
{
    public const string PolicyId = "SOURCE-TARGET-TYPE-POLICY";
    public const string EndpointRequiresApplication = "SOURCE-TARGET-TYPE-001";
    public const string ApplicationRequiresRepositoryContract = "SOURCE-TARGET-TYPE-002";
    public const string ValueObjectMustNotUseInfrastructure = "SOURCE-TARGET-TYPE-003";
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(TypeCaseRoles.Endpoint)]
public sealed class SampleEndpointAttribute : RoleAttribute
{
    public SampleEndpointAttribute() : base(TypeCaseRoles.Endpoint)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(TypeCaseRoles.ApplicationService)]
public sealed class SampleApplicationServiceAttribute : RoleAttribute
{
    public SampleApplicationServiceAttribute() : base(TypeCaseRoles.ApplicationService)
    {
    }
}

[AttributeUsage(AttributeTargets.Interface)]
[RoleAlias(TypeCaseRoles.RepositoryContract)]
public sealed class SampleRepositoryContractAttribute : RoleAttribute
{
    public SampleRepositoryContractAttribute() : base(TypeCaseRoles.RepositoryContract)
    {
    }
}

[AttributeUsage(AttributeTargets.Struct)]
[RoleAlias(TypeCaseRoles.ValueObject)]
public sealed class SampleValueObjectAttribute : RoleAttribute
{
    public SampleValueObjectAttribute() : base(TypeCaseRoles.ValueObject)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(TypeCaseRoles.InfrastructureAdapter)]
public sealed class SampleInfrastructureAdapterAttribute : RoleAttribute
{
    public SampleInfrastructureAdapterAttribute() : base(TypeCaseRoles.InfrastructureAdapter)
    {
    }
}

[SampleEndpoint]
public sealed class OrderEndpoint
{
}

[SampleApplicationService]
public sealed class SubmitOrderWorkflow
{
}

[SampleRepositoryContract]
public interface IOrderRepository
{
}

[SampleValueObject]
public readonly struct Money
{
}

[SampleInfrastructureAdapter]
public sealed class SqlOrderRepository
{
}
