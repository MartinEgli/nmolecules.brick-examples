using NMolecules.Bricks;

namespace Samples.Block04.Bricks;


[AttributeUsage(AttributeTargets.Property)]
public sealed class NamedIdentifierAttribute : Attribute
{
    public NamedIdentifierAttribute()
        : this(string.Empty)
    {
    }

    public NamedIdentifierAttribute(string name)
    {
        Name = name ?? string.Empty;
    }

    public string Name { get; }
}
