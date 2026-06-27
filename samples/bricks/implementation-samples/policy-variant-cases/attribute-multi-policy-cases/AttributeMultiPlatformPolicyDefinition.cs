using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


[Policy(
    id: AttributeMultiPolicyIds.PlatformPolicy,
    name: "Attribute multi platform policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]
[Rule(
    id: AttributeMultiRuleIds.ApplicationMustNotUseInfrastructure,
    sourceRole: AttributeMultiRoles.ApplicationType,
    targetRole: AttributeMultiRoles.InfrastructureType,
    mode: RuleMode.ForbidDependency,
    message: "Application types must not use infrastructure types.")]
public sealed class AttributeMultiPlatformPolicyDefinition
{
}
