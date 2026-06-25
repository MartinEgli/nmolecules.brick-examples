namespace Samples.Block04.Bricks;


[TwoToFourUseCase]
public sealed class TwoToFourTooManySample
{
    [RangeMarker]
    public string FirstMarker { get; } = "one";

    [RangeMarker]
    public string SecondMarker { get; } = "two";

    [RangeMarker]
    public string ThirdMarker { get; } = "three";

    [RangeMarker]
    public string FourthMarker { get; } = "four";

    [RangeMarker]
    public string FifthMarker { get; } = "five";
}
