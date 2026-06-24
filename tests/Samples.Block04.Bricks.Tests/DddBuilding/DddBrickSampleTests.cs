using System;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;
using Samples.Block04.Bricks.DddBuilding;
using Xunit;

namespace BrickExamples.Tests.DddBuilding;

/// <summary>
/// Verifies that the Bricks DDD sample covers the same core DDD concepts as
/// the original nMolecules DDD attribute package, but through Bricks roles and
/// attribute-only policy metadata.
/// </summary>
public sealed class DddBrickSampleTests
{
    /// <summary>
    /// Provides the expected target masks for the custom DDD role attributes
    /// used by the sample.
    /// </summary>
    public static TheoryData<Type, AttributeTargets> AttributeTargetsData => new()
    {
        { typeof(DddAggregateRootAttribute), AttributeTargets.Class | AttributeTargets.Struct },
        { typeof(DddEntityAttribute), AttributeTargets.Class },
        { typeof(DddValueObjectAttribute), AttributeTargets.Class | AttributeTargets.Struct },
        { typeof(DddRepositoryAttribute), AttributeTargets.Class | AttributeTargets.Interface },
        { typeof(DddFactoryAttribute), AttributeTargets.Class },
        { typeof(DddDomainServiceAttribute), AttributeTargets.Class },
        { typeof(DddApplicationServiceAttribute), AttributeTargets.Class },
        { typeof(DddInfrastructureAttribute), AttributeTargets.Class }
    };

    /// <summary>
    /// Ensures the custom DDD role attributes advertise the exact target set
    /// that the sample and documentation rely on.
    /// </summary>
    [Theory]
    [MemberData(nameof(AttributeTargetsData))]
    public void DeclaresExpectedDddRoleAttributeUsage(Type attributeType, AttributeTargets expectedTargets)
    {
        var usage = attributeType.GetCustomAttribute<AttributeUsageAttribute>();

        Assert.NotNull(usage);
        Assert.Equal(expectedTargets, usage!.ValidOn);
    }

    /// <summary>
    /// Verifies that every DDD sample role marker also declares the Bricks role
    /// alias that tools can use for vocabulary mapping.
    /// </summary>
    [Fact]
    public void DddRoleMarkersExposeRoleAliases()
    {
        AssertRoleAlias<DddAggregateRootAttribute>(DddBrickRoles.AggregateRoot);
        AssertRoleAlias<DddEntityAttribute>(DddBrickRoles.Entity);
        AssertRoleAlias<DddValueObjectAttribute>(DddBrickRoles.ValueObject);
        AssertRoleAlias<DddRepositoryAttribute>(DddBrickRoles.Repository);
        AssertRoleAlias<DddFactoryAttribute>(DddBrickRoles.Factory);
        AssertRoleAlias<DddDomainServiceAttribute>(DddBrickRoles.DomainService);
        AssertRoleAlias<DddApplicationServiceAttribute>(DddBrickRoles.ApplicationService);
        AssertRoleAlias<DddInfrastructureAttribute>(DddBrickRoles.Infrastructure);
    }

    /// <summary>
    /// Mirrors the original DDD package smoke tests: aggregate, entity, value
    /// object, repository, factory, domain service and application service
    /// markers are discoverable on the sample model.
    /// </summary>
    [Fact]
    public void DddRoleAttributesAreDiscoverableOnSampleTypes()
    {
        AssertRole<Contract, DddAggregateRootAttribute>(DddBrickRoles.AggregateRoot);
        AssertRole<ContractLine, DddEntityAttribute>(DddBrickRoles.Entity);
        AssertRole<ContractId, DddValueObjectAttribute>(DddBrickRoles.ValueObject);
        AssertRole<CustomerId, DddValueObjectAttribute>(DddBrickRoles.ValueObject);
        AssertRole<Money, DddValueObjectAttribute>(DddBrickRoles.ValueObject);
        AssertRole<IContractRepository, DddRepositoryAttribute>(DddBrickRoles.Repository);
        AssertRole<ContractFactory, DddFactoryAttribute>(DddBrickRoles.Factory);
        AssertRole<ContractPricingPolicy, DddDomainServiceAttribute>(DddBrickRoles.DomainService);
        AssertRole<ContractApplicationService, DddApplicationServiceAttribute>(DddBrickRoles.ApplicationService);
        AssertRole<InMemoryContractRepository, DddInfrastructureAttribute>(DddBrickRoles.Infrastructure);
    }

