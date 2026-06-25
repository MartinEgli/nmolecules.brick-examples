using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireMemberRange(typeof(RangeMarkerAttribute), 2, 4)]
public sealed class TwoToFourUseCaseAttribute : Attribute
{
}
