using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;
using Samples.Block04.Bricks;
using Xunit;

namespace BrickExamples.Tests.MemberContractCases;

/// <summary>
/// Verifies that the member-contract use cases are complete for the
/// analyzer-backed contracts.
/// </summary>
public sealed class MemberContractUseCaseTests
{
    public static TheoryData<MemberContractUseCaseExpectation> AnalyzerBackedCases =>
        CreateTheoryData(MemberContractUseCaseCatalog.Cases.Where(useCase => useCase.IsAnalyzerBacked));

    public static TheoryData<Type, string, string[]> AnalyzerBackedViolationCases => new()
    {
        {
            typeof(OnlyOneMissingSample),
            "RequireExactlyOneMember",
            new[] { "OnlyOneMissingSample must declare exactly one member marked with OnlyOneMarkerAttribute." }
        },
        {
            typeof(OnlyOneTooManySample),
            "RequireExactlyOneMember",
            new[] { "OnlyOneTooManySample must declare exactly one member marked with OnlyOneMarkerAttribute." }
        },
        {
            typeof(ExactlyTwoTooFewSample),
            "RequireMemberCount",
            new[] { "ExactlyTwoTooFewSample must declare RepeatedMarkerAttribute count expected 2, actual 1." }
        },
        {
            typeof(ExactlyTwoTooManySample),
            "RequireMemberCount",
            new[] { "ExactlyTwoTooManySample must declare RepeatedMarkerAttribute count expected 2, actual 3." }
        },
        {
            typeof(AndAPlusBMissingAInvalidSample),
            "RequireAllMembers",
            new[] { "AndAPlusBMissingAInvalidSample must declare at least one member marked with MarkerAAttribute." }
        },
        {
            typeof(AndAPlusBMissingBInvalidSample),
            "RequireAllMembers",
            new[] { "AndAPlusBMissingBInvalidSample must declare at least one member marked with MarkerBAttribute." }
        },
        {
            typeof(AndAPlusBMissingBothInvalidSample),
            "RequireAllMembers",
            new[]
            {
                "AndAPlusBMissingBothInvalidSample must declare at least one member marked with MarkerAAttribute.",
                "AndAPlusBMissingBothInvalidSample must declare at least one member marked with MarkerBAttribute.",
            }
        },
        {
            typeof(XorBothInvalidSample),
            "RequireExclusiveChoice",
            new[] { "XorBothInvalidSample must declare exactly one of XorLeftMarkerAttribute or XorRightMarkerAttribute." }
        },
        {
            typeof(XorNoneInvalidSample),
            "RequireExclusiveChoice",
            new[] { "XorNoneInvalidSample must declare exactly one of XorLeftMarkerAttribute or XorRightMarkerAttribute." }
        },
        {
            typeof(TwoToFourTooFewSample),
            "RequireMemberRange",
            new[] { "TwoToFourTooFewSample must declare RangeMarkerAttribute count between 2 and 4, actual 1." }
        },
        {
            typeof(TwoToFourTooManySample),
            "RequireMemberRange",
            new[] { "TwoToFourTooManySample must declare RangeMarkerAttribute count between 2 and 4, actual 5." }
        },
        {
            typeof(NotInvalidSample),
            "ForbidMember",
            new[] { "NotInvalidSample must not declare members marked with ForbiddenMarkerAttribute; actual 1." }
        },
        {
            typeof(RequiredNamedIdentifierMissingYInvalidSample),
            "RequireNamedMembers",
            new[] { "RequiredNamedIdentifierMissingYInvalidSample must declare a RequiredNamedIdentifierAttribute member named 'Y'." }
        },
        {
            typeof(UniqueNamedIdentifierDuplicateNameInvalidSample),
            "RequireUniqueNamedMember",
            new[] { "UniqueNamedIdentifierDuplicateNameInvalidSample must declare at most one NamedIdentifierAttribute member named 'X'; actual 2." }
        },
        {
            typeof(UniqueNamedIdentifierDuplicateUnnamedInvalidSample),
            "RequireUniqueNamedMember",
            new[] { "UniqueNamedIdentifierDuplicateUnnamedInvalidSample must declare at most one NamedIdentifierAttribute member named <unnamed>; actual 2." }
        },
    };

    [Theory]
    [MemberData(nameof(AnalyzerBackedCases))]
    public void AnalyzerBackedMemberContractUseCasesMatchEvaluator(MemberContractUseCaseExpectation useCase)
    {
        var violations = Evaluate(useCase.SampleType);

        Assert.Equal(useCase.ExpectedAnalyzerViolationCount, violations.Count);
        Assert.Equal(useCase.ExpectedAnalyzerViolationCount == 0, useCase.ShouldBeValid);
        Assert.All(violations, violation =>
        {
            Assert.Equal(BrickViolationKind.MemberCardinality, violation.Kind);
            Assert.Equal(BrickSeverity.Error, violation.Severity);
            Assert.Equal(BrickViolationState.Active, violation.State);
            Assert.Equal(useCase.ContractName, violation.RuleName);
        });
    }

