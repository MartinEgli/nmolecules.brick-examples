using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Block04.Bricks.DddBuilding;


[DddFactory]
internal sealed class ContractFactory
{
    public Contract Create(CustomerId customerId, Money firstLinePrice)
    {
        var contract = new Contract(ContractId.NewId(), customerId);
        contract.AddLine("Base subscription", firstLinePrice);
        return contract;
    }
}
