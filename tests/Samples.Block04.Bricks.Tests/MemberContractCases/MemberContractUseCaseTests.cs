using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;
using Samples.Block04.Bricks;
using Xunit;

namespace BrickExamples.Tests.MemberContractCases;

/// <summary>
/// Verifies that the member-contract use cases are complete for the analyzer-backed
/// contracts and clearly separated from concept-only future contracts.
/// </summary>
public sealed class MemberContractUseCaseTests
{
    public static TheoryData<MemberContractUseCaseExpectation> AnalyzerBackedCases =>
        CreateTheoryData(MemberContractUseCaseCatalog.Cases.Where(useCase => useCase.IsAnalyzerBacked));

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

    [Fact]
    public void CatalogDocumentsConceptOnlyMemberContractUseCasesSeparately()
    {
        var conceptCases = MemberContractUseCaseCatalog.Cases
            .Where(useCase => !useCase.IsAnalyzerBacked)
            .ToArray();

        Assert.Equal(6, conceptCases.Length);
        Assert.Contains(conceptCases, useCase => useCase.ContractName == "RequireMemberRange");
        Assert.Contains(conceptCases, useCase => useCase.ContractName == "ForbidMember");
        Assert.All(conceptCases, useCase => Assert.Equal(-1, useCase.ExpectedAnalyzerViolationCount));
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

        return BrickMemberCardinalityEvaluator.Evaluate(element, GetMemberContracts(sampleType), CountMemberMarkers(sampleType));
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

    private static bool IsMemberContract(Attribute attribute) =>
        attribute is RequireExactlyOneMemberAttribute
            or RequireMemberCountAttribute
            or RequireAllMembersAttribute
            or RequireExclusiveChoiceAttribute;
}
