using System.Linq;
using NMolecules.Bricks;
using Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;
using Samples.Block04.Bricks.PolicyVariants.AttributeOnly;
using Samples.Block04.Bricks.PolicyVariants.CodePolicy;
using Samples.Block04.Bricks.PolicyVariants.ComposedMultiPolicy;
using Samples.Block04.Bricks.PolicyVariants.JsonPolicy;
using Xunit;

namespace BrickExamples.Tests.PolicyVariantCases;

/// <summary>
/// Verifies the executable policy variant matrix for Bricks examples.
/// </summary>
public sealed class PolicyVariantCaseTests
{
    [Fact]
    public void AttributeOnlyPolicyReadsMultiplePoliciesAndExposesCorrelationTradeoff()
    {
        var inventory = AttributeOnlyPolicyCases.ReadInventory();
        var orders = AttributeOnlyPolicyCases.EvaluateOrdersPolicy();
        var payments = AttributeOnlyPolicyCases.EvaluatePaymentsPolicy();

        Assert.Equal(2, inventory.Policies.Count);
        Assert.Equal(3, inventory.Rules.Count);
        Assert.Equal(3, inventory.Dependencies.Count);
        Assert.Contains(inventory.RoleAssignments, fact => fact.Role == RoleId.From(AttributeRoles.DomainAggregate));
        Assert.True(orders.Correlation.HasSeveralPoliciesInAssembly);
        Assert.True(orders.Correlation.CanUseIdConvention);
        Assert.True(orders.Correlation.WouldBenefitFromExplicitPolicyCorrelation);
        Assert.Equal(BrickPolicyId.From(AttributePolicyIds.OrdersPolicy), orders.Policy.Id);
        Assert.Equal(BrickPolicyId.From(AttributePolicyIds.PaymentsPolicy), payments.Policy.Id);
        Assert.Contains(orders.Violations, violation => violation.RuleId == RuleId.From(AttributeRuleIds.OrdersDomainMustNotUseInfrastructure));
        Assert.Contains(payments.Violations, violation => violation.RuleId == RuleId.From(AttributeRuleIds.PaymentsNamespaceMustNotUseExternalGateway));
    }

    [Fact]
    public void AttributeMultiPolicyComposesClassScopedPolicyDefinitions()
    {
        var result = AttributeMultiPolicyCases.EvaluateTeamPolicy();
        var ruleIds = result.Composition.Policy.Rules.Select(rule => rule.RuleId).ToArray();

        Assert.Equal(4, result.Inventory.Definitions.Count);
        Assert.True(result.Correlation.UsesDefinitionTypeAsOwner);
        Assert.False(result.Correlation.NeedsPolicyIdOnRuleAttributes);
        Assert.False(result.Correlation.NeedsPolicyIdOnDependencyAttributes);
        Assert.Empty(result.Composition.Issues);
        Assert.Contains(result.Composition.Steps, step => step.PolicyId == BrickPolicyId.From(AttributeMultiPolicyIds.ExperimentalPolicy) && !step.Applied);
        Assert.Contains(RuleId.From(AttributeMultiRuleIds.ApplicationMustNotUseInfrastructure), ruleIds);
        Assert.Contains(RuleId.From(AttributeMultiRuleIds.ApplicationNamespaceMustNotUseInfrastructureNamespace), ruleIds);
        Assert.Contains(RuleId.From(AttributeMultiRuleIds.ApplicationRequiresDomain), ruleIds);
        Assert.DoesNotContain(RuleId.From(AttributeMultiRuleIds.ExperimentalRule), ruleIds);
        Assert.Equal(3, result.Dependencies.Count);
        Assert.Equal(2, result.Violations.Count);
        Assert.Contains(result.Violations, violation => violation.RuleId == RuleId.From(AttributeMultiRuleIds.ApplicationMustNotUseInfrastructure));
        Assert.Contains(result.Violations, violation => violation.RuleId == RuleId.From(AttributeMultiRuleIds.ApplicationNamespaceMustNotUseInfrastructureNamespace));
    }

