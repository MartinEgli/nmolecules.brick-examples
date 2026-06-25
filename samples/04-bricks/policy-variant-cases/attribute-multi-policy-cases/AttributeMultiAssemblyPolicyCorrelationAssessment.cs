using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public sealed class AttributeMultiAssemblyPolicyCorrelationAssessment
{
    public AttributeMultiAssemblyPolicyCorrelationAssessment(
        bool hasSeveralPoliciesInAssembly,
        bool usesIdPrefixConvention,
        bool needsPolicyIdOnRuleAttributes,
        bool needsPolicyIdOnDependencyAttributes)
    {
        HasSeveralPoliciesInAssembly = hasSeveralPoliciesInAssembly;
        UsesIdPrefixConvention = usesIdPrefixConvention;
        NeedsPolicyIdOnRuleAttributes = needsPolicyIdOnRuleAttributes;
        NeedsPolicyIdOnDependencyAttributes = needsPolicyIdOnDependencyAttributes;
    }

    public bool HasSeveralPoliciesInAssembly { get; }
    public bool UsesIdPrefixConvention { get; }
    public bool NeedsPolicyIdOnRuleAttributes { get; }
    public bool NeedsPolicyIdOnDependencyAttributes { get; }

    public static AttributeMultiAssemblyPolicyCorrelationAssessment From(AttributeMultiAssemblyPolicyInventory inventory)
    {
        var hasSeveralPolicies = inventory.Policies.Count > 1;
        var usesConvention = inventory.Rules.All(rule => rule.Id.StartsWith(AttributeMultiRuleIds.AssemblyPrefix, StringComparison.Ordinal)) &&
            inventory.Dependencies.All(dependency => dependency.Id.StartsWith(AttributeMultiDependencyIds.AssemblyPrefix, StringComparison.Ordinal));

        return new AttributeMultiAssemblyPolicyCorrelationAssessment(
            hasSeveralPolicies,
            usesConvention,
            needsPolicyIdOnRuleAttributes: hasSeveralPolicies,
            needsPolicyIdOnDependencyAttributes: hasSeveralPolicies);
    }
}
