using System;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.DddBuilding;

internal static class DddBrickRoles
{
    public const string AggregateRoot = "DDD.AggregateRoot";
    public const string Entity = "DDD.Entity";
    public const string ValueObject = "DDD.ValueObject";
    public const string Repository = "DDD.Repository";
    public const string Factory = "DDD.Factory";
    public const string DomainService = "DDD.DomainService";
    public const string ApplicationService = "DDD.ApplicationService";
    public const string Infrastructure = "DDD.Infrastructure";
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[RoleAlias(DddBrickRoles.AggregateRoot)]
internal sealed class DddAggregateRootAttribute : RoleAttribute
{
    public DddAggregateRootAttribute() : base(DddBrickRoles.AggregateRoot)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.Entity)]
internal sealed class DddEntityAttribute : RoleAttribute
{
    public DddEntityAttribute() : base(DddBrickRoles.Entity)
    {
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[RoleAlias(DddBrickRoles.ValueObject)]
internal sealed class DddValueObjectAttribute : RoleAttribute
{
    public DddValueObjectAttribute() : base(DddBrickRoles.ValueObject)
    {
    }
}

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.Repository)]
internal sealed class DddRepositoryAttribute : RoleAttribute
{
    public DddRepositoryAttribute() : base(DddBrickRoles.Repository)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.Factory)]
internal sealed class DddFactoryAttribute : RoleAttribute
{
    public DddFactoryAttribute() : base(DddBrickRoles.Factory)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.DomainService)]
internal sealed class DddDomainServiceAttribute : RoleAttribute
{
    public DddDomainServiceAttribute() : base(DddBrickRoles.DomainService)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.ApplicationService)]
internal sealed class DddApplicationServiceAttribute : RoleAttribute
{
    public DddApplicationServiceAttribute() : base(DddBrickRoles.ApplicationService)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
[RoleAlias(DddBrickRoles.Infrastructure)]
internal sealed class DddInfrastructureAttribute : RoleAttribute
{
    public DddInfrastructureAttribute() : base(DddBrickRoles.Infrastructure)
    {
    }
}
