using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


[Policy(
    id: AttributeMultiPolicyIds.TeamPolicy,
    name: "Attribute multi team policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Enforce)]
[PolicyImport(AttributeMultiPolicyIds.ProductPolicy, BrickPolicyImportMode.Import)]
[PolicyImport(AttributeMultiPolicyIds.ExperimentalPolicy, BrickPolicyImportMode.Disable)]
[Rule(
    id: AttributeMultiRuleIds.ApplicationRequiresDomain,
    sourceRole: AttributeMultiRoles.ApplicationType,
    targetRole: AttributeMultiRoles.DomainType,
    mode: RuleMode.RequireDependency,
    message: "Application types require domain types.")]
[Dependency(
    id: AttributeMultiDependencyIds.ApplicationToDomain,
    source: "Catalog.Application.UpdatePrice",
    target: "Catalog.Domain.Price",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]
[Dependency(
    id: AttributeMultiDependencyIds.ApplicationToInfrastructure,
    source: "Catalog.Application.UpdatePrice",
    target: "Catalog.Infrastructure.SqlPriceRepository",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]
[Dependency(
    id: AttributeMultiDependencyIds.ApplicationNamespaceToInfrastructureNamespace,
    source: "Catalog.Application",
    target: "Catalog.Infrastructure",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Namespace,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.AnalyzerInferred)]
public sealed class AttributeMultiTeamPolicyDefinition
{
}
