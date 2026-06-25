# Code Policy Cases

This project builds `BrickPolicy`, `BrickRule`, `BrickDependency` and resolved-role objects directly in C#.

It is useful when tests or tools need full object control instead of reading policy facts from attributes or JSON.

The sample covers:

- closed default-deny behavior
- type, member and dependency-registration dependencies
- required and forbidden dependency rules
