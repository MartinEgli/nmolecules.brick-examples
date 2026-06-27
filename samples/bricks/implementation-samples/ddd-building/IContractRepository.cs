using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Block04.Bricks.DddBuilding;


[DddRepository]
internal interface IContractRepository
{
    Contract? Find(ContractId id);
    void Save(Contract contract);
}