    [Theory]
    [MemberData(nameof(AnalyzerBackedViolationCases))]
    public void AnalyzerBackedMemberContractViolationCasesProduceConcreteViolations(
        Type sampleType,
        string expectedRuleName,
        string[] expectedMessages)
    {
        var violations = Evaluate(sampleType);

        Assert.Equal(expectedMessages.Length, violations.Count);
        Assert.All(violations, violation =>
        {
            Assert.Equal(BrickViolationKind.MemberCardinality, violation.Kind);
            Assert.Equal(BrickSeverity.Error, violation.Severity);
            Assert.Equal(BrickViolationState.Active, violation.State);
            Assert.Equal(expectedRuleName, violation.RuleName);
            Assert.Equal(sampleType.Name, violation.Source.DisplayName);
            Assert.Equal(sampleType.Assembly.GetName().Name, violation.Source.AssemblyName);
        });
        Assert.Equal(expectedMessages.OrderBy(message => message), violations.Select(violation => violation.Message).OrderBy(message => message));
    }

    [Fact]
    public void CatalogDocumentsAllMemberContractUseCasesAsAnalyzerBacked()
    {
        var conceptCases = MemberContractUseCaseCatalog.Cases.Where(useCase => !useCase.IsAnalyzerBacked).ToArray();
        var analyzerBackedNames = MemberContractUseCaseCatalog.Cases
            .Where(useCase => useCase.IsAnalyzerBacked)
            .Select(useCase => useCase.ContractName)
            .Distinct()
            .OrderBy(name => name)
            .ToArray();

        Assert.Empty(conceptCases);
        Assert.Equal(
            new[]
            {
                "ForbidMember",
                "RequireAllMembers",
                "RequireExactlyOneMember",
                "RequireExclusiveChoice",
                "RequireMemberCount",
                "RequireMemberRange",
                "RequireNamedMembers",
                "RequireUniqueNamedMember"
            },
            analyzerBackedNames);
    }

