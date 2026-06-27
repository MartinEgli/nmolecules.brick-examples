using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


[Policy(
    id: AttributeMultiPolicyIds.ProductPolicy,
    name: "Attribute multi product policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]
[PolicyImport(AttributeMultiPolicyIds.PlatformPolicy, BrickPolicyImportMode.Narrow)]
[Rule(
    id: AttributeMultiRuleIds.ApplicationMustNotUseInfrastructure,
    sourceRole: AttributeMultiRoles.ApplicationType,
    targetRole: AttributeMultiRoles.InfrastructureType,
    mode: RuleMode.ForbidDependency,
    message: "Application types must not use infrastructure types as product errors.")]
[Rule(
    id: AttributeMultiRuleIds.ApplicationNamespaceMustNotUseInfrastructureNamespace,
    sourceRole: AttributeMultiRoles.ApplicationNamespace,
    targetRole: AttributeMultiRoles.InfrastructureNamespace,
    mode: RuleMode.ForbidDependency,
    message: "Application namespace must not use infrastructure namespace.")]
public sealed class AttributeMultiProductPolicyDefinition
{
}
