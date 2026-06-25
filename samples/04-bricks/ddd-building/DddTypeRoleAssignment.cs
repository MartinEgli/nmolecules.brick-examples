using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


internal sealed class DddTypeRoleAssignment
{
    public DddTypeRoleAssignment(string typeName, IEnumerable<RoleId> roles)
    {
        TypeName = typeName ?? string.Empty;
        Roles = (roles ?? Enumerable.Empty<RoleId>()).ToArray();
    }

    public string TypeName { get; }
    public IReadOnlyList<RoleId> Roles { get; }
}
