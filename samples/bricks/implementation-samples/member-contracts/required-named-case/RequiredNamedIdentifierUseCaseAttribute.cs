using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireNamedMembers(typeof(RequiredNamedIdentifierAttribute), "X", "Y")]
public sealed class RequiredNamedIdentifierUseCaseAttribute : Attribute
{
}