    /// <summary>
    /// Verifies the assembly-level policy metadata declared by the sample:
    /// policy header, imports, role combinations, rules and dependency facts.
    /// </summary>
    [Fact]
    public void AssemblyLevelPolicyAttributesAreDiscoverable()
    {
        var assembly = typeof(Contract).Assembly;
        var policies = assembly.GetCustomAttributes<PolicyAttribute>()
            .Where(policy => policy.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .ToArray();
        var imports = assembly.GetCustomAttributes<PolicyImportAttribute>()
            .Where(import => import.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .ToArray();
        var combinations = assembly.GetCustomAttributes<RoleCombinationAttribute>()
            .Where(combination => combination.Name == "Domain model cannot be infrastructure")
            .ToArray();
        var rules = assembly.GetCustomAttributes<RuleAttribute>()
            .Where(rule => rule.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .OrderBy(rule => rule.Id, StringComparer.Ordinal)
            .ToArray();
        var dependencies = assembly.GetCustomAttributes<DependencyAttribute>()
            .Where(dependency => dependency.Id.StartsWith("DDD-BRICKS-", StringComparison.Ordinal))
            .OrderBy(dependency => dependency.Id, StringComparer.Ordinal)
            .ToArray();

        Assert.Single(policies);
        Assert.Equal(BrickPolicyId.From("DDD-BRICKS-POLICY"), policies[0].PolicyId);
        Assert.Equal("DDD Bricks attribute policy", policies[0].Name);
        Assert.Equal(BrickPermissionDefault.Allow, policies[0].DefaultDecision);
        Assert.Equal(BrickEnforcementMode.Analyze, policies[0].Enforcement);

        Assert.Single(imports);
        Assert.Equal(BrickPolicyId.From("DDD-BRICKS-PLATFORM-DEFAULTS"), imports[0].PolicyId);
        Assert.Equal(BrickPolicyImportMode.Extend, imports[0].Mode);

        Assert.Single(combinations);
        Assert.Equal(BrickRoleSelector.From("DDD.*"), combinations[0].LeftRoleSelector);
        Assert.Equal(BrickRoleSelector.From(DddBrickRoles.Infrastructure), combinations[0].RightRoleSelector);
        Assert.Equal(BrickCombinationKind.Incompatible, combinations[0].Kind);

        Assert.Collection(
            rules,
            rule =>
            {
                Assert.Equal("DDD-BRICKS-001", rule.Id);
                Assert.Equal(DddBrickRoles.AggregateRoot, rule.SourceRole);
                Assert.Equal(DddBrickRoles.Infrastructure, rule.TargetRole);
                Assert.Equal(RuleMode.ForbidDependency, rule.Mode);
            },
            rule =>
            {
                Assert.Equal("DDD-BRICKS-002", rule.Id);
                Assert.Equal(DddBrickRoles.ValueObject, rule.SourceRole);
                Assert.Equal(DddBrickRoles.Repository, rule.TargetRole);
                Assert.Equal(RuleMode.ForbidDependency, rule.Mode);
            },
            rule =>
            {
                Assert.Equal("DDD-BRICKS-003", rule.Id);
                Assert.Equal(DddBrickRoles.ApplicationService, rule.SourceRole);
                Assert.Equal(DddBrickRoles.Repository, rule.TargetRole);
                Assert.Equal(RuleMode.RequireDependency, rule.Mode);
            });

        Assert.Collection(
            dependencies,
            dependency =>
            {
                Assert.Equal("DDD-BRICKS-DEP-001", dependency.Id);
                Assert.Equal(nameof(Contract), dependency.Source);
                Assert.Equal(nameof(InMemoryContractRepository), dependency.Target);
            },
            dependency =>
            {
                Assert.Equal("DDD-BRICKS-DEP-002", dependency.Id);
                Assert.Equal(nameof(ContractApplicationService), dependency.Source);
                Assert.Equal(nameof(IContractRepository), dependency.Target);
            });
    }

    /// <summary>
    /// Verifies that the attribute-only reader sees the same DDD inventory that
    /// is visible through reflection on the sample assembly.
    /// </summary>
    [Fact]
    public void AttributeOnlyConfigurationReadsDddPolicyParts()
    {
        var configuration = DddAttributeOnlyConfigurationExample.ReadConfigurationFromAttributes();

        Assert.Single(configuration.Policies);
        Assert.Single(configuration.PolicyImports);
        Assert.Equal(3, configuration.Rules.Count);
        Assert.Single(configuration.RoleCombinations);
        Assert.Equal(2, configuration.Dependencies.Count);
        Assert.Equal(10, configuration.RoleAssignments.Count);
        Assert.Contains(configuration.RoleAssignments, assignment => assignment.TypeName == nameof(Contract) && assignment.Roles.Single() == RoleId.From(DddBrickRoles.AggregateRoot));
        Assert.Contains(configuration.RoleAssignments, assignment => assignment.TypeName == nameof(ContractLine) && assignment.Roles.Single() == RoleId.From(DddBrickRoles.Entity));
        Assert.Contains(configuration.RoleAssignments, assignment => assignment.TypeName == nameof(Money) && assignment.Roles.Single() == RoleId.From(DddBrickRoles.ValueObject));
        Assert.Contains(configuration.RoleAssignments, assignment => assignment.TypeName == nameof(IContractRepository) && assignment.Roles.Single() == RoleId.From(DddBrickRoles.Repository));
        Assert.Contains(configuration.RoleAssignments, assignment => assignment.TypeName == nameof(ContractApplicationService) && assignment.Roles.Single() == RoleId.From(DddBrickRoles.ApplicationService));
        Assert.Contains(configuration.RoleAssignments, assignment => assignment.TypeName == nameof(InMemoryContractRepository) && assignment.Roles.Single() == RoleId.From(DddBrickRoles.Infrastructure));
    }

    /// <summary>
    /// Verifies that the attribute-only policy contains imports, rules and role
    /// combination rules and still evaluates the intended infrastructure leak.
    /// </summary>
    [Fact]
    public void AttributeOnlyPolicyBuildsAndEvaluatesExpectedDddViolation()
    {
        var policy = DddAttributeOnlyConfigurationExample.BuildPolicyFromAssemblyAttributes();
        var violations = DddAttributeOnlyConfigurationExample.EvaluateExampleDependency();

        Assert.Equal(BrickPolicyId.From("DDD-BRICKS-POLICY"), policy.Id);
        Assert.Single(policy.Imports);
        Assert.Equal(3, policy.Rules.Count);
        Assert.Single(policy.CombinationRules);
        Assert.Single(violations);
        Assert.Equal(RuleId.From("DDD-BRICKS-001"), violations[0].RuleId);
        Assert.Equal(nameof(Contract), violations[0].Source.DisplayName);
        Assert.Equal(nameof(InMemoryContractRepository), violations[0].Target!.DisplayName);
        Assert.Equal(BrickSeverity.Error, violations[0].Severity);
    }

    /// <summary>
    /// Verifies that the historical sample entry points still expose the same
    /// attribute-derived policy, keeping existing readers compatible.
    /// </summary>
    [Fact]
    public void HistoricalPolicyExamplesDelegateToAttributeOnlyPolicy()
    {
        var attributePolicy = DddAttributeOnlyConfigurationExample.BuildPolicyFromAssemblyAttributes();
        var policyExample = DddBrickPolicyExample.BuildPolicy();
        var codeExample = DddCodePolicyExample.BuildPolicy();

        Assert.Equal(attributePolicy.Id, policyExample.Id);
        Assert.Equal(attributePolicy.Id, codeExample.Id);
        Assert.Equal(attributePolicy.Rules.Count, policyExample.Rules.Count);
        Assert.Equal(attributePolicy.Rules.Count, codeExample.Rules.Count);
        Assert.Equal(attributePolicy.Imports.Count, policyExample.Imports.Count);
        Assert.Equal(attributePolicy.CombinationRules.Count, codeExample.CombinationRules.Count);
        Assert.Single(DddBrickPolicyExample.EvaluateExampleDependency());
        Assert.Single(DddCodePolicyExample.EvaluateExampleDependency());
    }

    /// <summary>
    /// Verifies the domain behavior behind the role markers: factories create
    /// aggregates, value objects add safely, and application services persist
    /// through repository contracts.
    /// </summary>
    [Fact]
    public void DddSampleDomainBehaviorWorksThroughRepositoryContract()
    {
        var repository = new InMemoryContractRepository();
        var factory = new ContractFactory();
        var pricingPolicy = new ContractPricingPolicy();
        var applicationService = new ContractApplicationService(repository, factory, pricingPolicy);
        var customerId = new CustomerId(Guid.NewGuid());

        var contractId = applicationService.OpenContract(customerId, new Money(42m, "CHF"));
        var saved = repository.Find(contractId);

        Assert.NotNull(saved);
        Assert.Equal(customerId, saved!.CustomerId);
        Assert.Single(saved.Lines);
        Assert.Equal(new Money(42m, "CHF"), saved.MonthlyTotal());
        Assert.False(pricingPolicy.RequiresApproval(saved));
    }

    /// <summary>
    /// Verifies value-object safety for the sample money type.
    /// </summary>
    [Fact]
    public void MoneyRejectsMixedCurrencies()
    {
        var money = new Money(10m, "CHF");

        var exception = Assert.Throws<InvalidOperationException>(() => money.Add(new Money(10m, "EUR")));

        Assert.Equal("Money values must use the same currency.", exception.Message);
    }

    private static void AssertRoleAlias<TAttribute>(string expectedRole)
        where TAttribute : Attribute
    {
        var alias = typeof(TAttribute).GetCustomAttribute<RoleAliasAttribute>();

        Assert.NotNull(alias);
        Assert.Equal(expectedRole, alias!.Role);
        Assert.Equal(RoleId.From(expectedRole), alias.RoleId);
    }

    private static void AssertRole<TType, TAttribute>(string expectedRole)
        where TAttribute : RoleAttribute
    {
        var role = typeof(TType).GetCustomAttribute<TAttribute>(inherit: false);

        Assert.NotNull(role);
        Assert.Equal(expectedRole, role!.Name);
        Assert.Equal(RoleId.From(expectedRole), role.Id);
    }
}
