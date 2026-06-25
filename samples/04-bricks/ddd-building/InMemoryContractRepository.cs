using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Block04.Bricks.DddBuilding;


[DddInfrastructure]
internal sealed class InMemoryContractRepository : IContractRepository
{
    private readonly Dictionary<ContractId, Contract> _contracts = new();

    public Contract? Find(ContractId id) =>
        _contracts.TryGetValue(id, out var contract) ? contract : null;

    public void Save(Contract contract)
    {
        _contracts[contract.Id] = contract;
    }
}
