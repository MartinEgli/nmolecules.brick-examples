namespace Samples.Block04.Bricks;

[NotUseCase]
public sealed class NotValidSample
{
    public string Description { get; } = "no forbidden markers";
}

[NotUseCase]
public sealed class NotInvalidSample
{
    [ForbiddenMarker]
    public string Forbidden { get; } = "forbidden";
}
