using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Class)]
[BillingDomainRole]
[RequireUniqueNamedMember(typeof(NamedIdentifierAttribute))]
public sealed class UniqueNamedIdentifierUseCaseAttribute : Attribute
{
}
