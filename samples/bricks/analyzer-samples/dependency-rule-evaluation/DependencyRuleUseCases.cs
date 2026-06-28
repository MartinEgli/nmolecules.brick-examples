using NMolecules.Bricks;

namespace Samples.Bricks.Analyzers.DependencyRuleEvaluation;

[Role("Domain")]
public sealed class OrderAggregate
{
    public string Id { get; init; } = string.Empty;
}

[Role("Domain")]
public sealed class CoupledOrderAggregate
{
    private readonly SqlOrderRepository _repository = default!;
}

[Role("Domain")]
public sealed class BodyCoupledOrderAggregate
{
    public void Rehydrate()
    {
        var repository = new SqlOrderRepository();
        _ = repository;
    }
}

[Role("Application")]
public sealed class SubmitOrderHandler
{
    private readonly IOrderRepository _repository = default!;
}

[Role("Application")]
public sealed class MissingRepositoryHandler
{
    public void Submit(OrderAggregate order)
    {
    }
}

[Role("Repository")]
public interface IOrderRepository
{
    void Save(OrderAggregate order);
}

[Role("Infrastructure")]
public sealed class SqlOrderRepository : IOrderRepository
{
    public void Save(OrderAggregate order)
    {
    }
}
