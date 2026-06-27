using NMolecules.Bricks;

namespace Samples.Block04.Bricks.NamespaceContracts;


public sealed class NamespaceContractCaseResult
{
    public NamespaceContractCaseResult(
        BrickPolicy policy,
        IEnumerable<BrickDependency> dependencies,
        IEnumerable<BrickResolvedRoles> roles,
        IEnumerable<BrickViolation> violations)
    {
        Policy = policy;
        Dependencies = dependencies.ToArray();
        Roles = roles.ToArray();
        Violations = violations.ToArray();
    }

    public BrickPolicy Policy { get; }
    public IReadOnlyList<BrickDependency> Dependencies { get; }
    public IReadOnlyList<BrickResolvedRoles> Roles { get; }
    public IReadOnlyList<BrickViolation> Violations { get; }
}
