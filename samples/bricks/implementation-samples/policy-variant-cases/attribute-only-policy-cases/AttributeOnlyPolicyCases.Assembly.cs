using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

[assembly: Policy(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributePolicyIds.OrdersPolicy,
    name: "Orders attribute policy",
    defaultDecision: BrickPermissionDefault.Allow,
    enforcement: BrickEnforcementMode.Analyze)]
[assembly: Policy(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributePolicyIds.PaymentsPolicy,
    name: "Payments attribute policy",
    defaultDecision: BrickPermissionDefault.Deny,
    enforcement: BrickEnforcementMode.Analyze)]
[assembly: Rule(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRuleIds.OrdersDomainMustNotUseInfrastructure,
    sourceRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.DomainAggregate,
    targetRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.InfrastructureAdapter,
    mode: RuleMode.ForbidDependency,
    message: "Orders aggregate must not use infrastructure directly.")]
[assembly: Rule(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRuleIds.OrdersHandlerRequiresRepositoryContract,
    sourceRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.ApplicationHandler,
    targetRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.RepositoryContract,
    mode: RuleMode.RequireDependency,
    message: "Orders handler must use a repository contract.")]
[assembly: Rule(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRuleIds.PaymentsNamespaceMustNotUseExternalGateway,
    sourceRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.PaymentNamespace,
    targetRole: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeRoles.ExternalPaymentGateway,
    mode: RuleMode.ForbidDependency,
    message: "Payment namespace must not depend on the external gateway package.")]
[assembly: Dependency(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeDependencyIds.OrderAggregateToSqlAdapter,
    source: nameof(Samples.Block04.Bricks.PolicyVariants.AttributeOnly.OrderAggregate),
    target: nameof(Samples.Block04.Bricks.PolicyVariants.AttributeOnly.SqlOrderAdapter),
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]
[assembly: Dependency(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeDependencyIds.OrderHandlerToRepositoryContract,
    source: nameof(Samples.Block04.Bricks.PolicyVariants.AttributeOnly.SubmitOrderHandler),
    target: nameof(Samples.Block04.Bricks.PolicyVariants.AttributeOnly.IOrderRepository),
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Type,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.CompilerConfirmed)]
[assembly: Dependency(
    id: Samples.Block04.Bricks.PolicyVariants.AttributeOnly.AttributeDependencyIds.PaymentNamespaceToGateway,
    source: "Samples.Payments.Application",
    target: "Stripe",
    kind: BrickDependencyKinds.TypeReference,
    scope: BrickScope.Namespace,
    layer: BrickDependencyLayer.Static,
    strength: BrickDependencyStrength.Direct,
    evidenceLevel: BrickEvidenceLevel.AnalyzerInferred)]
