using System;
using System.Collections.Generic;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.FunctionCoverage;

internal static class SampleBrickModel
{
    public static readonly DateTimeOffset GeneratedAt = new(2026, 6, 24, 8, 0, 0, TimeSpan.Zero);

    public static BrickElement Type(string fullName) =>
        new(
            BrickElementId.From("type:" + fullName),
            BrickElementKind.Type,
            fullName,
            "Billing",
            Namespace(fullName),
            fullName,
            BrickElementOrigin.Source,
            BrickElementSource.Code);

    public static BrickElement Assembly(string name) =>
        new(
            BrickElementId.From("assembly:" + name),
            BrickElementKind.Assembly,
            name,
            name,
            fullName: name,
            origin: BrickElementOrigin.Source,
            source: BrickElementSource.Code);

    public static BrickRoleAssignment Assignment(BrickElement element, string roleId, BrickAssignmentMode mode = BrickAssignmentMode.DirectAttribute) =>
        new(
            new BrickElementSelector(element.Kind, element.Id.Value, element.AssemblyName),
            RoleId.From(roleId),
            mode,
            mode == BrickAssignmentMode.ExternalConfiguration ? BrickAssignmentSource.PolicyFile : BrickAssignmentSource.SourceAttribute,
            new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.Direct),
            BrickAssignmentBehavior.Apply,
            "Example role assignment.");

    public static BrickResolvedRoles Resolved(BrickElement element, params string[] roleIds)
    {
        var assignments = new List<BrickRoleAssignment>();
        foreach (var roleId in roleIds)
        {
            assignments.Add(Assignment(element, roleId));
        }

        return new BrickResolvedRoles(element, assignments, assignments, null, null);
    }

    public static BrickDependency Dependency(BrickElement source, BrickElement target, string kind = "TypeReference") =>
        new(
            source,
            target,
            BrickDependencyKindId.From(kind),
            BrickScope.Type,
            BrickDependencyLayer.Static,
            BrickDependencyStrength.Direct,
            BrickEvidenceLevel.CompilerConfirmed,
            new BrickSourceLocation("Billing/OrderApplicationService.cs", 24, 17),
            "Application service touches infrastructure directly.");

    public static BrickViolation Violation(BrickElement source, BrickElement target) =>
        new(
            BrickViolationKind.DependencyRule,
            source,
            "Application code must depend on contracts, not infrastructure implementations.",
            BrickSeverity.Error,
            BrickViolationState.Active,
            RuleId.From("XMoleculesBricks0001"),
            "No application to infrastructure",
            target,
            new[] { RoleId.From("Billing.Application") },
            new[] { RoleId.From("Billing.Infrastructure") },
            BrickDependencyKindId.From("TypeReference"),
            BrickScope.Type,
            BrickDependencyLayer.Static,
            BrickEvidenceLevel.CompilerConfirmed);

    private static string Namespace(string fullName)
    {
        var lastDot = fullName.LastIndexOf('.');
        return lastDot < 0 ? string.Empty : fullName[..lastDot];
    }
}
