using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public sealed class AttributeMultiPolicyDefinition
{
    public AttributeMultiPolicyDefinition(
        string ownerTypeName,
        BrickPolicy policy,
        IEnumerable<DependencyAttribute> dependencies)
    {
        OwnerTypeName = ownerTypeName;
        Policy = policy;
        Dependencies = dependencies.ToArray();
    }

    public string OwnerTypeName { get; }
    public BrickPolicy Policy { get; }
    public IReadOnlyList<DependencyAttribute> Dependencies { get; }
}
