using System;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;
using Samples.Block04.Bricks;
using Xunit;

namespace BrickExamples.Tests.RuleFilterCases;

/// <summary>
/// Keeps the shipped naming/filter examples executable instead of relying only
/// on comments in the sample source.
/// </summary>
public sealed class RuleFilterSampleTests
{
    [Fact]
    public void BillingRuleFiltersExposeEveryNameFilterVariant()
    {
        var filters = typeof(BillingRuleFilterExamples).Assembly
            .GetCustomAttributes<RuleFilterAttribute>()
            .ToArray();

        AssertFilter<ExcludedSourceNameContainsAttribute>(
            filters,
            BillingRules.DomainMustNotDependOnInfrastructureExceptLegacySource,
            "Legacy");
        AssertFilter<ExcludedTargetNameContainsAttribute>(
            filters,
            BillingRules.DomainMustNotDependOnInfrastructureExceptFacadeTarget,
            "Facade");
        AssertFilter<ExcludedMemberNameContainsAttribute>(
            filters,
            BillingRules.DomainMustNotDependOnInfrastructureExceptAllowMembers,
            "Allow");
        AssertFilter<RequiredSourceNameContainsAttribute>(
            filters,
            BillingRules.DomainMustNotDependOnInfrastructureForContractSources,
            "Contract");
        AssertFilter<RequiredTargetNameContainsAttribute>(
            filters,
            BillingRules.DomainMustNotDependOnInfrastructureForRepositoryTargets,
            "Repository");
    }

    [Fact]
    public void BillingRuleFilterExamplesMapTokensToConcreteSourceTargetAndMemberNames()
    {
        Assert.Contains("Legacy", nameof(LegacyContractLifecyclePolicy), StringComparison.Ordinal);
        Assert.Contains("Contract", nameof(ContractLifecyclePolicy), StringComparison.Ordinal);
        Assert.Contains("Contract", nameof(LegacyContractLifecyclePolicy), StringComparison.Ordinal);
        Assert.Contains("Facade", nameof(InvoiceFacadeGateway), StringComparison.Ordinal);
        Assert.Contains("Repository", nameof(InvoiceRepositoryGateway), StringComparison.Ordinal);

        var allowMember = typeof(ContractLifecyclePolicy).GetMethod(nameof(ContractLifecyclePolicy.AllowManualImport));
        var enforceMember = typeof(ContractLifecyclePolicy).GetMethod(nameof(ContractLifecyclePolicy.EnforceInvoiceSync));

        Assert.NotNull(allowMember);
        Assert.NotNull(enforceMember);
        Assert.Contains("Allow", allowMember!.Name, StringComparison.Ordinal);
        Assert.DoesNotContain("Allow", enforceMember!.Name, StringComparison.Ordinal);
    }

    private static void AssertFilter<TAttribute>(
        RuleFilterAttribute[] filters,
        string ruleId,
        string token)
        where TAttribute : RuleFilterAttribute
    {
        var filter = Assert.Single(filters.OfType<TAttribute>(), candidate => candidate.Rule == ruleId);

        Assert.Equal(ruleId, filter.Rule);
        Assert.Equal(RuleId.From(ruleId), filter.RuleId);
        Assert.Equal(new[] { token }, filter.Tokens);
        Assert.Equal(new[] { token }, filter.ToFilter().Tokens);
    }
}
