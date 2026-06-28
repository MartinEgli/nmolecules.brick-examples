namespace Samples.Block04.Bricks;


[RequiredNamedIdentifierUseCase]
public sealed class RequiredNamedIdentifierValidSample
{
    [RequiredNamedIdentifier("X")]
    public string ExternalIdentifier { get; init; } = string.Empty;

    [RequiredNamedIdentifier("Y")]
    public string InternalIdentifier { get; init; } = string.Empty;
}
