# Member Contract Catalog

This project references all member-contract case projects and exposes `MemberContractUseCaseCatalog`.

The catalog is used by tests to verify:

- which member contracts are analyzer-backed today
- whether every member-contract project has at least one valid and invalid boundary case
- whether range, forbidden-member, and unique-named-member contracts are evaluated by the same runtime path as the other member contracts

It exists so the individual case projects stay small while the test suite can still evaluate the whole matrix.
