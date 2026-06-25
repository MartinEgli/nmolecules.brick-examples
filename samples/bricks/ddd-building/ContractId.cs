using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Block04.Bricks.DddBuilding;


[DddValueObject]
internal readonly record struct ContractId(Guid Value)
{
    public static ContractId NewId() => new(Guid.NewGuid());
}
