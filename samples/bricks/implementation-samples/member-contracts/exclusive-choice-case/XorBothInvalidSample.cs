namespace Samples.Block04.Bricks;


[XorUseCase]
public sealed class XorBothInvalidSample
{
    [XorLeftMarker]
    public string Left { get; } = "left";

    [XorRightMarker]
    public string Right { get; } = "right";
}
