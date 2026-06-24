using System.Linq;
using NMolecules.Bricks;
using Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases;
using Samples.Block04.Bricks.SourceTarget.NamespaceAssemblyExternalCases;
using Samples.Block04.Bricks.SourceTarget.TypeCases;
using Xunit;

namespace BrickExamples.Tests.SourceTargetCases;

/// <summary>
/// Verifies that the small source/target example projects cover the intended
/// Bricks element kinds and produce deterministic policy results.
/// </summary>
public sealed class SourceTargetCaseTests
{
    [Fact]
    public void TypeCasesCoverClassInterfaceAndStructTargets()
    {
        var result = TypeSourceTargetCases.Evaluate();

        Assert.Equal(5, result.Elements.Count);
        Assert.All(result.Elements, element => Assert.Equal(BrickElementKind.Type, element.Kind));
        Assert.Contains(result.Roles, role => role.EffectiveRoles.Contains(RoleId.From(TypeCaseRoles.Endpoint)));
        Assert.Contains(result.Roles, role => role.EffectiveRoles.Contains(RoleId.From(TypeCaseRoles.RepositoryContract)));
        Assert.Contains(result.Roles, role => role.EffectiveRoles.Contains(RoleId.From(TypeCaseRoles.ValueObject)));
        Assert.Collection(
            result.Violations,
            violation =>
            {
                Assert.Equal(RuleId.From(TypeCaseRules.ValueObjectMustNotUseInfrastructure), violation.RuleId);
                Assert.Equal(BrickScope.Type, violation.Scope);
                Assert.Equal("Money", violation.Source.DisplayName);
                Assert.Equal("SqlOrderRepository", violation.Target!.DisplayName);
            });
    }

    [Fact]
    public void MemberRegistrationCasesCoverMemberTypeRegistrationAndExternalTargets()
    {
        var result = MemberRegistrationSourceTargetCases.Evaluate();

        Assert.Contains(result.Elements, element => element.Kind == BrickElementKind.Member);
        Assert.Contains(result.Elements, element => element.Kind == BrickElementKind.Type);
        Assert.Contains(result.Elements, element => element.Kind == BrickElementKind.DependencyRegistration);
        Assert.Contains(result.Elements, element => element.Kind == BrickElementKind.ExternalReference);
        Assert.Contains(result.Dependencies, dependency => dependency.Layer == BrickDependencyLayer.Configuration);
        Assert.Collection(
            result.Violations,
            violation =>
            {
                Assert.Equal(RuleId.From(MemberRegistrationRules.CommandHandlerMustNotCallInfrastructure), violation.RuleId);
                Assert.Equal(BrickScope.Member, violation.Scope);
                Assert.Equal("SubmitOrderHandler.Handle", violation.Source.DisplayName);
                Assert.Equal("SqlOrderRepository", violation.Target!.DisplayName);
            });
    }

    [Fact]
    public void NamespaceAssemblyExternalCasesCoverLargeBoundaryTargets()
    {
        var result = NamespaceAssemblyExternalSourceTargetCases.Evaluate();

        Assert.Contains(result.Elements, element => element.Kind == BrickElementKind.Namespace);
        Assert.Contains(result.Elements, element => element.Kind == BrickElementKind.Assembly);
        Assert.Contains(result.Elements, element => element.Kind == BrickElementKind.ExternalReference);
        Assert.Equal(3, result.Dependencies.Count);
        Assert.Equal(2, result.Violations.Count);
        Assert.Contains(
            result.Violations,
            violation =>
                violation.RuleId == RuleId.From(NamespaceAssemblyExternalRules.DomainNamespaceMustNotUseInfrastructureNamespace) &&
                violation.Scope == BrickScope.Namespace &&
                violation.Source.DisplayName == "Samples.Orders.Domain");
        Assert.Contains(
            result.Violations,
            violation =>
                violation.RuleId == RuleId.From(NamespaceAssemblyExternalRules.DomainAssemblyMustNotUseExternalPackage) &&
                violation.Scope == BrickScope.Assembly &&
                violation.Target!.DisplayName == "Newtonsoft.Json");
    }
}
