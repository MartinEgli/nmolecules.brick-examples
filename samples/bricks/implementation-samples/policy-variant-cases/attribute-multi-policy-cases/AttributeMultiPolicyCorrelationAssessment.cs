using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public sealed class AttributeMultiPolicyCorrelationAssessment
{
    public AttributeMultiPolicyCorrelationAssessment(
        bool usesDefinitionTypeAsOwner,
        bool needsPolicyIdOnRuleAttributes,
        bool needsPolicyIdOnDependencyAttributes)
    {
        UsesDefinitionTypeAsOwner = usesDefinitionTypeAsOwner;
        NeedsPolicyIdOnRuleAttributes = needsPolicyIdOnRuleAttributes;
        NeedsPolicyIdOnDependencyAttributes = needsPolicyIdOnDependencyAttributes;
    }

    public bool UsesDefinitionTypeAsOwner { get; }
    public bool NeedsPolicyIdOnRuleAttributes { get; }
    public bool NeedsPolicyIdOnDependencyAttributes { get; }

    public static AttributeMultiPolicyCorrelationAssessment From(AttributeMultiPolicyInventory inventory)
    {
        var everyDefinitionHasPolicy = inventory.Definitions.All(definition => !definition.Policy.Id.IsEmpty);

        return new AttributeMultiPolicyCorrelationAssessment(
            usesDefinitionTypeAsOwner: everyDefinitionHasPolicy,
            needsPolicyIdOnRuleAttributes: false,
            needsPolicyIdOnDependencyAttributes: false);
    }
}
