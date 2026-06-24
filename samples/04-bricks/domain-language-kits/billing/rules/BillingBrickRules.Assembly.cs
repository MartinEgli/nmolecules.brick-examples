using NMolecules.Bricks;
using Samples.Block04.Bricks;

// Assembly-level brick rule:
// Types tagged as Billing.Domain must not depend on types tagged as Billing.Infrastructure.
[assembly: Rule(
    id: BillingRules.DomainMustNotDependOnInfrastructure,
    sourceRole: BillingRoles.Domain,
    targetRole: BillingRoles.Infrastructure,
    mode: RuleMode.ForbidDependency,
    message: "Rule {rule}: {source} must not depend on {target} ({member}).")]

// Optional filter example: [ExcludedSourceNameContains(..., "Legacy")]
// Matches the example type LegacyContractLifecyclePolicy.
[assembly: Rule(
    id: BillingRules.DomainMustNotDependOnInfrastructureExceptLegacySource,
    sourceRole: BillingRoles.Domain,
    targetRole: BillingRoles.Infrastructure)]
[assembly: ExcludedSourceNameContains(
    BillingRules.DomainMustNotDependOnInfrastructureExceptLegacySource,
    "Legacy")]

// Optional filter example: [ExcludedTargetNameContains(..., "Facade")]
// Matches the example type InvoiceFacadeGateway.
[assembly: Rule(
    id: BillingRules.DomainMustNotDependOnInfrastructureExceptFacadeTarget,
    sourceRole: BillingRoles.Domain,
    targetRole: BillingRoles.Infrastructure)]
[assembly: ExcludedTargetNameContains(
    BillingRules.DomainMustNotDependOnInfrastructureExceptFacadeTarget,
    "Facade")]

// Optional filter example: [ExcludedMemberNameContains(..., "Allow")]
// Matches the example member ContractLifecyclePolicy.AllowManualImport().
[assembly: Rule(
    id: BillingRules.DomainMustNotDependOnInfrastructureExceptAllowMembers,
    sourceRole: BillingRoles.Domain,
    targetRole: BillingRoles.Infrastructure)]
[assembly: ExcludedMemberNameContains(
    BillingRules.DomainMustNotDependOnInfrastructureExceptAllowMembers,
    "Allow")]

// Optional filter example: [RequiredSourceNameContains(..., "Contract")]
// Matches the example source types ContractLifecyclePolicy and LegacyContractLifecyclePolicy.
[assembly: Rule(
    id: BillingRules.DomainMustNotDependOnInfrastructureForContractSources,
    sourceRole: BillingRoles.Domain,
    targetRole: BillingRoles.Infrastructure)]
[assembly: RequiredSourceNameContains(
    BillingRules.DomainMustNotDependOnInfrastructureForContractSources,
    "Contract")]

// Optional filter example: [RequiredTargetNameContains(..., "Repository")]
// Matches the example target type InvoiceRepositoryGateway.
[assembly: Rule(
    id: BillingRules.DomainMustNotDependOnInfrastructureForRepositoryTargets,
    sourceRole: BillingRoles.Domain,
    targetRole: BillingRoles.Infrastructure)]
[assembly: RequiredTargetNameContains(
    BillingRules.DomainMustNotDependOnInfrastructureForRepositoryTargets,
    "Repository")]
