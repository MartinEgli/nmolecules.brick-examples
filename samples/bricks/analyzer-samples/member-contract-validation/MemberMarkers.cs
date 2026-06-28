using System;
using NMolecules.Bricks;

namespace Samples.Bricks.Analyzers.MemberContractValidation;

public sealed class IdentityAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
[RequireExactlyOneMember(typeof(IdentityAttribute))]
public sealed class AggregateContractAttribute : Attribute
{
}

public sealed class CommandHandlerAttribute : Attribute
{
}

public sealed class ReadRouteAttribute : Attribute
{
}

public sealed class WriteRouteAttribute : Attribute
{
}

public sealed class ParticipantAttribute : Attribute
{
}

public sealed class ForbiddenSecretAttribute : Attribute
{
}

public sealed class NamedIndicatorAttribute : Attribute
{
    public NamedIndicatorAttribute()
        : this(string.Empty)
    {
    }

    public NamedIndicatorAttribute(string name)
    {
        Name = name ?? string.Empty;
    }

    public string Name { get; }
}
