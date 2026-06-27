using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Block04.Bricks.DddBuilding;


[DddDomainService]
internal sealed class ContractPricingPolicy
{
    public bool RequiresApproval(Contract contract) =>
        contract.MonthlyTotal().Amount > 10_000m;
}
