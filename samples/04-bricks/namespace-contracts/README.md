# Namespace Contracts

This project shows namespace-level contracts through modeled namespace role assignments.

C# does not support attributes on namespaces, so namespace contracts are represented as `BrickElementKind.Namespace` elements with explicit role assignments.

| Case | Dependency | Expected result |
| --- | --- | --- |
| Application namespace uses domain namespace | `Contracts.Application` -> `Contracts.Domain` | Valid |
| Application namespace uses infrastructure and misses domain | `Contracts.Application` -> `Contracts.Infrastructure` | Violations |
