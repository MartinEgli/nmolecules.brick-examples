using System;
using NMolecules.Bricks;

namespace Samples.Bricks.Analyzers.MetadataValidation;

public sealed class DomainIdentityAttribute : Attribute
{
}

[Role("Domain")]
public sealed class ValidOrderAggregate
{
    [DomainIdentity]
    public string Id { get; init; } = string.Empty;
}

[Role("")]
[RequireMemberCount(typeof(DomainIdentityAttribute), -1)]
[RequireMemberRange(typeof(DomainIdentityAttribute), 3, 2)]
[ForbidMember(null)]
[RequireNamedMembers(typeof(DomainIdentityAttribute))]
[RequireUniqueNamedMember(null, "")]
public sealed class BrokenOrderAggregateMetadata
{
}
