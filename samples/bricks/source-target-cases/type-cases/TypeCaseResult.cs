using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.TypeCases;


/// <summary>
/// Evaluation output for the type source and target sample.
/// </summary>
public sealed class TypeCaseResult
{
    public TypeCaseResult(
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
