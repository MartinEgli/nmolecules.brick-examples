namespace Samples.Block04.Bricks;


[OnlyOneUseCase]
public sealed class OnlyOneTooManySample
{
    [OnlyOneMarker]
    public string FirstValue { get; } = "one";

    [OnlyOneMarker]
    public string SecondValue { get; } = "two";
}
