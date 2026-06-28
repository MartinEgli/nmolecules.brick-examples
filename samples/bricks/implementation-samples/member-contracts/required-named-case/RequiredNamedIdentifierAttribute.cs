namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Property)]
public sealed class RequiredNamedIdentifierAttribute : Attribute
{
    public RequiredNamedIdentifierAttribute(string name)
    {
        Name = name ?? string.Empty;
    }

    public string Name { get; }
}
