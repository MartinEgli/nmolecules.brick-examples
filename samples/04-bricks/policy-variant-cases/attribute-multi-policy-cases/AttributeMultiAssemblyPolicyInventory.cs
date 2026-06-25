using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public sealed class AttributeMultiAssemblyPolicyInventory
{
    public AttributeMultiAssemblyPolicyInventory(
        IEnumerable<PolicyAttribute> policies,
        IEnumerable<RuleAttribute> rules,
        IEnumerable<DependencyAttribute> dependencies)
    {
        Policies = policies.ToArray();
        Rules = rules.ToArray();
        Dependencies = dependencies.ToArray();
    }

    public IReadOnlyList<PolicyAttribute> Policies { get; }
    public IReadOnlyList<RuleAttribute> Rules { get; }
    public IReadOnlyList<DependencyAttribute> Dependencies { get; }
}
