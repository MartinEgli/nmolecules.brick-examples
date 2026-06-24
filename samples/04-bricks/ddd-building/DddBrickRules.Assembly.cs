using NMolecules.Bricks;
using Samples.Block04.Bricks.DddBuilding;

[assembly: Policy(
    id: DddBrickRules.DddPolicy,
    name: DddBrickRules.DddPolicyName,
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]

[assembly: PolicyImport(
    id: DddBrickRules.PlatformDefaultsPolicy,
    mode: BrickPolicyImportMode.Extend)]

[assembly: RoleCombination(
    name: DddBrickRules.DomainModelCannotBeInfrastructureCombination,
    leftRoles: DddBrickRules.DddDomainRolePattern,
    rightRoles: DddBrickRoles.Infrastructure,
    kind: BrickCombinationKind.Incompatible,
    reason: DddBrickRules.DomainModelCannotBeInfrastructureReason)]

[assembly: Rule(
    id: DddBrickRules.DomainMustNotDependOnInfrastructure,
    sourceRole: DddBrickRoles.AggregateRoot,
    targetRole: DddBrickRoles.Infrastructure,
    mode: RuleMode.ForbidDependency,
    message: DddBrickRules.DomainMustNotDependOnInfrastructureMessage)]

[assembly: Rule(
    id: DddBrickRules.ValueObjectMustNotDependOnRepository,
    sourceRole: DddBrickRoles.ValueObject,
    targetRole: DddBrickRoles.Repository,
    mode: RuleMode.ForbidDependency,
    message: DddBrickRules.ValueObjectMustNotDependOnRepositoryMessage)]

[assembly: Rule(
    id: DddBrickRules.ApplicationServiceRequiresRepositoryContract,
    sourceRole: DddBrickRoles.ApplicationService,
    targetRole: DddBrickRoles.Repository,
    mode: RuleMode.RequireDependency,
    message: DddBrickRules.ApplicationServiceRequiresRepositoryContractMessage)]

[assembly: Dependency(
    id: DddBrickRules.ContractDependsOnInfrastructureDependency,
    source: nameof(Contract),
    target: nameof(InMemoryContractRepository),
    kind: DddBrickRules.TypeReferenceDependencyKind,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]

[assembly: Dependency(
    id: DddBrickRules.ApplicationServiceDependsOnRepositoryDependency,
    source: nameof(ContractApplicationService),
    target: nameof(IContractRepository),
    kind: DddBrickRules.TypeReferenceDependencyKind,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]

namespace Samples.Block04.Bricks.DddBuilding;

internal static class DddBrickRules
{
    public const string DddPolicy = "DDD-BRICKS-POLICY";
    public const string DddPolicyName = "DDD Bricks attribute policy";
    public const string PlatformDefaultsPolicy = "DDD-BRICKS-PLATFORM-DEFAULTS";
    public const string DomainModelCannotBeInfrastructureCombination = "Domain model cannot be infrastructure";
    public const string DddDomainRolePattern = "DDD.*";
    public const string DomainMustNotDependOnInfrastructure = "DDD-BRICKS-001";
    public const string ValueObjectMustNotDependOnRepository = "DDD-BRICKS-002";
    public const string ApplicationServiceRequiresRepositoryContract = "DDD-BRICKS-003";
    public const string ContractDependsOnInfrastructureDependency = "DDD-BRICKS-DEP-001";
    public const string ApplicationServiceDependsOnRepositoryDependency = "DDD-BRICKS-DEP-002";
    public const string TypeReferenceDependencyKind = "TypeReference";
    public const string DomainModelCannotBeInfrastructureReason = "A DDD building block should not also own infrastructure responsibilities.";
    public const string DomainMustNotDependOnInfrastructureMessage = "Rule {rule}: aggregate {source} must not depend on infrastructure {target} through {member}.";
    public const string ValueObjectMustNotDependOnRepositoryMessage = "Rule {rule}: value object {source} must stay persistence-free and must not depend on {target}.";
    public const string ApplicationServiceRequiresRepositoryContractMessage = "Rule {rule}: application service {source} should coordinate persistence through a repository contract {target}.";
}
