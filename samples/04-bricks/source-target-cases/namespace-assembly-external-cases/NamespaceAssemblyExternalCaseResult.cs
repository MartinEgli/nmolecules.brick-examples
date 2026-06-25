using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.NamespaceAssemblyExternalCases;


/// <summary>Evaluation output for namespace, assembly and external examples.</summary>
public sealed class NamespaceAssemblyExternalCaseResult
{
    public NamespaceAssemblyExternalCaseResult(
        IEnumerable<BrickElement> elements,
        IEnumerable<BrickDependency> dependencies,
        IEnumerable<BrickResolvedRoles> roles,
        IEnumerable<BrickViolation> violations)
    {
        Elements = elements.ToArray();
        Dependencies = dependencies.ToArray();
        Roles = roles.ToArray();
        Violations = violations.ToArray();
    }

    public IReadOnlyList<BrickElement> Elements { get; }
    public IReadOnlyList<BrickDependency> Dependencies { get; }
    public IReadOnlyList<BrickResolvedRoles> Roles { get; }
    public IReadOnlyList<BrickViolation> Violations { get; }
}
