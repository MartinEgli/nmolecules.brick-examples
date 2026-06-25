using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.CodePolicy;


/// <summary>
/// Demonstrates explicit C# policy objects for cases where tests need full
/// control over rules, default decisions and source/target facts.
/// </summary>
public static class CodePolicyCases
{
    public static CodePolicyCaseResult Evaluate()
    {
        var command = Element("member:Orders.CreateOrder.Handle", BrickElementKind.Member, "CreateOrder.Handle", "Orders.Application.Commands.CreateOrder.Handle");
        var repository = Element("type:Orders.Infrastructure.SqlOrderRepository", BrickElementKind.Type, "SqlOrderRepository", "Orders.Infrastructure.SqlOrderRepository");
        var registration = Element("registration:Orders.Api.Program.AddOrders", BrickElementKind.DependencyRegistration, "Program.AddOrders", "Orders.Api.Program.AddOrders");
        var externalQueue = Element("external:Azure.Storage.Queues", BrickElementKind.ExternalReference, "Azure Storage Queues", "Azure.Storage.Queues.QueueClient");
        var uncategorized = Element("type:Orders.Api.UnclassifiedController", BrickElementKind.Type, "UnclassifiedController", "Orders.Api.UnclassifiedController");
        var domain = Element("type:Orders.Domain.Order", BrickElementKind.Type, "Order", "Orders.Domain.Order");

        var dependencies = new[]
        {
            Dependency(command, repository, BrickScope.Member, BrickDependencyLayer.Static),
            Dependency(registration, externalQueue, BrickScope.Global, BrickDependencyLayer.Configuration),
            Dependency(uncategorized, domain, BrickScope.Type, BrickDependencyLayer.Static)
        };
        var roles = new[]
        {
            Resolved(command, CodePolicyRoles.CommandMember),
            Resolved(repository, CodePolicyRoles.InfrastructureType),
            Resolved(registration, CodePolicyRoles.CompositionRoot),
            Resolved(externalQueue, CodePolicyRoles.ExternalQueue),
            Resolved(uncategorized, CodePolicyRoles.UnclassifiedSource),
            Resolved(domain, CodePolicyRoles.DomainType)
        };
        var policy = BuildClosedPolicy();

        return new CodePolicyCaseResult(policy, dependencies, roles, BrickRuleEvaluator.Evaluate(policy, dependencies, roles));
    }

    public static BrickPolicy BuildClosedPolicy() =>
        new(
            BrickPolicyId.From(CodePolicyIds.Policy),
            "Code policy with closed default",
            imports: null,
            rules: new[]
            {
                new BrickRule(
                    RuleId.From(CodePolicyIds.CommandMemberMustNotUseInfrastructure),
                    "Command member must not use infrastructure type",
                    RoleId.From(CodePolicyRoles.CommandMember),
                    RoleId.From(CodePolicyRoles.InfrastructureType),
                    BrickDecision.Deny,
                    BrickScope.Member,
                    BrickSeverity.Error),
                new BrickRule(
                    RuleId.From(CodePolicyIds.CompositionRootRequiresExternalQueue),
                    "Composition root requires external queue registration",
                    RoleId.From(CodePolicyRoles.CompositionRoot),
                    RoleId.From(CodePolicyRoles.ExternalQueue),
                    BrickDecision.Require,
                    BrickScope.Global,
                    BrickSeverity.Warning),
                new BrickRule(
                    RuleId.From(CodePolicyIds.CompositionRootMayRegisterExternalQueue),
                    "Composition root may register the reviewed external queue",
                    RoleId.From(CodePolicyRoles.CompositionRoot),
                    RoleId.From(CodePolicyRoles.ExternalQueue),
                    BrickDecision.Allow,
                    BrickScope.Global,
                    BrickSeverity.Warning)
            },
            defaultDecision: BrickPermissionDefault.Deny,
            enforcement: BrickEnforcementMode.Enforce);

    private static BrickDependency Dependency(BrickElement source, BrickElement target, BrickScope scope, BrickDependencyLayer layer) =>
        new(
            source,
            target,
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            scope,
            layer,
            BrickDependencyStrength.Direct,
            layer == BrickDependencyLayer.Configuration ? BrickEvidenceLevel.ConfigurationDeclared : BrickEvidenceLevel.CompilerConfirmed);

    private static BrickResolvedRoles Resolved(BrickElement element, string role)
    {
        var assignment = new BrickRoleAssignment(
            new BrickElementSelector(element.Kind, element.Id.Value, "Samples.Block04.Bricks.PolicyVariants.CodePolicy"),
            RoleId.From(role),
            BrickAssignmentMode.ExternalConfiguration,
            BrickAssignmentSource.PolicyFile,
            new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.External),
            BrickAssignmentBehavior.Apply);

        return new BrickResolvedRoles(element, new[] { assignment }, new[] { assignment }, null, null);
    }

    private static BrickElement Element(string id, BrickElementKind kind, string displayName, string fullName) =>
        new(
            BrickElementId.From(id),
            kind,
            displayName,
            "Samples.Block04.Bricks.PolicyVariants.CodePolicy",
            NamespaceOf(fullName),
            fullName);

    private static string NamespaceOf(string fullName)
    {
        var lastDot = fullName.LastIndexOf('.');
        return lastDot < 0 ? string.Empty : fullName.Substring(0, lastDot);
    }
}
