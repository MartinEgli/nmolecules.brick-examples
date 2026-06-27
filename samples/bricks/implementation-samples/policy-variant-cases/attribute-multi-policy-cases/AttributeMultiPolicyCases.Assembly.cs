using NMolecules.Bricks;

[assembly: Policy(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiPolicyIds.AssemblyPlatformPolicy,
    name: "Assembly multi platform policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]
[assembly: Policy(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiPolicyIds.AssemblyProductPolicy,
    name: "Assembly multi product policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]
[assembly: Policy(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiPolicyIds.AssemblyTeamPolicy,
    name: "Assembly multi team policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Enforce)]
[assembly: Rule(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiRuleIds.AssemblyPlatformApplicationMustNotUseInfrastructure,
    sourceRole: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiRoles.ApplicationType,
    targetRole: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiRoles.InfrastructureType,
    mode: RuleMode.ForbidDependency,
    message: "Assembly multi application types must not use infrastructure types.")]
[assembly: Rule(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiRuleIds.AssemblyProductApplicationNamespaceMustNotUseInfrastructureNamespace,
    sourceRole: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiRoles.ApplicationNamespace,
    targetRole: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiRoles.InfrastructureNamespace,
    mode: RuleMode.ForbidDependency,
    message: "Assembly multi application namespace must not use infrastructure namespace.")]
[assembly: Rule(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiRuleIds.AssemblyTeamApplicationRequiresDomain,
    sourceRole: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiRoles.ApplicationType,
    targetRole: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiRoles.DomainType,
    mode: RuleMode.RequireDependency,
    message: "Assembly multi application types require domain types.")]
[assembly: Dependency(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiDependencyIds.AssemblyTeamApplicationToDomain,
    source: "Catalog.Application.UpdatePrice",
    target: "Catalog.Domain.Price",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]
[assembly: Dependency(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiDependencyIds.AssemblyTeamApplicationToInfrastructure,
    source: "Catalog.Application.UpdatePrice",
    target: "Catalog.Infrastructure.SqlPriceRepository",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]
[assembly: Dependency(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy.AttributeMultiDependencyIds.AssemblyTeamApplicationNamespaceToInfrastructureNamespace,
    source: "Catalog.Application",
    target: "Catalog.Infrastructure",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Namespace,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.AnalyzerInferred)]
