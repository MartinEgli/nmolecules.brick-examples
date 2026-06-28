using NMolecules.Bricks;
using Samples.Bricks.Analyzers.GenericRoleCombinations;

[assembly: Policy(
    "generic-role-combinations",
    "Generic Bricks role-combination policy")]
[assembly: Rule(
    "GENERIC-UI-001",
    GenericRoleNames.UserInterfaceEndpoint,
    GenericRoleNames.DatabaseAdapter,
    RuleMode.ForbidDependency,
    "User-interface endpoints must not talk to database adapters directly.")]
[assembly: Rule(
    "GENERIC-WORKFLOW-001",
    GenericRoleNames.BusinessWorkflow,
    GenericRoleNames.VendorSdkAdapter,
    RuleMode.ForbidDependency,
    "Business workflows must use an internal adapter boundary instead of vendor SDKs.")]
[assembly: Rule(
    "GENERIC-FEATURE-001",
    GenericRoleNames.CheckoutFeatureInternalApi,
    GenericRoleNames.BillingFeatureInternalApi,
    RuleMode.ForbidDependency,
    "One feature's internal API must not depend on another feature's internal API.")]
[assembly: Rule(
    "GENERIC-RUNTIME-001",
    GenericRoleNames.ProductionRuntimeComponent,
    GenericRoleNames.TestHarness,
    RuleMode.ForbidDependency,
    "Production runtime code must not depend on test harnesses.")]
[assembly: Rule(
    "GENERIC-GENERATED-001",
    GenericRoleNames.GeneratedClient,
    GenericRoleNames.BusinessPolicy,
    RuleMode.ForbidDependency,
    "Generated clients must stay transport-focused and must not depend on business policy.")]
[assembly: Rule(
    "GENERIC-JOB-001",
    GenericRoleNames.ScheduledJob,
    GenericRoleNames.CheckpointStore,
    RuleMode.RequireDependency,
    "Scheduled jobs must persist progress through a checkpoint store.")]
