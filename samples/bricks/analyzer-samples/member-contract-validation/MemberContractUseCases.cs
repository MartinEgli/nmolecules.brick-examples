using NMolecules.Bricks;

namespace Samples.Bricks.Analyzers.MemberContractValidation;

[RequireExactlyOneMember(typeof(IdentityAttribute))]
public sealed class OrderAggregate
{
    [Identity]
    public string Id { get; init; } = string.Empty;
}

[RequireExactlyOneMember(typeof(IdentityAttribute))]
public sealed class MissingIdentityAggregate
{
}

[AggregateContract]
public sealed class CustomContractAggregate
{
    [Identity]
    public string Id { get; init; } = string.Empty;
}

[AggregateContract]
public sealed class MissingCustomContractIdentityAggregate
{
}

[RequireMemberCount(typeof(CommandHandlerAttribute), 2)]
public sealed class SubmitOrderProcessor
{
    [CommandHandler]
    public void Submit()
    {
    }

    [CommandHandler]
    public void Retry()
    {
    }
}

[RequireMemberCount(typeof(CommandHandlerAttribute), 2)]
public sealed class TooFewCommandHandlers
{
    [CommandHandler]
    public void Submit()
    {
    }
}

[RequireAllMembers(typeof(ReadRouteAttribute), typeof(WriteRouteAttribute))]
public sealed class OrdersController
{
    [ReadRoute]
    public void Get()
    {
    }

    [WriteRoute]
    public void Post()
    {
    }
}

[RequireAllMembers(typeof(ReadRouteAttribute), typeof(WriteRouteAttribute))]
public sealed class ReadOnlyController
{
    [ReadRoute]
    public void Get()
    {
    }
}

[RequireExclusiveChoice(typeof(ReadRouteAttribute), typeof(WriteRouteAttribute))]
public sealed class ReadEndpoint
{
    [ReadRoute]
    public void Get()
    {
    }
}

[RequireExclusiveChoice(typeof(ReadRouteAttribute), typeof(WriteRouteAttribute))]
public sealed class AmbiguousEndpoint
{
    [ReadRoute]
    public void Get()
    {
    }

    [WriteRoute]
    public void Post()
    {
    }
}

[RequireMemberRange(typeof(ParticipantAttribute), 2, 4)]
public sealed class ReviewPanel
{
    [Participant]
    public string PrimaryReviewer { get; init; } = string.Empty;

    [Participant]
    public string SecondaryReviewer { get; init; } = string.Empty;
}

[RequireMemberRange(typeof(ParticipantAttribute), 2, 4)]
public sealed class UnderstaffedReviewPanel
{
    [Participant]
    public string PrimaryReviewer { get; init; } = string.Empty;
}

[ForbidMember(typeof(ForbiddenSecretAttribute))]
public sealed class PublicContractDto
{
    public string ContractNumber { get; init; } = string.Empty;
}

[ForbidMember(typeof(ForbiddenSecretAttribute))]
public sealed class LeakyPublicContractDto
{
    [ForbiddenSecret]
    public string InternalSecret { get; init; } = string.Empty;
}

[RequireNamedMembers(typeof(NamedIndicatorAttribute), "X", "Y")]
public sealed class RequiredProjectionIdentifiers
{
    [NamedIndicator("X")]
    public string ExternalXId { get; init; } = string.Empty;

    [NamedIndicator("Y")]
    public string ExternalYId { get; init; } = string.Empty;
}

[RequireNamedMembers(typeof(NamedIndicatorAttribute), "X", "Y")]
public sealed class MissingRequiredProjectionIdentifier
{
    [NamedIndicator("X")]
    public string ExternalXId { get; init; } = string.Empty;
}

[RequireUniqueNamedMember(typeof(NamedIndicatorAttribute))]
public sealed class ProjectionIdentifiers
{
    [NamedIndicator("X")]
    public string ExternalXId { get; init; } = string.Empty;

    [NamedIndicator("Y")]
    public string ExternalYId { get; init; } = string.Empty;
}

[RequireUniqueNamedMember(typeof(NamedIndicatorAttribute))]
public sealed class DuplicateProjectionIdentifiers
{
    [NamedIndicator("X")]
    public string PrimaryXId { get; init; } = string.Empty;

    [NamedIndicator("X")]
    public string SecondaryXId { get; init; } = string.Empty;
}
