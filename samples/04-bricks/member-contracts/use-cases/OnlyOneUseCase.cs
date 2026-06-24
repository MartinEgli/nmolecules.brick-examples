namespace Samples.Block04.Bricks;

[OnlyOneUseCase]
public sealed class OnlyOneValidSample
{
    [OnlyOneMarker]
    public string Value { get; } = "one";
}

[OnlyOneUseCase]
public sealed class OnlyOneMissingSample
{
    public string Description { get; } = "missing single marker";
}

[OnlyOneUseCase]
public sealed class OnlyOneTooManySample
{
    [OnlyOneMarker]
    public string FirstValue { get; } = "one";

    [OnlyOneMarker]
    public string SecondValue { get; } = "two";
}
