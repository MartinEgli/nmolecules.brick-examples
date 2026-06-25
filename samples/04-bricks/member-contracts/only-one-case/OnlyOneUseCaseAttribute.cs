using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireExactlyOneMember(typeof(OnlyOneMarkerAttribute))]
public sealed class OnlyOneUseCaseAttribute : Attribute
{
}
