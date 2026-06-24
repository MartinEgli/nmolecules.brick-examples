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

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ForbidMemberAttribute : Attribute
{
    public ForbidMemberAttribute(Type memberAttributeType)
    {
        MemberAttributeType = memberAttributeType;
    }

    public Type MemberAttributeType { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class OnlyOneMarkerAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class RepeatedMarkerAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class RangeMarkerAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class XorLeftMarkerAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class XorRightMarkerAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MarkerAAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MarkerBAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class ForbiddenMarkerAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireExactlyOneMember(typeof(OnlyOneMarkerAttribute))]
public sealed class OnlyOneUseCaseAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireMemberCount(typeof(RepeatedMarkerAttribute), 2)]
public sealed class ExactlyTwoUseCaseAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireMemberRange(typeof(RangeMarkerAttribute), 2, 4)]
public sealed class TwoToFourUseCaseAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireExclusiveChoice(typeof(XorLeftMarkerAttribute), typeof(XorRightMarkerAttribute))]
public sealed class XorUseCaseAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireAllMembers(typeof(MarkerAAttribute), typeof(MarkerBAttribute))]
public sealed class AndAPlusBUseCaseAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[ForbidMember(typeof(ForbiddenMarkerAttribute))]
public sealed class NotUseCaseAttribute : Attribute
{
}
