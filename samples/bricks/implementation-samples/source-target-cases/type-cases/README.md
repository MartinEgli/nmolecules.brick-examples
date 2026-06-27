# Type Source/Target Cases

This project demonstrates type-level dependencies.

| Element | Purpose |
| --- | --- |
| `SubmitOrderWorkflow` | Application service source role. |
| `IOrderRepository` | Required repository contract target. |
| `SqlOrderRepository` | Forbidden infrastructure target. |
| `Money` | Value object target. |
| `OrderEndpoint` | Endpoint source role. |

The project verifies how `BrickRuleEvaluator` handles forbidden and required dependencies at `BrickScope.Type`.
