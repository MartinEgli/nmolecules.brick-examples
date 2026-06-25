# Class Contracts

This project shows contracts attached to classes through specialized `RoleAttribute` markers.

| Case | Sample | Expected result |
| --- | --- | --- |
| Application class uses a domain class | `ValidApplicationClassSample` | Valid |
| Application class uses infrastructure and misses domain | `InvalidApplicationClassSample` | Violations |

The sample evaluates the class contracts with `BrickRuleEvaluator`, so violations carry normal Bricks rule IDs, severity, source and target evidence.
