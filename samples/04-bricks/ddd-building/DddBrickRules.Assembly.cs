using NMolecules.Bricks;
using Samples.Block04.Bricks.DddBuilding;

[assembly: Rule(
    id: DddBrickRules.DomainMustNotDependOnInfrastructure,
    sourceRole: DddBrickRoles.AggregateRoot,
    targetRole: DddBrickRoles.Infrastructure,
    mode: RuleMode.ForbidDependency,
    message: "Rule {rule}: aggregate {source} must not depend on infrastructure {target} through {member}.")]

[assembly: Rule(
    id: DddBrickRules.ValueObjectMustNotDependOnRepository,
    sourceRole: DddBrickRoles.ValueObject,
    targetRole: DddBrickRoles.Repository,
    mode: RuleMode.ForbidDependency,
    message: "Rule {rule}: value object {source} must stay persistence-free and must not depend on {target}.")]

[assembly: Rule(
    id: DddBrickRules.ApplicationServiceRequiresRepositoryContract,
    sourceRole: DddBrickRoles.ApplicationService,
    targetRole: DddBrickRoles.Repository,
    mode: RuleMode.RequireDependency,
    message: "Rule {rule}: application service {source} should coordinate persistence through a repository contract {target}.")]

namespace Samples.Block04.Bricks.DddBuilding;

internal static class DddBrickRules
{
    public const string DomainMustNotDependOnInfrastructure = "DDD-BRICKS-001";
    public const string ValueObjectMustNotDependOnRepository = "DDD-BRICKS-002";
    public const string ApplicationServiceRequiresRepositoryContract = "DDD-BRICKS-003";
}
