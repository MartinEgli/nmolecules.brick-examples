using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


[Policy(
    id: AttributeMultiPolicyIds.ExperimentalPolicy,
    name: "Disabled attribute experimental policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]
[Rule(
    id: AttributeMultiRuleIds.ExperimentalRule,
    sourceRole: AttributeMultiRoles.ApplicationType,
    targetRole: AttributeMultiRoles.DomainType,
    mode: RuleMode.ForbidDependency,
    message: "Disabled experimental rule should not enter composition.")]
public sealed class AttributeMultiExperimentalPolicyDefinition
{
}
