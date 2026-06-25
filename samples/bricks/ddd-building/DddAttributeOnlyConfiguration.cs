using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;


internal sealed class DddAttributeOnlyConfiguration
{
    public DddAttributeOnlyConfiguration(
        IEnumerable<PolicyAttribute> policies,
        IEnumerable<PolicyImportAttribute> policyImports,
        IEnumerable<RuleAttribute> rules,
        IEnumerable<RoleCombinationAttribute> roleCombinations,
        IEnumerable<DependencyAttribute> dependencies,
        IEnumerable<DddTypeRoleAssignment> roleAssignments)
    {
        Policies = (policies ?? Enumerable.Empty<PolicyAttribute>()).ToArray();
        PolicyImports = (policyImports ?? Enumerable.Empty<PolicyImportAttribute>()).ToArray();
        Rules = (rules ?? Enumerable.Empty<RuleAttribute>()).ToArray();
        RoleCombinations = (roleCombinations ?? Enumerable.Empty<RoleCombinationAttribute>()).ToArray();
        Dependencies = (dependencies ?? Enumerable.Empty<DependencyAttribute>()).ToArray();
        RoleAssignments = (roleAssignments ?? Enumerable.Empty<DddTypeRoleAssignment>()).ToArray();
    }

    public IReadOnlyList<PolicyAttribute> Policies { get; }
    public IReadOnlyList<PolicyImportAttribute> PolicyImports { get; }
    public IReadOnlyList<RuleAttribute> Rules { get; }
    public IReadOnlyList<RoleCombinationAttribute> RoleCombinations { get; }
    public IReadOnlyList<DependencyAttribute> Dependencies { get; }
    public IReadOnlyList<DddTypeRoleAssignment> RoleAssignments { get; }
}
