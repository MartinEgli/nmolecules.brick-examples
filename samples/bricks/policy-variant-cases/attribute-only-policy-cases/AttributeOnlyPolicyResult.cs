using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


public sealed class AttributeOnlyPolicyResult
{
    public AttributeOnlyPolicyResult(
        BrickPolicy policy,
        IEnumerable<BrickDependency> dependencies,
        IEnumerable<BrickResolvedRoles> roles,
        IEnumerable<BrickViolation> violations,
        AttributePolicyCorrelationAssessment correlation)
    {
        Policy = policy;
        Dependencies = dependencies.ToArray();
        Roles = roles.ToArray();
        Violations = violations.ToArray();
        Correlation = correlation;
    }

    public BrickPolicy Policy { get; }
    public IReadOnlyList<BrickDependency> Dependencies { get; }
    public IReadOnlyList<BrickResolvedRoles> Roles { get; }
    public IReadOnlyList<BrickViolation> Violations { get; }
    public AttributePolicyCorrelationAssessment Correlation { get; }
}
