using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.AttributeOnly;


public readonly struct AttributeRoleFact
{
    public AttributeRoleFact(string typeName, RoleId role)
    {
        TypeName = typeName;
        Role = role;
    }

    public string TypeName { get; }
    public RoleId Role { get; }
}
