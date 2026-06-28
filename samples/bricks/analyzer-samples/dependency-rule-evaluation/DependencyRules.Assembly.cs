using NMolecules.Bricks;

[assembly: Policy("orders-ddd", "Orders DDD dependency policy")]
[assembly: Rule(
    "ORDERS-DDD-001",
    "Domain",
    "Infrastructure",
    RuleMode.ForbidDependency,
    "Domain model must not depend on infrastructure.")]
[assembly: Rule(
    "ORDERS-DDD-002",
    "Application",
    "Repository",
    RuleMode.RequireDependency,
    "Application handlers should depend on repository contracts.")]
