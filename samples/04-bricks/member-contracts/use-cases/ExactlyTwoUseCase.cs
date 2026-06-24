namespace Samples.Block04.Bricks;

[ExactlyTwoUseCase]
public sealed class ExactlyTwoValidSample
{
    [RepeatedMarker]
    public string FirstMarker { get; } = "first";

    [RepeatedMarker]
    public string SecondMarker { get; } = "second";
}

[ExactlyTwoUseCase]
public sealed class ExactlyTwoTooFewSample
{
    [RepeatedMarker]
    public string FirstMarker { get; } = "first";
}
