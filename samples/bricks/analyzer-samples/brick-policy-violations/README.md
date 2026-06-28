# Brick Policy Violation Analyzer Sample

This project is intentionally invalid. It exists so developers can see Bricks analyzer diagnostics in the IDE and in command-line builds.

## What It Shows

- a source type with a Bricks role
- a target type with a Bricks role
- assembly-level rule/dependency metadata that should produce diagnostics
- analyzer wiring through `OutputItemType="Analyzer"` and `ReferenceOutputAssembly="false"`

## How To Use

Build this project directly when you want diagnostics:

```powershell
dotnet build .\samples\bricks\analyzer-samples\brick-policy-violations\Samples.Block04.Bricks.Violations.csproj -v minimal
```

Do not add this project to the green solution path.

Expected result: the build fails with one `XMoleculesBricks0001` error and one `XMoleculesBricks0002` warning from the integration analyzer.
