using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


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
