# Member Contract Catalog

This project references all member-contract case projects and exposes `MemberContractUseCaseCatalog`.

The catalog is used by tests to verify:

- which member contracts are analyzer-backed today
- which concept-only contracts are documented for future analyzer work
- whether every member-contract project has at least one valid or invalid boundary case

It exists so the individual case projects stay small while the test suite can still evaluate the whole matrix.
