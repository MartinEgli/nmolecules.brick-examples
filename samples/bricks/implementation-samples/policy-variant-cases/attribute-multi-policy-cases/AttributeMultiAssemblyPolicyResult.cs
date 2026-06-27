using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public sealed class AttributeMultiAssemblyPolicyResult
{
    public AttributeMultiAssemblyPolicyResult(
        AttributeMultiAssemblyPolicyInventory inventory,
        BrickPolicy policy,
        IEnumerable<BrickDependency> dependencies,
        IEnumerable<BrickResolvedRoles> roles,
        IEnumerable<BrickViolation> violations,
        AttributeMultiAssemblyPolicyCorrelationAssessment correlation)
    {
        Inventory = inventory;
        Policy = policy;
        Dependencies = dependencies.ToArray();
        Roles = roles.ToArray();
        Violations = violations.ToArray();
        Correlation = correlation;
    }

    public AttributeMultiAssemblyPolicyInventory Inventory { get; }
    public BrickPolicy Policy { get; }
    public IReadOnlyList<BrickDependency> Dependencies { get; }
    public IReadOnlyList<BrickResolvedRoles> Roles { get; }
    public IReadOnlyList<BrickViolation> Violations { get; }
    public AttributeMultiAssemblyPolicyCorrelationAssessment Correlation { get; }
}
