# Enum Coverage Cases

These samples prove that every public enum value in `NMolecules.Bricks` can be represented in both supported configuration styles.

## attribute-only-enum-cases

Uses assembly-level attributes with typed enum parameters. This shows how a team can express enum-backed Bricks decisions without executable policy-building code.

## code-enum-cases

Uses a static catalog built from real enum constants. This shows how a team can keep enum-backed policy examples in ordinary C# code.

Tests compare both catalogs against the public enums in `NMolecules.Bricks` by reflection. A newly added enum value therefore requires one attribute-only example and one code example.