# Unique Named Member Contract

This project demonstrates `RequireUniqueNamedMemberAttribute`.

| Sample | Intent |
| --- | --- |
| `UniqueNamedIdentifierValidSample` | One unnamed identifier plus one `X` and one `Y` identifier is valid. |
| `UniqueNamedIdentifierDuplicateNameInvalidSample` | Two `X` identifiers produce one member-cardinality violation. |
| `UniqueNamedIdentifierDuplicateUnnamedInvalidSample` | Two unnamed identifiers produce one member-cardinality violation. |

Use this pattern when a marker attribute can carry a logical name, for example
`[NamedIdentifier("X")]` and `[NamedIdentifier("Y")]`, and each name must be
unique while different names may coexist.
