namespace Samples.Block04.Bricks;


[ExactlyTwoUseCase]
public sealed class ExactlyTwoTooManySample
{
    [RepeatedMarker]
    public string FirstMarker { get; } = "first";

    [RepeatedMarker]
    public string SecondMarker { get; } = "second";

    [RepeatedMarker]
    public string ThirdMarker { get; } = "third";
}
