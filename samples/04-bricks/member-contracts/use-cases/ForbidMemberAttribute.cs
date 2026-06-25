using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ForbidMemberAttribute : Attribute
{
    public ForbidMemberAttribute(Type memberAttributeType)
    {
        MemberAttributeType = memberAttributeType;
    }

    public Type MemberAttributeType { get; }
}
