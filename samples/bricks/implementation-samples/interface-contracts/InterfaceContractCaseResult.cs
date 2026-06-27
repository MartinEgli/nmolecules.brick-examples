using NMolecules.Bricks;

namespace Samples.Block04.Bricks.InterfaceContracts;


public sealed class InterfaceContractCaseResult
{
    public InterfaceContractCaseResult(
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
