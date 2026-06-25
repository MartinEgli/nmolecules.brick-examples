using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireAllMembers(typeof(MarkerAAttribute), typeof(MarkerBAttribute))]
public sealed class AndAPlusBUseCaseAttribute : Attribute
{
}
