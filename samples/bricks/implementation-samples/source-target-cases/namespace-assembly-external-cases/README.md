# Namespace, Assembly And External Cases

This project demonstrates larger dependency boundaries.

| Boundary | Purpose |
| --- | --- |
| Namespace | Domain namespace must not depend on infrastructure namespace. |
| Assembly | Application assembly requires a contracts assembly. |
| External reference | Domain assembly must not depend on an external package. |

Use this sample when architecture rules are owned by packages or namespaces instead of individual classes.
