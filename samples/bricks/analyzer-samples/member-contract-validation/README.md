# Member Contract Analyzer Sample

This project demonstrates `BrickMemberContractAnalyzer`.

## Realistic Use Case

The Orders API requires each aggregate to expose exactly one identity, each
command processor to expose two command handlers, each controller to expose
read and write routes, each endpoint to choose read or write behavior but not
both, review panels to stay within a participant range, and public DTOs to keep
internal-secret markers out. Projection identifiers may use different logical
names such as `X` and `Y`, but each logical name must be unique.
Projection contracts can also require that specific logical names are present.

## Covered Cases

| Case | Example | Expected diagnostic |
| --- | --- | --- |
| Simple valid contract | `OrderAggregate` has exactly one identity | none |
| Custom contract attribute | `CustomContractAggregate` uses `AggregateContractAttribute` | none |
| Missing member | `MissingIdentityAggregate` | `XMoleculesBricks0003` |
| Missing member through custom contract | `MissingCustomContractIdentityAggregate` | `XMoleculesBricks0003` |
| Too few members | `TooFewCommandHandlers` | `XMoleculesBricks0005` |
| Missing required marker | `ReadOnlyController` | `XMoleculesBricks0004` |
| Ambiguous exclusive choice | `AmbiguousEndpoint` | `XMoleculesBricks0006` |
| Too few range members | `UnderstaffedReviewPanel` | `XMoleculesBricks0007` |
| Forbidden marker present | `LeakyPublicContractDto` | `XMoleculesBricks0008` |
| Required named marker missing | `MissingRequiredProjectionIdentifier` | `XMoleculesBricks0010` |
| Duplicate named marker | `DuplicateProjectionIdentifiers` | `XMoleculesBricks0009` |

## Run

```powershell
dotnet build .\samples\bricks\analyzer-samples\member-contract-validation\Samples.Bricks.Analyzers.MemberContractValidation.csproj -v minimal
```

This project is expected to report diagnostics.

Expected result: the build fails with 9 member-contract errors: two
`XMoleculesBricks0003`, one `XMoleculesBricks0004`, one
`XMoleculesBricks0005`, one `XMoleculesBricks0006`, one
`XMoleculesBricks0007`, one `XMoleculesBricks0008`, and one
`XMoleculesBricks0009`, and one `XMoleculesBricks0010`.
