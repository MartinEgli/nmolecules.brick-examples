using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;

/// <summary>
/// Keeps the former code-policy entry point, but now resolves the DDD sample
/// policy from assembly attributes only.
/// </summary>
/// <remarks>
/// The policy is declared through <c>[assembly: Policy(...)]</c>,
/// <c>[assembly: Rule(...)]</c> and <c>[assembly: Dependency(...)]</c> in
/// <c>DddBrickRules.Assembly.cs</c>. This compatibility wrapper does not
/// configure rules, dependencies or policies with C# object blocks.
/// </remarks>
internal static class DddCodePolicyExample
{
    public static BrickPolicy BuildPolicy()
    {
        return DddAttributeOnlyConfigurationExample.BuildPolicyFromAssemblyAttributes();
    }

    public static IReadOnlyList<BrickViolation> EvaluateExampleDependency()
    {
        return DddAttributeOnlyConfigurationExample.EvaluateExampleDependency();
    }

    public static BrickRoleAssignment[] CreateAssignmentsFromDddAttributeNames()
    {
        var element = new BrickElement(
            BrickElementId.From("type:Samples." + nameof(Contract)),
            BrickElementKind.Type,
            nameof(Contract),
            "Samples.Block04.Bricks",
            "Samples.Block04.Bricks.DddBuilding",
            "Samples.Block04.Bricks.DddBuilding." + nameof(Contract));

        return BrickBuiltInAttributeRoleBridges.Ddd
            .CreateAssignments(element, new[] { "AggregateRootAttribute", "EntityAttribute" })
            .ToArray();
    }
}
