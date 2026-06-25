using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Block04.Bricks.DddBuilding;


[DddApplicationService]
internal sealed class ContractApplicationService
{
    private readonly IContractRepository _repository;
    private readonly ContractFactory _factory;
    private readonly ContractPricingPolicy _pricingPolicy;

    public ContractApplicationService(
        IContractRepository repository,
        ContractFactory factory,
        ContractPricingPolicy pricingPolicy)
    {
        _repository = repository;
        _factory = factory;
        _pricingPolicy = pricingPolicy;
    }

    public ContractId OpenContract(CustomerId customerId, Money firstLinePrice)
    {
        var contract = _factory.Create(customerId, firstLinePrice);
        _ = _pricingPolicy.RequiresApproval(contract);
        _repository.Save(contract);
        return contract.Id;
    }
}
