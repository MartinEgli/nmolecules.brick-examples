using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public sealed class AttributeMultiPolicyResult
{
    public AttributeMultiPolicyResult(
        AttributeMultiPolicyInventory inventory,
        BrickPolicyCompositionResult composition,
        IEnumerable<BrickDependency> dependencies,
        IEnumerable<BrickResolvedRoles> roles,
        IEnumerable<BrickViolation> violations,
        AttributeMultiPolicyCorrelationAssessment correlation)
    {
        Inventory = inventory;
        Composition = composition;
        Dependencies = dependencies.ToArray();
        Roles = roles.ToArray();
        Violations = violations.ToArray();
        Correlation = correlation;
    }

    public AttributeMultiPolicyInventory Inventory { get; }
    public BrickPolicyCompositionResult Composition { get; }
    public IReadOnlyList<BrickDependency> Dependencies { get; }
    public IReadOnlyList<BrickResolvedRoles> Roles { get; }
    public IReadOnlyList<BrickViolation> Violations { get; }
    public AttributeMultiPolicyCorrelationAssessment Correlation { get; }
}
