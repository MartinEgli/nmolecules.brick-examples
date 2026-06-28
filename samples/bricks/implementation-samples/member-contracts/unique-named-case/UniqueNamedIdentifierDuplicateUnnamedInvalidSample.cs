namespace Samples.Block04.Bricks;


[UniqueNamedIdentifierUseCase]
public sealed class UniqueNamedIdentifierDuplicateUnnamedInvalidSample
{
    [NamedIdentifier]
    public string InternalId { get; } = "internal";

    [NamedIdentifier]
    public string LegacyInternalId { get; } = "legacy";
}
