namespace Samples.Block04.Bricks;

[XorUseCase]
public sealed class XorLeftValidSample
{
    [XorLeftMarker]
    public string Left { get; } = "left";
}

[XorUseCase]
public sealed class XorRightValidSample
{
    [XorRightMarker]
    public string Right { get; } = "right";
}

[XorUseCase]
public sealed class XorBothInvalidSample
{
    [XorLeftMarker]
    public string Left { get; } = "left";

    [XorRightMarker]
    public string Right { get; } = "right";
}

[XorUseCase]
public sealed class XorNoneInvalidSample
{
    public string Description { get; } = "no xor markers";
}
