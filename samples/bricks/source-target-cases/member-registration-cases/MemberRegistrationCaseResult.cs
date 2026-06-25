using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases;


/// <summary>Evaluation output for member and dependency-registration examples.</summary>
public sealed class MemberRegistrationCaseResult
{
    public MemberRegistrationCaseResult(
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
