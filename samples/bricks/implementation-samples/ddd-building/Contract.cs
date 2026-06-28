using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Block04.Bricks.DddBuilding;


[DddAggregateRoot]
internal sealed class Contract
{
    private readonly List<ContractLine> _lines = new();

    public Contract(ContractId id, CustomerId customerId)
    {
        Id = id;
        CustomerId = customerId;
    }

    [DddIdentifier]
    public ContractId Id { get; }
    public CustomerId CustomerId { get; }
    public IReadOnlyList<ContractLine> Lines => _lines;

    public void AddLine(string description, Money monthlyPrice)
    {
        _lines.Add(new ContractLine(description, monthlyPrice));
    }

    public Money MonthlyTotal() =>
        _lines.Aggregate(Money.Zero("CHF"), (sum, line) => sum.Add(line.MonthlyPrice));
}
