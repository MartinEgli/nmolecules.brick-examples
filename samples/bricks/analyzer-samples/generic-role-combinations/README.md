# Generic Role Combination Analyzer Sample

This project demonstrates Bricks as a general-purpose architecture rule language.
The role names are intentionally not DDD-specific. Each name describes the
architectural use directly.

## Covered Role Combinations

| Rule | Source role | Target role | Why it is useful |
| --- | --- | --- | --- |
| `GENERIC-UI-001` | `UserInterfaceEndpoint` | `DatabaseAdapter` | UI code should go through application or service boundaries. |
| `GENERIC-WORKFLOW-001` | `BusinessWorkflow` | `VendorSdkAdapter` | Business workflows should not bind directly to vendor SDKs. |
| `GENERIC-FEATURE-001` | `CheckoutFeatureInternalApi` | `BillingFeatureInternalApi` | Feature-internal APIs should not become cross-feature contracts. |
| `GENERIC-RUNTIME-001` | `ProductionRuntimeComponent` | `TestHarness` | Production code must not depend on test-only support. |
| `GENERIC-GENERATED-001` | `GeneratedClient` | `BusinessPolicy` | Generated transport clients should stay free of business decisions. |
| `GENERIC-JOB-001` | `ScheduledJob` | `CheckpointStore` | Jobs that can rerun should declare a progress/checkpoint dependency. |

## Role Declaration Styles

The sample mixes both analyzer-backed role styles:

- direct roles through `[Role("...")]`
- consumer-friendly role attributes through `[RoleAlias("...")]`

## Run

```powershell
dotnet build .\samples\bricks\analyzer-samples\generic-role-combinations\Samples.Bricks.Analyzers.GenericRoleCombinations.csproj -v minimal
```

Expected result: the build fails with 6 `XMoleculesBricks0001` errors.
