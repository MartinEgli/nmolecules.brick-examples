namespace Samples.Block04.Bricks;


[OnlyOneUseCase]
public sealed class OnlyOneValidSample
{
    [OnlyOneMarker]
    public string Value { get; } = "one";
}
