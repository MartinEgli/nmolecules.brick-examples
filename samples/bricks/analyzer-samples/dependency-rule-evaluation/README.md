# Dependency Rule Analyzer Sample

This project demonstrates `BrickDependencyRuleAnalyzer`.

## Realistic Use Case

An Orders bounded context keeps aggregates persistence-free and requires application handlers to coordinate persistence through repository contracts.

## Covered Cases

| Case | Example | Expected diagnostic |
| --- | --- | --- |
| Simple valid dependency | `SubmitOrderHandler` depends on `IOrderRepository` | none |
| Multiple valid roles | domain, application, repository and infrastructure roles in one project | none |
| Forbidden field dependency | `CoupledOrderAggregate` depends on `SqlOrderRepository` | `XMoleculesBricks0001` |
| Forbidden body type usage | `BodyCoupledOrderAggregate` creates `SqlOrderRepository` inside a method body | `XMoleculesBricks0001` |
| Missing required dependency | `MissingRepositoryHandler` has no repository contract | `XMoleculesBricks0001` |

## Run

```powershell
dotnet build .\samples\bricks\analyzer-samples\dependency-rule-evaluation\Samples.Bricks.Analyzers.DependencyRuleEvaluation.csproj -v minimal
```

This project is expected to report diagnostics.

Expected result: the build fails with 3 `XMoleculesBricks0001` errors.
