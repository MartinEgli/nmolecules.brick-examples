namespace Samples.Block04.Bricks;


[AndAPlusBUseCase]
public sealed class AndAPlusBDuplicateMarkersValidSample
{
    [MarkerA]
    public string FirstA { get; } = "a1";

    [MarkerA]
    public string SecondA { get; } = "a2";

    [MarkerB]
    public string FirstB { get; } = "b1";

    [MarkerB]
    public string SecondB { get; } = "b2";
}
