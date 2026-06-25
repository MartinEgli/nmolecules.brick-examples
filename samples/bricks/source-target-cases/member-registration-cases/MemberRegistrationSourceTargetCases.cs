using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases;


/// <summary>
/// Demonstrates member sources and dependency-registration sources.
/// </summary>
public static class MemberRegistrationSourceTargetCases
{
    /// <summary>
    /// Builds a policy where methods must not call infrastructure directly, but
    /// composition roots are allowed and expected to register external services.
    /// </summary>
    public static BrickPolicy BuildPolicy() =>
        new(
            BrickPolicyId.From(MemberRegistrationRules.PolicyId),
            "Member and registration source cases",
            imports: null,
            rules: new[]
            {
                new BrickRule(
                    RuleId.From(MemberRegistrationRules.CommandHandlerMustNotCallInfrastructure),
                    "Command handler member must not call infrastructure type",
                    RoleId.From(MemberRegistrationRoles.CommandHandlerMember),
                    RoleId.From(MemberRegistrationRoles.InfrastructureType),
                    BrickDecision.Deny,
                    BrickScope.Member,
                    BrickSeverity.Error),
                new BrickRule(
                    RuleId.From(MemberRegistrationRules.CompositionRootRequiresExternalMessageBus),
                    "Composition root must register the external message bus",
                    RoleId.From(MemberRegistrationRoles.CompositionRootRegistration),
                    RoleId.From(MemberRegistrationRoles.ExternalMessageBus),
                    BrickDecision.Require,
                    BrickScope.Global,
                    BrickSeverity.Warning)
            },
            defaultDecision: BrickPermissionDefault.Allow,
            enforcement: BrickEnforcementMode.Analyze);

    /// <summary>
    /// Creates one member-to-type violation and one dependency-registration to
    /// external-reference dependency that satisfies the required rule.
    /// </summary>
    public static MemberRegistrationCaseResult Evaluate()
    {
        var commandHandlerMember = Element(
            "member:SubmitOrderHandler.Handle",
            BrickElementKind.Member,
            "SubmitOrderHandler.Handle",
            "Samples.Block04.Bricks.MemberRegistrationCases",
            "Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases",
            "Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases.SubmitOrderHandler.Handle");
        var sqlRepositoryType = Element(
            "type:SqlOrderRepository",
            BrickElementKind.Type,
            "SqlOrderRepository",
            "Samples.Block04.Bricks.MemberRegistrationCases",
            "Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases",
            "Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases.SqlOrderRepository");
        var compositionRootRegistration = Element(
            "registration:Program.AddMessageBus",
            BrickElementKind.DependencyRegistration,
            "Program.AddMessageBus",
            "Samples.Block04.Bricks.MemberRegistrationCases",
            "Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases",
            "Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases.Program.AddMessageBus");
        var azureServiceBus = Element(
            "external:AzureServiceBus",
            BrickElementKind.ExternalReference,
            "Azure Service Bus",
            assemblyName: null,
            namespaceName: "Azure.Messaging.ServiceBus",
            fullName: "Azure.Messaging.ServiceBus.ServiceBusClient");
        var dependencies = new[]
        {
            Dependency(commandHandlerMember, sqlRepositoryType, BrickScope.Member, BrickDependencyLayer.Static),
            Dependency(compositionRootRegistration, azureServiceBus, BrickScope.Global, BrickDependencyLayer.Configuration)
        };
        var roles = new[]
        {
            Resolved(commandHandlerMember, MemberRegistrationRoles.CommandHandlerMember),
            Resolved(sqlRepositoryType, MemberRegistrationRoles.InfrastructureType),
            Resolved(compositionRootRegistration, MemberRegistrationRoles.CompositionRootRegistration),
            Resolved(azureServiceBus, MemberRegistrationRoles.ExternalMessageBus)
        };
        var violations = BrickRuleEvaluator.Evaluate(BuildPolicy(), dependencies, roles);

        return new MemberRegistrationCaseResult(
            new[] { commandHandlerMember, sqlRepositoryType, compositionRootRegistration, azureServiceBus },
            dependencies,
            roles,
            violations);
    }

    private static BrickDependency Dependency(
        BrickElement source,
        BrickElement target,
        BrickScope scope,
        BrickDependencyLayer layer) =>
        new(
            source,
            target,
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            scope,
            layer,
            BrickDependencyStrength.Direct,
            layer == BrickDependencyLayer.Configuration
                ? BrickEvidenceLevel.ConfigurationDeclared
                : BrickEvidenceLevel.CompilerConfirmed);

    private static BrickElement Element(
        string id,
        BrickElementKind kind,
        string displayName,
        string? assemblyName,
        string? namespaceName,
        string? fullName) =>
        new(
            BrickElementId.From(id),
            kind,
            displayName,
            assemblyName,
            namespaceName,
            fullName);

    private static BrickResolvedRoles Resolved(BrickElement element, string role) 
    {
        var assignment = new BrickRoleAssignment(
            new BrickElementSelector(element.Kind, element.Id.Value, element.AssemblyName),
            RoleId.From(role),
            BrickAssignmentMode.DirectAttribute,
            BrickAssignmentSource.SourceAttribute,
            new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.Direct),
            BrickAssignmentBehavior.Apply);

        return new BrickResolvedRoles(element, new[] { assignment }, new[] { assignment }, null, null);
    }
}
