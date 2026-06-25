using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


public sealed class AttributeOnlyPolicyInventory
{
    public AttributeOnlyPolicyInventory(
        IEnumerable<PolicyAttribute> policies,
        IEnumerable<RuleAttribute> rules,
        IEnumerable<DependencyAttribute> dependencies,
        IEnumerable<AttributeRoleFact> roleAssignments)
    {
        Policies = policies.ToArray();
        Rules = rules.ToArray();
        Dependencies = dependencies.ToArray();
        RoleAssignments = roleAssignments.ToArray();
    }

    public IReadOnlyList<PolicyAttribute> Policies { get; }
    public IReadOnlyList<RuleAttribute> Rules { get; }
    public IReadOnlyList<DependencyAttribute> Dependencies { get; }
    public IReadOnlyList<AttributeRoleFact> RoleAssignments { get; }
}
