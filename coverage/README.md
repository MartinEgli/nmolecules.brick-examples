# Bricks Coverage Contracts

This folder contains machine-readable coverage contracts for the Bricks example
repository.

## `bricks-enum-variation-matrix.json`

The matrix groups public `NMolecules.Bricks` enum values into meaningful
variation families. It is intentionally not a blind Cartesian product. A row is
valid when it describes a real Bricks use case and links to an executable sample
or tracked documentation.

The sample test project checks that:

- every public Bricks enum value appears in the matrix
- every enum and value named by the matrix exists in `NMolecules.Bricks`
- every row has stable evidence
- every referenced sample path exists

The Bricks roundtrip runs these tests so enum variation gaps fail the normal
validation path.

## Violation examples

`samples/bricks/implementation-samples/function-coverage/ViolationExamples.cs`
is the compiled violation catalog. It gives each public API family and each
enum variation family a clean violation example. Analyzer-backed entries point
to intentional diagnostic projects; runtime-backed entries point to buildable
examples that model the violation state explicitly.

The sample test project checks that:

- every API family in `bricks-api-family-coverage.json` has a violation example
- every family in `bricks-enum-variation-matrix.json` has a violation example
- every public Bricks enum value appears in at least one violation example
- every referenced evidence path exists

## API-coupled enum combinations

`tests/Samples.Block04.Bricks.Tests/EnumCoverageCases/EnumDependencyCombinationTests.cs`
checks enum pairs that are coupled by public Bricks API types rather than only
by documentation rows. These tests cover rule semantics, dependency evidence,
violation projection, policy/profile defaults and reflection confidence.
