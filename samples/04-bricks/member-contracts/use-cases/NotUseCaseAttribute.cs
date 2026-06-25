using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[ForbidMember(typeof(ForbiddenMarkerAttribute))]
public sealed class NotUseCaseAttribute : Attribute
{
}
