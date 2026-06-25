using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireMemberCount(typeof(RepeatedMarkerAttribute), 2)]
public sealed class ExactlyTwoUseCaseAttribute : Attribute
{
}
