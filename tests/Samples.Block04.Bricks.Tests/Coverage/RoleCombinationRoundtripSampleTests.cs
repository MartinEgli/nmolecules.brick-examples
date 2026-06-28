using System.Linq;
using NMolecules.Bricks;
using Samples.Block04.Bricks.FunctionCoverage;
using Xunit;

namespace BrickExamples.Tests.Coverage;

public sealed class RoleCombinationRoundtripSampleTests
{
    [Fact]
    public void SampleResolvesRoleHierarchyBeforeProjectingRoleCombinationException()
    {
        var result = PolicyAndResolutionExamples.ResolveRoleCombinationHierarchyAndException();

        Assert.Contains(RoleId.From("Billing.Contract"), result.ResolvedRoles.EffectiveRoles);
        Assert.Contains(RoleId.From("Billing.Shared"), result.ResolvedRoles.EffectiveRoles);
        Assert.Contains(RoleId.From("Billing.Business.Policy"), result.ResolvedRoles.EffectiveRoles);
        Assert.Contains(RoleId.From("Billing.Generated.Client"), result.ResolvedRoles.EffectiveRoles);
        Assert.DoesNotContain(RoleId.From("Billing.Business.Support"), result.ResolvedRoles.EffectiveRoles);
        Assert.Contains(result.ResolvedRoles.SuppressedAssignments, assignment =>
            assignment.RoleId == RoleId.From("Billing.Business.Support") &&
            assignment.Behavior == BrickAssignmentBehavior.Apply);
        Assert.Empty(result.ResolvedRoles.Conflicts);

        var activeViolation = Assert.Single(result.ActiveViolations);
        Assert.Equal(BrickViolationKind.RoleCombination, activeViolation.Kind);
        Assert.Equal(RuleId.From("generated-business-policy-incompatible"), activeViolation.RuleId);
        Assert.Equal(BrickViolationState.Active, activeViolation.State);

        var projectedViolation = Assert.Single(result.ProjectedViolations);
        Assert.Equal(BrickViolationState.Suppressed, projectedViolation.State);
        Assert.Contains("temporarily allowed", projectedViolation.StateReason);
    }

    [Fact]
    public void SampleDocumentsSuppressionAsReviewedRoleCombinationException()
    {
        var result = PolicyAndResolutionExamples.ResolveRoleCombinationHierarchyAndException();
        var suppression = Assert.Single(result.AdoptionDocument.Suppressions);

        Assert.Equal(RuleId.From("generated-business-policy-incompatible"), suppression.RuleId);
        Assert.Equal(BrickElementKind.Type, suppression.Selector.Kind);
        Assert.Equal("Billing.Generated.*", suppression.Selector.Pattern);
        Assert.Equal("Billing Team", suppression.Owner);
        Assert.True(result.AdoptionDocument.HasEntries);
    }
}