    [Fact]
    public void AttributeMultiPolicyAlsoShowsAssemblyScopedPolicyFileTradeoff()
    {
        var result = AttributeMultiPolicyCases.EvaluateAssemblyTeamPolicy();
        var ruleIds = result.Policy.Rules.Select(rule => rule.RuleId).ToArray();

        Assert.Equal(3, result.Inventory.Policies.Count);
        Assert.Equal(3, result.Inventory.Rules.Count);
        Assert.Equal(3, result.Inventory.Dependencies.Count);
        Assert.True(result.Correlation.HasSeveralPoliciesInAssembly);
        Assert.True(result.Correlation.UsesIdPrefixConvention);
        Assert.True(result.Correlation.NeedsPolicyIdOnRuleAttributes);
        Assert.True(result.Correlation.NeedsPolicyIdOnDependencyAttributes);
        Assert.Equal(BrickPolicyId.From(AttributeMultiPolicyIds.AssemblyTeamPolicy), result.Policy.Id);
        Assert.Contains(RuleId.From(AttributeMultiRuleIds.AssemblyPlatformApplicationMustNotUseInfrastructure), ruleIds);
        Assert.Contains(RuleId.From(AttributeMultiRuleIds.AssemblyProductApplicationNamespaceMustNotUseInfrastructureNamespace), ruleIds);
        Assert.Contains(RuleId.From(AttributeMultiRuleIds.AssemblyTeamApplicationRequiresDomain), ruleIds);
        Assert.Equal(2, result.Violations.Count);
        Assert.Contains(result.Violations, violation => violation.RuleId == RuleId.From(AttributeMultiRuleIds.AssemblyPlatformApplicationMustNotUseInfrastructure));
        Assert.Contains(result.Violations, violation => violation.RuleId == RuleId.From(AttributeMultiRuleIds.AssemblyProductApplicationNamespaceMustNotUseInfrastructureNamespace));
    }

    [Fact]
    public void CodePolicyShowsClosedDefaultDenyAndRegistrationRequirement()
    {
        var result = CodePolicyCases.Evaluate();

        Assert.Equal(BrickPermissionDefault.Deny, result.Policy.DefaultDecision);
        Assert.Contains(result.Dependencies, dependency => dependency.Source.Kind == BrickElementKind.DependencyRegistration);
        Assert.Contains(result.Dependencies, dependency => dependency.Source.Kind == BrickElementKind.Member);
        Assert.Equal(2, result.Violations.Count);
        Assert.Contains(result.Violations, violation => violation.RuleId == RuleId.From(CodePolicyIds.CommandMemberMustNotUseInfrastructure));
        Assert.Contains(result.Violations, violation => violation.RuleId == null && violation.Source.DisplayName == "UnclassifiedController");
    }

    [Fact]
    public void JsonPolicyLoadsRulesImportsAliasesAndAssignments()
    {
        var result = JsonPolicyCases.Evaluate();

        Assert.True(result.Document.IsCurrentSchema);
        Assert.Single(result.Document.Policy.Imports);
        Assert.Single(result.Document.Policy.Aliases);
        Assert.Single(result.Document.Policy.ExternalAssignments);
        Assert.Equal(2, result.Violations.Count);
        Assert.Contains(result.Violations, violation => violation.Scope == BrickScope.Namespace);
        Assert.Contains(result.Violations, violation => violation.Scope == BrickScope.Assembly);
    }

    [Fact]
    public void ComposedPolicyCombinesPlatformProductAndTeamPolicies()
    {
        var result = ComposedMultiPolicyCases.Evaluate();
        var ruleIds = result.Composition.Policy.Rules.Select(rule => rule.RuleId).ToArray();

        Assert.Empty(result.Composition.Issues);
        Assert.Equal(3, result.Composition.Steps.Count(step => step.Applied));
        Assert.Contains(result.Composition.Steps, step => step.PolicyId == BrickPolicyId.From(ComposedPolicyIds.ExperimentalPolicy) && !step.Applied);
        Assert.Contains(RuleId.From(ComposedPolicyIds.ApplicationMustNotUseInfrastructure), ruleIds);
        Assert.DoesNotContain(RuleId.From(ComposedPolicyIds.ExperimentalRule), ruleIds);
        Assert.Contains(result.Composition.Policy.Rules, rule =>
            rule.RuleId == RuleId.From(ComposedPolicyIds.ApplicationMustNotUseInfrastructure) &&
            rule.Severity == BrickSeverity.Error);
        Assert.Equal(2, result.Violations.Count);
        Assert.Contains(result.Violations, violation => violation.RuleId == RuleId.From(ComposedPolicyIds.ApplicationMustNotUseInfrastructure));
        Assert.Contains(result.Violations, violation => violation.RuleId == RuleId.From(ComposedPolicyIds.ApplicationNamespaceMustNotUseInfrastructureNamespace));
    }
}
