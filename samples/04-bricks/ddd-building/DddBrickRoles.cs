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
