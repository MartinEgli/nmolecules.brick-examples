namespace Samples.Block04.Bricks;

[AndAPlusBUseCase]
public sealed class AndAPlusBValidSample
{
    [MarkerA]
    public string A { get; } = "a";

    [MarkerB]
    public string B { get; } = "b";
}

[AndAPlusBUseCase]
public sealed class AndAPlusBMissingBInvalidSample
{
    [MarkerA]
    public string A { get; } = "a";
}
