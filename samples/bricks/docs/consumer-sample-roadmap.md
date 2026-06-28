# Bricks Consumer Sample Roadmap

Status baseline: June 27, 2026

This document defines a consumer-first target structure for the Bricks samples.
It is not a raw API inventory. It is a teaching and adoption plan for teams that want
to build their own architecture language on top of `NMolecules.Bricks`.

## Design Goal

The Bricks block should answer these user questions in a direct order:

1. How do I define my own domain-specific roles?
2. How do I express dependency rules between those roles?
3. How do I model marker and member contracts on consumer types?
4. Which contracts are enforced by the Roslyn analyzer today?
5. Which patterns are valid design ideas, but still conceptual for now?

## Target Structure

The long-term structure should separate sample intent, not just technical artifact type.

```text
samples/bricks/
|-- docs/
|   `-- consumer-sample-roadmap.md
|-- getting-started/
|   |-- README.md
|   |-- MinimalBricksSample.cs
|   `-- MinimalBricksViolations.cs
|-- role-modeling/
|   |-- README.md
|   |-- BasicRoleAttributes.cs
|   |-- RoleAliases.cs
|   `-- RoleCatalog.cs
|-- dependency-rules/
|   |-- README.md
|   |-- AllowRules.Assembly.cs
|   |-- DenyRules.Assembly.cs
|   |-- RequireRules.Assembly.cs
|   `-- DependencyRuleExamples.cs
|-- rule-filters/
|   |-- README.md
|   |-- MessageFilterExamples.cs
|   |-- RequiredNameExamples.cs
|   |-- ExcludedNameExamples.cs
|   `-- FilterMessageExamples.cs
|-- domain-language-kits/
|   |-- README.md
|   `-- billing/
|       |-- catalog/
|       |-- roles/
|       |-- rules/
|       |-- domain/
|       |-- application/
|       `-- infrastructure/
|-- member-contracts/
|   |-- README.md
|   `-- use-cases/
|       |-- UseCaseContractSupport.cs
|       |-- OnlyOneUseCase.cs
|       |-- ExactlyTwoUseCase.cs
|       |-- TwoToFourUseCase.cs
|       |-- XorUseCase.cs
|       |-- AndAPlusBUseCase.cs
|       `-- NotUseCase.cs
|-- composition/
|   |-- README.md
|   |-- CombinedRoleAndContractExamples.cs
|   `-- CombinedViolationExamples.cs
`-- violations/
    `-- BadBrickRules.cs
```

## Recommended Teaching Sequence

### 1. Getting Started

This should be the smallest possible end-to-end sample:

- one custom role
- one dependency rule
- one valid dependency
- one invalid dependency

Why it matters:
- Most users do not need a full domain-language kit on day one.
- They need a minimal proof that Bricks can encode one local architecture rule.

Recommended files:
- `getting-started/README.md`
- `getting-started/MinimalRoleAttribute.cs`
- `getting-started/MinimalRules.Assembly.cs`
- `getting-started/MinimalGoodDependency.cs`
- `getting-started/MinimalBadDependency.cs`

### 2. Role Modeling

This area should teach how to create a stable consumer-facing role vocabulary.

Recommended files:
- `role-modeling/BasicRoleAttributes.cs`
- `role-modeling/RoleAliases.cs`
- `role-modeling/RoleCatalog.cs`
- `role-modeling/RoleIdExamples.cs`

Consumer value:
- helps teams replace ad hoc naming conventions with explicit attributes
- shows how to publish a reusable role catalog

### 3. Dependency Rules

This should isolate dependency governance from the broader domain kit.

Recommended files:
- `dependency-rules/AllowRules.Assembly.cs`
- `dependency-rules/DenyRules.Assembly.cs`
- `dependency-rules/RequireRules.Assembly.cs`
- `dependency-rules/DependencyRuleExamples.cs`

Consumer value:
- this is the most common first production use of Bricks
- it demonstrates the difference between allowed, forbidden, and required dependencies

### 4. Rule Filters

This area should demonstrate how to narrow rules without creating noisy diagnostics.

Recommended files:
- `rule-filters/MessageFilterExamples.cs`
- `rule-filters/RequiredNameExamples.cs`
- `rule-filters/ExcludedNameExamples.cs`
- `rule-filters/FilterMessageExamples.cs`

