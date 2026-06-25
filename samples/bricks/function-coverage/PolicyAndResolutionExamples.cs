using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.FunctionCoverage;

/// <summary>
/// Demonstrates the deterministic core: model elements, assign roles, compose policies,
/// resolve role conflicts, and evaluate observed dependencies.
/// </summary>
internal static class PolicyAndResolutionExamples
{
    public static IReadOnlyList<BrickViolation> EvaluateBillingArchitecture()
    {
        var application = SampleBrickModel.Type("Billing.Application.ContractApplicationService");
        var infrastructure = SampleBrickModel.Type("Billing.Infrastructure.SqlContractRepository");
        var dependency = SampleBrickModel.Dependency(application, infrastructure);

        var roleDimension = new BrickRoleDimension(
            BrickDimensionId.From("billing-layer"),
            "Billing layer",
            allowsMultipleRoles: false,
            isExclusiveByDefault: true,
            "Separates application orchestration from infrastructure.");
        var applicationRole = new BrickRole(RoleId.From("Billing.Application"), roleDimension.Id, "Application", "Layer", "Coordinates use cases.");
        var infrastructureRole = new BrickRole(RoleId.From("Billing.Infrastructure"), roleDimension.Id, "Infrastructure", "Layer", "Implements technical persistence.");

        var rule = new BrickRule(
            RuleId.From("XMoleculesBricks0001"),
            "No application to infrastructure",
            applicationRole.Id,
            infrastructureRole.Id,
            BrickDecision.Deny,
            BrickScope.Type,
            BrickSeverity.Error,
            priority: 100,
            "Application services stay testable when infrastructure is behind a contract.");
        var policy = new BrickPolicy(
            BrickPolicyId.From("billing-policy"),
            "Billing policy",
            new[] { new BrickPolicyImport(BrickPolicyId.From("platform-defaults"), BrickPolicyImportMode.Import) },
            new[] { rule },
            BrickPermissionDefault.Allow,
            BrickEnforcementMode.Enforce);

        var resolvedRoles = new[]
        {
            SampleBrickModel.Resolved(application, applicationRole.Id),
            SampleBrickModel.Resolved(infrastructure, infrastructureRole.Id)
        };

        return BrickRuleEvaluator.Evaluate(policy, new[] { dependency }, resolvedRoles);
    }

    public static BrickPolicyCompositionResult ComposeConsumerPolicy()
    {
        var platformPolicy = new BrickPolicy(
            BrickPolicyId.From("platform-defaults"),
            "Platform defaults",
            null,
            new[]
            {
                new BrickRule(
                    RuleId.From("XMoleculesBricks0002"),
                    "Infrastructure may use platform",
                    RoleId.From("Billing.Infrastructure"),
                    RoleId.From("Billing.Platform"),
                    BrickDecision.Allow)
            },
            BrickPermissionDefault.Deny,
            BrickEnforcementMode.Analyze);

        var consumerPolicy = new BrickPolicy(
            BrickPolicyId.From("billing-consumer"),
            "Billing consumer",
            new[] { new BrickPolicyImport(platformPolicy.Id, BrickPolicyImportMode.Narrow) },
            new[]
            {
                new BrickRule(
                    RuleId.From("XMoleculesBricks0002"),
                    "Infrastructure may use platform only through reviewed contracts",
                    RoleId.From("Billing.Infrastructure"),
                    RoleId.From("Billing.Platform"),
                    BrickDecision.Require,
                    severity: BrickSeverity.Warning)
            },
            new[]
            {
                new BrickRoleCombinationRule(
                    "Entity cannot also be infrastructure",
                    BrickRoleSelector.From("DDD.Entity"),
                    BrickRoleSelector.From("Billing.Infrastructure"),
                    BrickCombinationKind.Incompatible,
                    "Business objects should not own technical implementation roles.")
            },
            new[]
            {
                new BrickRoleAssignment(
                    new BrickElementSelector(BrickElementKind.Namespace, "Billing.Application.*", "Billing"),
                    RoleId.From("Billing.Application"),
                    BrickAssignmentMode.ExternalConfiguration,
                    BrickAssignmentSource.PolicyFile,
                    new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Namespace, BrickAssignmentAuthority.External),
                    BrickAssignmentBehavior.Apply,
                    "Consumer namespace convention.")
            },
            new[]
            {
                new BrickAlias(
                    "BillingApplicationService",
                    new BrickElementSelector(BrickElementKind.Type, "*ApplicationService", "Billing"),
                    RoleId.From("Billing.Application"),
                    new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.Alias),
                    BrickAssignmentBehavior.Apply,
                    "Maps naming convention to a canonical role.")
            },
            BrickPermissionDefault.Deny,
            BrickEnforcementMode.Analyze);

        return BrickPolicyComposer.Compose(consumerPolicy, new[] { platformPolicy });
    }

    public static BrickResolvedRoles ResolveConflictingRoles()
    {
        var element = SampleBrickModel.Type("Billing.Domain.ContractPolicy");
        var entity = SampleBrickModel.Assignment(element, "DDD.Entity");
        var infrastructure = SampleBrickModel.Assignment(element, "Billing.Infrastructure");
        var exclusiveRule = new BrickRoleCombinationRule(
            "Domain entity excludes infrastructure",
            BrickRoleSelector.From("DDD.Entity"),
            BrickRoleSelector.From("Billing.Infrastructure"),
            BrickCombinationKind.Exclusive,
            "A type should not be both business state and persistence implementation.");

        var resolved = BrickRoleResolver.Resolve(element, new[] { entity, infrastructure }, new[] { exclusiveRule });
        _ = BrickRoleResolver.FindResolutionViolations(resolved).ToArray();
        _ = BrickRoleResolver.FindCombinationViolations(resolved, new[] { exclusiveRule }).ToArray();

        return resolved;
    }

    public static BrickPolicyDocument LoadPolicyDocumentShape()
    {
        const string json = """
            {
              "schema": "NMolecules.Bricks.Policy/1.0",
              "policy": {
                "id": "billing-json",
                "name": "Billing JSON policy",
                "defaultDecision": "Deny",
                "enforcement": "Analyze",
                "imports": [],
                "rules": [
                  {
                    "ruleId": "XMoleculesBricks0003",
                    "name": "Application requires domain contract",
                    "sourceRole": "Billing.Application",
                    "targetRole": "Billing.Domain",
                    "decision": "Require",
                    "scope": "Type",
                    "severity": "Warning",
                    "priority": 10,
                    "reason": "Use cases should collaborate with domain contracts."
                  }
                ],
                "combinationRules": [],
                "externalAssignments": [],
                "aliases": []
              }
            }
            """;

        var document = BrickPolicyJsonSerializer.Deserialize(json);
        _ = BrickPolicyDocumentValidator.Validate(document);
        return document;
    }

    public static IReadOnlyList<object> ExplainRuleMessagingAndFilters()
    {
        var message = RuleMessage.Builder()
            .Text("Rule ")
            .Rule()
            .Text(": ")
            .Source()
            .Text(" must not depend on ")
            .Target()
            .Text(" through ")
            .Member()
            .Build();

        return new object[]
        {
            message,
            new ExcludedSourceNameContainsRuleFilter("Generated", "Migration"),
            new ExcludedTargetNameContainsRuleFilter("Dto"),
            new ExcludedMemberNameContainsRuleFilter("Designer"),
            new RequiredSourceNameContainsRuleFilter("Application"),
            new RequiredTargetNameContainsRuleFilter("Repository"),
            BrickDiagnosticIdGovernance.FindRange(RuleId.From("XMoleculesBricks0001"))
        };
    }
}
