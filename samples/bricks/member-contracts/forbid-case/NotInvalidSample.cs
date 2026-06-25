namespace Samples.Block04.Bricks;


[NotUseCase]
public sealed class NotInvalidSample
{
    [ForbiddenMarker]
    public string Forbidden { get; } = "forbidden";
}
