# Rule Filters

This area contains the shipped naming-oriented filter scenarios for Bricks
rules.

Use these samples to understand how `Required*`, `Excluded*`, and custom rule
messages narrow dependency diagnostics.

These filters are token filters on source, target and member names. They are
not the future strict naming-convention analyzer described in the Layer 3
concept documents. Use them today when a dependency rule should only apply to,
or should explicitly ignore, elements whose names contain agreed terms such as
`Contract`, `Repository`, `Legacy`, `Facade` or `Allow`.

`tests/Samples.Block04.Bricks.Tests/RuleFilterCases/RuleFilterSampleTests.cs`
keeps the mapping executable: every filter attribute is matched to a concrete
sample type or member name.
