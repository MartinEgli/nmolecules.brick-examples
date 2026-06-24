using System.Collections.Generic;
using System.IO;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.FunctionCoverage;

/// <summary>
/// Demonstrates runtime-aware dependencies, adoption documents, normalized reports,
/// SARIF output and export documents for architecture tooling.
/// </summary>
internal static class RuntimeAndExportExamples
{
    public static IReadOnlyList<BrickViolation> EvaluateRuntimeBoundaries()
    {
        var domainAssembly = SampleBrickModel.Assembly("Billing.Domain");
        var testAssembly = SampleBrickModel.Assembly("Billing.Domain.Tests");
        var compositionRoot = SampleBrickModel.Type("Billing.Api.CompositionRoot");
        var pluginHost = SampleBrickModel.Type("Billing.Infrastructure.PluginHost");
        var reflectionActivator = SampleBrickModel.Type("Billing.Infrastructure.ReflectionActivator");
        var repositoryContract = SampleBrickModel.Type("Billing.Application.IContractRepository");
        var sqlRepository = SampleBrickModel.Type("Billing.Infrastructure.SqlContractRepository");
        var domainPolicy = SampleBrickModel.Type("Billing.Domain.ContractPolicy");

        var friendGrant = new BrickFriendAssemblyGrant(domainAssembly, testAssembly, "Tests inspect internal domain builders.");
        var registration = new BrickDependencyRegistration(
            compositionRoot,
            repositoryContract,
            sqlRepository,
            "Scoped",
            "Composition root owns infrastructure wiring.");
        var activation = new BrickRuntimeActivation(
            pluginHost,
            domainPolicy,
            "ActivatorUtilities.CreateInstance",
            "Plugin host loads policy extensions.");
        var reflection = new BrickReflectionAccess(
            reflectionActivator,
            domainPolicy,
            "Activator.CreateInstance",
            BrickReflectionConfidence.High,
            "Serializer needs deterministic constructor activation.");

        var roles = new Dictionary<BrickElementId, IEnumerable<RoleId>>
        {
            [testAssembly.Id] = new[] { RoleId.From("TestOnly") },
            [compositionRoot.Id] = new[] { RoleId.From("Infrastructure") },
            [pluginHost.Id] = new[] { RoleId.From("Infrastructure") }
        };

        return BrickVisibilityEvaluator.EvaluateFriendAssemblies(new[] { friendGrant }, roles)
            .Concat(BrickRuntimeWiringEvaluator.EvaluateRegistrations(new[] { registration }, roles, new BrickRuntimeWiringPolicy(requireJustification: true)))
            .Concat(BrickRuntimeActivationEvaluator.EvaluateActivations(new[] { activation }, roles))
            .Concat(BrickReflectionEvaluator.EvaluateAccesses(new[] { reflection }))
            .ToArray();
    }

    public static BrickReportDocument CreateAdoptionAwareReport()
    {
        var source = SampleBrickModel.Type("Billing.Application.ContractApplicationService");
        var target = SampleBrickModel.Type("Billing.Infrastructure.SqlContractRepository");
        var violation = SampleBrickModel.Violation(source, target);
        var suppression = new BrickSuppression(
            RuleId.From("XMoleculesBricks0001"),
            new BrickElementSelector(BrickElementKind.Type, "Billing.Application.*", "Billing"),
            "Temporary exception while repository contract is extracted.",
            "Billing Team",
            SampleBrickModel.GeneratedAt.AddDays(30));
        var baseline = new BrickBaselineEntry(
            RuleId.From("XMoleculesBricks0001"),
            "Billing.Application.*",
            "Billing.Infrastructure.*",
            "Accepted legacy debt for the first Bricks rollout.",
            "Architecture Board",
            SampleBrickModel.GeneratedAt.AddDays(90));
        var adoptionDocument = new BrickAdoptionDocument(
            SampleBrickModel.GeneratedAt,
            new[] { baseline },
            new[] { suppression });
        var projected = BrickViolationStateProjector.Project(
            new[] { violation },
            adoptionDocument.Suppressions,
            adoptionDocument.Baselines,
            SampleBrickModel.GeneratedAt);

        _ = BrickAdoptionDocumentValidator.Validate(adoptionDocument);
        _ = BrickAdoptionJsonSerializer.Serialize(adoptionDocument);

        return new BrickReportDocument(SampleBrickModel.GeneratedAt, projected);
    }

    public static IReadOnlyDictionary<string, string> SerializeReportsAndExports()
    {
        var source = SampleBrickModel.Type("Billing.Application.ContractApplicationService");
        var target = SampleBrickModel.Type("Billing.Infrastructure.SqlContractRepository");
        var dependency = SampleBrickModel.Dependency(source, target);
        var sourceRoles = SampleBrickModel.Resolved(source, "Billing.Application");
        var targetRoles = SampleBrickModel.Resolved(target, "Billing.Infrastructure");
        var report = new BrickReportDocument(
            SampleBrickModel.GeneratedAt,
            new[] { SampleBrickModel.Violation(source, target) });
        var roleMap = BrickRoleMapDocument.FromResolvedRoles(
            SampleBrickModel.GeneratedAt,
            new[] { sourceRoles, targetRoles });
        var graph = BrickDependencyGraphDocument.FromDependencies(
            SampleBrickModel.GeneratedAt,
            new[] { dependency });
        var trace = BrickResolutionTraceDocument.FromTraces(
            SampleBrickModel.GeneratedAt,
            new[]
            {
                new BrickResolutionTrace(
                    source,
                    sourceRoles.CandidateAssignments,
                    sourceRoles.EffectiveRoles,
                    new[] { "Direct attribute won." },
                    false)
            });

        _ = BrickExportDocumentValidator.Validate(roleMap);
        _ = BrickExportDocumentValidator.Validate(graph);
        _ = BrickExportDocumentValidator.Validate(trace);

        return new Dictionary<string, string>
        {
            ["report.json"] = BrickReportJsonSerializer.Serialize(report),
            ["report.sarif"] = BrickReportSarifSerializer.Serialize(report),
            ["role-map.json"] = BrickExportJsonSerializer.Serialize(roleMap),
            ["dependency-graph.json"] = BrickExportJsonSerializer.Serialize(graph),
            ["resolution-trace.json"] = BrickExportJsonSerializer.Serialize(trace)
        };
    }

    public static void SaveReportLikeCliWouldDo(string outputDirectory)
    {
        var report = CreateAdoptionAwareReport();
        var roleMap = BrickRoleMapDocument.FromResolvedRoles(SampleBrickModel.GeneratedAt, new[] { SampleBrickModel.Resolved(report.Violations[0].Source, "Billing.Application") });
        var graph = BrickDependencyGraphDocument.FromDependencies(SampleBrickModel.GeneratedAt, Enumerable.Empty<BrickDependency>());
        var trace = new BrickResolutionTraceDocument(SampleBrickModel.GeneratedAt, null);

        BrickJsonFile.SaveReport(Path.Combine(outputDirectory, "bricks-report.json"), report);
        BrickJsonFile.SaveRoleMap(Path.Combine(outputDirectory, "bricks-role-map.json"), roleMap);
        BrickJsonFile.SaveDependencyGraph(Path.Combine(outputDirectory, "bricks-dependency-graph.json"), graph);
        BrickJsonFile.SaveResolutionTrace(Path.Combine(outputDirectory, "bricks-resolution-trace.json"), trace);
    }
}
