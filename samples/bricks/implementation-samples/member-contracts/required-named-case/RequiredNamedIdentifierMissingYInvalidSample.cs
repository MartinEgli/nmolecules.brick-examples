namespace Samples.Block04.Bricks;


[RequiredNamedIdentifierUseCase]
public sealed class RequiredNamedIdentifierMissingYInvalidSample
{
    [RequiredNamedIdentifier("X")]
    public string ExternalIdentifier { get; init; } = string.Empty;
}
