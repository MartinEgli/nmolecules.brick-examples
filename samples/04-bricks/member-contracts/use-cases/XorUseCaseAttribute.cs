using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireExclusiveChoice(typeof(XorLeftMarkerAttribute), typeof(XorRightMarkerAttribute))]
public sealed class XorUseCaseAttribute : Attribute
{
}
