# Metadata Validation Analyzer Sample

This project demonstrates `BrickMetadataAnalyzer`.

## Realistic Use Case

A team publishes an Orders architecture policy as assembly metadata before wiring it into CI. The analyzer catches empty policy, rule, dependency and member-contract metadata early in the IDE.

## Covered Cases

| Case | Example | Expected diagnostic |
| --- | --- | --- |
| Simple valid metadata | `orders-policy` and `ORDERS001` | none |
| Multiple valid metadata | two policies plus a dependency evidence item | none |
| Empty policy id | `Policy("")` | `XMoleculesBricks0002` |
| Empty rule parts | `Rule("", "", "")` | `XMoleculesBricks0002` |
| Empty dependency parts | `Dependency("", "", "", "")` | `XMoleculesBricks0002` |
| Empty role name | `Role("")` | `XMoleculesBricks0002` |
| Invalid member contract | negative member count | `XMoleculesBricks0002` |
| Invalid member range contract | minimum greater than maximum | `XMoleculesBricks0002` |
| Invalid forbidden-member contract | missing forbidden marker type | `XMoleculesBricks0002` |
| Invalid required named member contract | no required marker names configured | `XMoleculesBricks0002` |
| Invalid unique named member contract | missing marker type and empty name argument | `XMoleculesBricks0002` |

## Run

```powershell
dotnet build .\samples\bricks\analyzer-samples\metadata-validation\Samples.Bricks.Analyzers.MetadataValidation.csproj -v minimal
```

Expected result: the build succeeds with 15 `XMoleculesBricks0002` warnings.
