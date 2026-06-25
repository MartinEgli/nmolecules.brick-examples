# Interface Contracts

This project shows interface contracts as specialized role attributes on interfaces.

| Case | Sample | Expected result |
| --- | --- | --- |
| Application class depends on repository interface | `ValidInterfaceConsumerSample` -> `IRepositoryContractSample` | Valid |
| Application class depends on external gateway interface and misses repository interface | `InvalidInterfaceConsumerSample` -> `IExternalGatewayContractSample` | Violations |

Interfaces are still represented as `BrickElementKind.Type`; the contract is the interface-targeted role attribute.
