using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Block04.Bricks.DddBuilding;


[DddEntity]
internal sealed class ContractLine
{
    [DddIdentifier]
    public Guid Key { get; } = Guid.NewGuid();

    public ContractLine(string description, Money monthlyPrice)
    {
        Description = description;
        MonthlyPrice = monthlyPrice;
    }

    public string Description { get; }
    public Money MonthlyPrice { get; }
}