Consumer value:
- helps teams model exceptions precisely
- prevents the first Bricks rollout from becoming too blunt

### 5. Domain-Language Kits

This is where a realistic consumer DSL should live.

Current good candidate:
- the current `domain-language-kits/billing` slice

Recommended final location:
- `domain-language-kits/billing/`

Expected content:
- role catalog
- rule catalog
- alias markers
- assembly rule registration
- domain, application, and infrastructure examples

Consumer value:
- shows how a team turns Bricks into a shared internal language

### 6. Member Contracts By Use Case

This should stay organized by use case, not by low-level analyzer primitive.

Current use cases:
- only one
- exactly two
- two to four
- XOR
- A + B
- not

Current analyzer-backed use cases:
- `OnlyOneUseCase.cs`
- `ExactlyTwoUseCase.cs`
- `TwoToFourUseCase.cs`
- `XorUseCase.cs`
- `AndAPlusBUseCase.cs`
- `NotUseCase.cs`

Consumer value:
- this is directly reusable when a team wants its own marker contracts
- it documents all current analyzer-backed member-contract capabilities

### 7. Composition

This area should show how roles, dependency rules, and member contracts combine.

Recommended files:
- `composition/CombinedRoleAndContractExamples.cs`
- `composition/CombinedViolationExamples.cs`

Examples should include:
- a type with a consumer-defined role and a member contract
- a dependency rule that applies only to a role-bearing type
- a use case where both a dependency violation and a member-contract violation can appear

Consumer value:
- real systems rarely use Bricks in isolated fragments
- teams need examples of combined usage to design coherent rule sets

## Current Mapping

These current files now anchor the first real structure slice. Paths in this
table refer to the current repository layout under
`samples/bricks/implementation-samples/` unless noted otherwise.

| Current file | Target area | Notes |
|---|---|---|
| `domain-language-kits/billing/catalog/BillingRoles.cs` | `domain-language-kits/billing/` | in place |
| `domain-language-kits/billing/catalog/BillingRules.cs` | `domain-language-kits/billing/` | in place |
| `domain-language-kits/billing/roles/BillingDomainRoleAttribute.cs`, `BillingApplicationRoleAttribute.cs`, `BillingInfrastructureRoleAttribute.cs` | `domain-language-kits/billing/` | in place |
| `domain-language-kits/billing/rules/BillingBrickRules.Assembly.cs` | `dependency-rules/` plus `domain-language-kits/billing/` | split later if needed |
| `rule-filters/BillingRuleFilterExamples.cs` | `rule-filters/` | in place |
| `domain-language-kits/billing/domain/ContractPolicy.cs` | `domain-language-kits/billing/` | in place |
| `domain-language-kits/billing/domain/IContractRepository.cs` | `domain-language-kits/billing/` | in place |
| `domain-language-kits/billing/application/ContractApplicationService.cs` | `domain-language-kits/billing/` | in place |
| `domain-language-kits/billing/infrastructure/SqlContractRepository.cs` | `domain-language-kits/billing/` | in place |
| `member-contracts/only-one-case/`, `exactly-two-case/`, `all-members-case/`, `exclusive-choice-case`, `range-case/`, `forbid-case/`, `unique-named-case/` | `member-contracts/` | analyzer-backed |
| `member-contracts/catalog/MemberContractUseCaseCatalog.cs` | `member-contracts/` | executable index for analyzer-backed cases |
| `../analyzer-samples/metadata-validation/` | `violations/` | expected Bricks configuration warnings |
| `../analyzer-samples/dependency-rule-evaluation/` | `violations/` | expected dependency-rule errors |
| `../analyzer-samples/generic-role-combinations/` | `violations/` | expected generic role-combination errors |
| `../analyzer-samples/member-contract-validation/` | `violations/` | expected member-contract errors |
| `../analyzer-samples/brick-policy-violations/` | `violations/` | expected mixed policy violation sample |

## Current Executable Sample Areas

The current implementation already contains more than the first teaching
structure. These are the executable sample areas that must stay visible in this
roadmap:

