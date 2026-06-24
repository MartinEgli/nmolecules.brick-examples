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

[DddEntity]
internal sealed class ContractLine
{
    public ContractLine(string description, Money monthlyPrice)
    {
        Description = description;
        MonthlyPrice = monthlyPrice;
    }

    public string Description { get; }
    public Money MonthlyPrice { get; }
}

[DddValueObject]
internal readonly record struct ContractId(Guid Value)
{
    public static ContractId NewId() => new(Guid.NewGuid());
}

[DddValueObject]
internal readonly record struct CustomerId(Guid Value);

[DddValueObject]
internal readonly record struct Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0m, currency);

    public Money Add(Money other)
    {
        if (!string.Equals(Currency, other.Currency, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Money values must use the same currency.");
        }

        return new Money(Amount + other.Amount, Currency);
    }
}

[DddRepository]
internal interface IContractRepository
{
    Contract? Find(ContractId id);
    void Save(Contract contract);
}

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

[DddDomainService]
internal sealed class ContractPricingPolicy
{
    public bool RequiresApproval(Contract contract) =>
        contract.MonthlyTotal().Amount > 10_000m;
}

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