    [Fact]
    public void CatalogCoversEveryMemberContractBoundary()
    {
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(OnlyOneMissingSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(OnlyOneTooManySample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(ExactlyTwoTooFewSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(ExactlyTwoTooManySample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(AndAPlusBMissingAInvalidSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(AndAPlusBMissingBInvalidSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(AndAPlusBMissingBothInvalidSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(AndAPlusBDuplicateMarkersValidSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(XorBothInvalidSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(XorNoneInvalidSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(RequiredNamedIdentifierMissingYInvalidSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(UniqueNamedIdentifierDuplicateNameInvalidSample));
        Assert.Contains(MemberContractUseCaseCatalog.Cases, useCase => useCase.SampleType == typeof(UniqueNamedIdentifierDuplicateUnnamedInvalidSample));
    }

    [Fact]
    public void MemberContractUseCasesStaySplitByProject()
    {
        var assemblyByContract = MemberContractUseCaseCatalog.Cases
            .GroupBy(useCase => useCase.ContractName)
            .ToDictionary(
                group => group.Key,
                group => Assert.Single(group.Select(useCase => useCase.SampleType.Assembly.GetName().Name).Distinct()));

        Assert.Equal("Samples.Block04.Bricks.MemberContracts.OnlyOne", assemblyByContract["RequireExactlyOneMember"]);
        Assert.Equal("Samples.Block04.Bricks.MemberContracts.ExactlyTwo", assemblyByContract["RequireMemberCount"]);
        Assert.Equal("Samples.Block04.Bricks.MemberContracts.AllMembers", assemblyByContract["RequireAllMembers"]);
        Assert.Equal("Samples.Block04.Bricks.MemberContracts.ExclusiveChoice", assemblyByContract["RequireExclusiveChoice"]);
        Assert.Equal("Samples.Block04.Bricks.MemberContracts.Range", assemblyByContract["RequireMemberRange"]);
        Assert.Equal("Samples.Block04.Bricks.MemberContracts.Forbid", assemblyByContract["ForbidMember"]);
        Assert.Equal("Samples.Block04.Bricks.MemberContracts.RequiredNamed", assemblyByContract["RequireNamedMembers"]);
        Assert.Equal("Samples.Block04.Bricks.MemberContracts.UniqueNamed", assemblyByContract["RequireUniqueNamedMember"]);
        Assert.Equal(8, assemblyByContract.Values.Distinct().Count());
    }

    [Fact]
    public void EveryMemberContractProjectHasViolationCase()
    {
        var invalidCaseAssemblies = MemberContractUseCaseCatalog.Cases
            .Where(useCase => !useCase.ShouldBeValid)
            .Select(useCase => useCase.SampleType.Assembly.GetName().Name)
            .Distinct()
            .ToArray();

        Assert.Contains("Samples.Block04.Bricks.MemberContracts.OnlyOne", invalidCaseAssemblies);
        Assert.Contains("Samples.Block04.Bricks.MemberContracts.ExactlyTwo", invalidCaseAssemblies);
        Assert.Contains("Samples.Block04.Bricks.MemberContracts.AllMembers", invalidCaseAssemblies);
        Assert.Contains("Samples.Block04.Bricks.MemberContracts.ExclusiveChoice", invalidCaseAssemblies);
        Assert.Contains("Samples.Block04.Bricks.MemberContracts.Range", invalidCaseAssemblies);
        Assert.Contains("Samples.Block04.Bricks.MemberContracts.Forbid", invalidCaseAssemblies);
        Assert.Contains("Samples.Block04.Bricks.MemberContracts.RequiredNamed", invalidCaseAssemblies);
        Assert.Contains("Samples.Block04.Bricks.MemberContracts.UniqueNamed", invalidCaseAssemblies);
        Assert.Equal(8, invalidCaseAssemblies.Length);
    }

    private static TheoryData<MemberContractUseCaseExpectation> CreateTheoryData(IEnumerable<MemberContractUseCaseExpectation> useCases)
    {
        var data = new TheoryData<MemberContractUseCaseExpectation>();
        foreach (var useCase in useCases)
        {
            data.Add(useCase);
        }

        return data;
    }

    private static IReadOnlyList<BrickViolation> Evaluate(Type sampleType)
    {
        var element = new BrickElement(
            BrickElementId.From(sampleType.FullName ?? sampleType.Name),
            BrickElementKind.Type,
            sampleType.Name,
            sampleType.Assembly.GetName().Name,
            sampleType.Namespace,
            sampleType.FullName);

        return BrickMemberCardinalityEvaluator.Evaluate(
            element,
            GetMemberContracts(sampleType),
            CountMemberMarkers(sampleType),
            CountNamedMemberMarkers(sampleType));
    }

    private static IReadOnlyList<Attribute> GetMemberContracts(Type sampleType) =>
        sampleType.GetCustomAttributes(inherit: false)
            .SelectMany(attribute => attribute.GetType().GetCustomAttributes(inherit: false))
            .OfType<Attribute>()
            .Where(IsMemberContract)
            .ToArray();

    private static IReadOnlyDictionary<Type, int> CountMemberMarkers(Type sampleType)
    {
        var counts = new Dictionary<Type, int>();
        var members = sampleType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(member => member.DeclaringType == sampleType);

        foreach (var markerType in members.SelectMany(member => member.GetCustomAttributes(inherit: false)).Select(attribute => attribute.GetType()))
        {
            counts.TryGetValue(markerType, out var count);
            counts[markerType] = count + 1;
        }

        return counts;
    }

    private static IReadOnlyDictionary<Type, IReadOnlyDictionary<string, int>> CountNamedMemberMarkers(Type sampleType)
    {
        var counts = new Dictionary<Type, Dictionary<string, int>>();
        var members = sampleType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(member => member.DeclaringType == sampleType);

        foreach (var attribute in members.SelectMany(member => member.GetCustomAttributes(inherit: false)).OfType<Attribute>())
        {
            var markerType = attribute.GetType();
            var markerName = ReadMarkerName(attribute);
            if (!counts.TryGetValue(markerType, out var markerCounts))
            {
                markerCounts = new Dictionary<string, int>(StringComparer.Ordinal);
                counts[markerType] = markerCounts;
            }

            markerCounts.TryGetValue(markerName, out var count);
            markerCounts[markerName] = count + 1;
        }

        return counts.ToDictionary(
            pair => pair.Key,
            pair => (IReadOnlyDictionary<string, int>)pair.Value);
    }

    private static string ReadMarkerName(Attribute attribute)
    {
        var property = attribute.GetType().GetProperty("Name", BindingFlags.Instance | BindingFlags.Public);
        return property?.GetValue(attribute) as string ?? string.Empty;
    }

    private static bool IsMemberContract(Attribute attribute) =>
        attribute is RequireExactlyOneMemberAttribute
            or RequireMemberCountAttribute
            or RequireMemberRangeAttribute
            or RequireAllMembersAttribute
            or RequireExclusiveChoiceAttribute
            or ForbidMemberAttribute
            or RequireNamedMembersAttribute
            or RequireUniqueNamedMemberAttribute;
}
