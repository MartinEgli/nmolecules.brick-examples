namespace Samples.Block04.Bricks;


[UniqueNamedIdentifierUseCase]
public sealed class UniqueNamedIdentifierDuplicateNameInvalidSample
{
    [NamedIdentifier("X")]
    public string PrimaryXId { get; } = "x-1";

    [NamedIdentifier("X")]
    public string SecondaryXId { get; } = "x-2";

    [NamedIdentifier("Y")]
    public string ExternalYId { get; } = "y-1";
}
