using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


// Range and negation are sample-only patterns for now. The current Bricks
// analyzer enforces the exact-one, exact-count, all-of, and XOR contracts that
// reuse the generic attributes from NMolecules.Bricks.

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class RequireMemberRangeAttribute : Attribute
{
    public RequireMemberRangeAttribute(Type memberAttributeType, int minimumCount, int maximumCount)
    {
        MemberAttributeType = memberAttributeType;
        MinimumCount = minimumCount;
        MaximumCount = maximumCount;
    }

    public Type MemberAttributeType { get; }

    public int MinimumCount { get; }

    public int MaximumCount { get; }
}
