using NMolecules.Bricks;

[assembly: Rule("BRK-BAD-CONFIG", "", "Infrastructure")]
[assembly: Rule("BRK-BAD-VIOLATION", "Domain", "Infrastructure")]

namespace Samples.Block04.Bricks.Violations;

// XMoleculesBricks0001
// Violates because a type in role "Domain" depends on a type in role "Infrastructure".
[Role("Domain")]
public sealed class BrickRuleSource
{
    private readonly BrickRuleTarget _target = default!;
}

[Role("Infrastructure")]
public sealed class BrickRuleTarget
{
}
