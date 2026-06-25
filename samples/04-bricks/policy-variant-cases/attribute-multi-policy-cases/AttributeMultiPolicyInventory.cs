using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeMultiPolicy;


public sealed class AttributeMultiPolicyInventory
{
    public AttributeMultiPolicyInventory(IEnumerable<AttributeMultiPolicyDefinition> definitions)
    {
        Definitions = definitions.ToArray();
    }

    public IReadOnlyList<AttributeMultiPolicyDefinition> Definitions { get; }
}
