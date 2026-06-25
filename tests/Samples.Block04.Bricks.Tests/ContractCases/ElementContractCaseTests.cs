using System;
using System.Linq;
using NMolecules.Bricks;
using Samples.Block04.Bricks.ClassContracts;
using Samples.Block04.Bricks.InterfaceContracts;
using Samples.Block04.Bricks.NamespaceContracts;
using Xunit;

namespace BrickExamples.Tests.ContractCases;

/// <summary>
/// Verifies class, interface and namespace contract examples with explicit
/// valid and violation paths.
/// </summary>
public sealed class ElementContractCaseTests
{
    [Fact]
    public void ClassContractsEvaluateValidAndViolationCases()
    {
        var result = ClassContractCases.Evaluate();

        Assert.Contains(result.Roles, role => role.Element.DisplayName == nameof(ValidApplicationClassSample));
        Assert.Contains(result.Roles, role => role.Element.DisplayName == nameof(InvalidApplicationClassSample));
        Assert.Equal(2, result.Violations.Count);
        AssertViolation(
            result.Violations,
            ClassContractRules.ApplicationMustNotUseInfrastructure,
            nameof(InvalidApplicationClassSample),
            nameof(InfrastructureClassSample),
            BrickSeverity.Error);
        AssertViolation(
            result.Violations,
            ClassContractRules.ApplicationRequiresDomain,
            nameof(InvalidApplicationClassSample),
            null,
            BrickSeverity.Warning);
    }

    [Fact]
    public void InterfaceContractsUseInterfaceTargetedRoleAttributes()
    {
        var repositoryTargets = Attribute.GetCustomAttribute(
            typeof(RepositoryInterfaceContractAttribute),
            typeof(AttributeUsageAttribute)) as AttributeUsageAttribute;
        var gatewayTargets = Attribute.GetCustomAttribute(
            typeof(ExternalGatewayInterfaceContractAttribute),
            typeof(AttributeUsageAttribute)) as AttributeUsageAttribute;

        Assert.NotNull(repositoryTargets);
        Assert.NotNull(gatewayTargets);
        Assert.Equal(AttributeTargets.Interface, repositoryTargets!.ValidOn);
        Assert.Equal(AttributeTargets.Interface, gatewayTargets!.ValidOn);

        var result = InterfaceContractCases.Evaluate();

        Assert.Equal(2, result.Violations.Count);
        AssertViolation(
            result.Violations,
            InterfaceContractRules.ApplicationRequiresRepositoryInterface,
            nameof(InvalidInterfaceConsumerSample),
            null,
            BrickSeverity.Warning);
        AssertViolation(
            result.Violations,
            InterfaceContractRules.ApplicationMustNotUseExternalGatewayInterface,
            nameof(InvalidInterfaceConsumerSample),
            nameof(IExternalGatewayContractSample),
            BrickSeverity.Error);
    }

    [Fact]
    public void NamespaceContractsUseModeledNamespaceAssignments()
    {
        var valid = NamespaceContractCases.EvaluateValid();
        var invalid = NamespaceContractCases.EvaluateInvalid();

        Assert.Empty(valid.Violations);
        Assert.All(valid.Roles, role => Assert.Equal(BrickElementKind.Namespace, role.Element.Kind));
        Assert.All(valid.Roles, role =>
            Assert.Equal(BrickAssignmentSource.PolicyFile, role.AppliedAssignments.Single().Source));
        Assert.Equal(2, invalid.Violations.Count);
        AssertViolation(
            invalid.Violations,
            NamespaceContractRules.ApplicationNamespaceMustNotUseInfrastructure,
            NamespaceContractNames.Application,
            NamespaceContractNames.Infrastructure,
            BrickSeverity.Error);
        AssertViolation(
            invalid.Violations,
            NamespaceContractRules.ApplicationNamespaceRequiresDomain,
            NamespaceContractNames.Application,
            null,
            BrickSeverity.Warning);
    }

    private static void AssertViolation(
        IReadOnlyList<BrickViolation> violations,
        string ruleId,
        string sourceDisplayName,
        string? targetDisplayName,
        BrickSeverity severity)
    {
        Assert.Contains(violations, violation =>
            violation.RuleId == RuleId.From(ruleId) &&
            violation.Source.DisplayName == sourceDisplayName &&
            (targetDisplayName == null || violation.Target?.DisplayName == targetDisplayName) &&
            violation.Severity == severity &&
            violation.State == BrickViolationState.Active);
    }
}
