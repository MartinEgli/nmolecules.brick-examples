namespace Samples.Block04.Bricks;


[UniqueNamedIdentifierUseCase]
public sealed class UniqueNamedIdentifierValidSample
{
    [NamedIdentifier]
    public string InternalId { get; } = "internal";

    [NamedIdentifier("X")]
    public string ExternalXId { get; } = "x-1";

    [NamedIdentifier("Y")]
    public string ExternalYId { get; } = "y-1";
}