| Sample area | Current role |
|---|---|
| `ai-governance` | Advisory AI output, trust-boundary and proposal workflow examples. |
| `class-contracts` | Class-level contract and role metadata examples. |
| `ddd-building` | DDD-oriented role, identifier and member-contract examples. |
| `domain-language-kits` | Consumer-owned role/rule DSL examples, currently anchored by billing. |
| `enum-coverage-cases` | Attribute and code examples for public enum values. |
| `function-coverage` | Public API family, enum variation and violation-example evidence. |
| `implementation-analyzer-samples` | Implementation-side analyzer sample helpers. |
| `interface-contracts` | Interface-based role and contract examples. |
| `member-contracts` | Analyzer-backed member contracts including exact-one, exact-count, all-members, exclusive-choice, range, forbidden-member and unique-named-member samples. |
| `namespace-contracts` | Namespace-level contract examples. |
| `policy-variant-cases` | Attribute, code, JSON and composed policy variants. |
| `rule-filters` | Source, target and member-name filter examples. |
| `scenarios` | Scenario-oriented examples that combine lower-level Bricks features. |
| `source-target-cases` | Source/target shape examples for type, member, namespace, assembly and external references. |

## Sample Matrix By Consumer Question

| Consumer question | Current sample |
|---|---|
| How do I start with one tiny Bricks rule? | `analyzer-samples/dependency-rule-evaluation/` for the smallest analyzer-backed dependency-rule loop |
| How do I define my own role language? | `domain-language-kits/billing/roles/*`, `ddd-building/Ddd*Attribute.cs` |
| How do I express allowed/forbidden/required dependencies? | `domain-language-kits/billing/rules/BillingBrickRules.Assembly.cs`, `policy-variant-cases/*` |
| How do I limit rules with filters? | `rule-filters/*`, plus `tests/Samples.Block04.Bricks.Tests/RuleFilterCases/RuleFilterSampleTests.cs` |
| How do I publish a complete domain language kit? | `domain-language-kits/billing/*` |
| How do I model marker contracts? | `member-contracts/*-case/`, `ddd-building/DddIdentifierAttribute.cs` |
| How do I combine all of that? | `ddd-building/`, `domain-language-kits/billing/`, `function-coverage/` |

## Additional Future Sample Ideas

The following ideas remain useful, but they are not required for the current
100% Bricks roundtrip because each shipped public API family, enum variation
family and public enum value already has checked sample evidence. The shipped
member-contract families are analyzer-backed; only broader variants remain
future ideas.

### Range Contracts

- additional `AtLeastOneUseCase.cs`
- additional `AtLeastTwoUseCase.cs`
- additional `AtMostOneUseCase.cs`

### Choice Sets

- `ExactlyOneOfThreeUseCase.cs`
- `AtLeastOneOfThreeUseCase.cs`
- `AtLeastTwoOfFourUseCase.cs`

### Negative Contracts

- `NotTogetherUseCase.cs`
- `ForbiddenCombinationUseCase.cs`

### Conditional Contracts

- `IfAThenBUseCase.cs`
- `IfRoleXThenMarkerYUseCase.cs`
- `IfExternalApiThenRetryPolicyUseCase.cs`

### Rich Member Targets

- `MethodMarkerContracts.cs`
- `FieldMarkerContracts.cs`
- `ParameterMarkerContracts.cs`
- `MixedMemberTargetContracts.cs`

### Type System Edge Cases

- `InheritedMarkerContracts.cs`
- `GenericTypeContracts.cs`
- `NestedTypeContracts.cs`
- `PartialTypeContracts.cs`

### Delivery And Reuse

- `Contoso.BricksKit.csproj`
- `Contoso.BricksKit.SampleConsumer.csproj`
- `PackagingAndConsumption.md`

## Priority Order

The next recommended implementation order is:

1. `getting-started`
2. `role-modeling`
3. `dependency-rules`
4. `rule-filters`
5. finish the `member-contracts/use-cases` documentation split
6. `composition`
7. only then add aspirational patterns such as ranges, negation, and conditionals

## Explicit Capability Boundary

The Bricks block should always call out this distinction:

- `analyzer-backed`: the Roslyn integration raises diagnostics today
- `concept-only`: the sample is useful as a modeling blueprint, but is not yet enforced automatically

Without that distinction, consumers will overestimate the current automation level and
misread design samples as enforcement guarantees.
