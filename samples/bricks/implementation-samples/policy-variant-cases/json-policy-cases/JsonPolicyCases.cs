using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.JsonPolicy;


/// <summary>
/// Demonstrates a policy loaded from JSON with imports, aliases and external
/// assignments.
/// </summary>
public static class JsonPolicyCases
{
    public static JsonPolicyCaseResult Evaluate()
    {
        var document = BrickPolicyJsonSerializer.Deserialize(Json);
        var webNamespace = Element("namespace:Retail.Web", BrickElementKind.Namespace, "Retail.Web", "Retail.Web");
        var domainAssembly = Element("assembly:Retail.Domain", BrickElementKind.Assembly, "Retail.Domain", "Retail.Domain");
        var webAssembly = Element("assembly:Retail.Web", BrickElementKind.Assembly, "Retail.Web", "Retail.Web");
        var externalPackage = Element("external:Serilog", BrickElementKind.ExternalReference, "Serilog", "Serilog");

        var dependencies = new[]
        {
            Dependency(webNamespace, domainAssembly, BrickScope.Namespace),
            Dependency(webAssembly, externalPackage, BrickScope.Assembly)
        };
        var roles = new[]
        {
            Resolved(webNamespace, JsonPolicyRoles.WebNamespace),
            Resolved(domainAssembly, JsonPolicyRoles.DomainAssembly),
            Resolved(webAssembly, JsonPolicyRoles.WebAssembly),
            Resolved(externalPackage, JsonPolicyRoles.ExternalPackage)
        };

        return new JsonPolicyCaseResult(
            document,
            dependencies,
            roles,
            BrickRuleEvaluator.Evaluate(document.Policy, dependencies, roles));
    }

    public const string Json = """
        {
          "schema": "NMolecules.Bricks.Policy/1.0",
          "policy": {
            "id": "JSON-POLICY-RETAIL",
            "name": "Retail JSON policy",
            "defaultDecision": "Allow",
            "enforcement": "Analyze",
            "imports": [
              { "id": "JSON-PLATFORM-DEFAULTS", "mode": "Import" }
            ],
            "rules": [
              {
                "ruleId": "JSON-POLICY-001",
                "name": "Web namespace must not use domain assembly directly",
                "sourceRole": "Json.Namespace.Web",
                "targetRole": "Json.Assembly.Domain",
                "decision": "Deny",
                "scope": "Namespace",
                "severity": "Error",
                "priority": 20,
                "reason": "Web should go through application contracts."
              },
              {
                "ruleId": "JSON-POLICY-002",
                "name": "Web assembly may use logging package only when reviewed",
                "sourceRole": "Json.Assembly.Web",
                "targetRole": "Json.External.Package",
                "decision": "Deny",
                "scope": "Assembly",
                "severity": "Warning",
                "priority": 10,
                "reason": "External packages should be reviewed at assembly boundaries."
              }
            ],
            "combinationRules": [
              {
                "name": "Web assembly cannot be domain assembly",
                "leftRole": "Json.Assembly.Web",
                "rightRole": "Json.Assembly.Domain",
                "kind": "Incompatible",
                "reason": "A deployable web shell should not also own domain model responsibilities."
              }
            ],
            "externalAssignments": [
              {
                "selector": { "kind": "Namespace", "pattern": "Retail.Web", "assemblyName": "Retail.Web" },
                "roleId": "Json.Namespace.Web",
                "mode": "ExternalConfiguration",
                "source": "PolicyFile",
                "precedence": { "specificity": "Namespace", "authority": "External" },
                "behavior": "Apply",
                "reason": "Namespace role from JSON policy."
              }
            ],
            "aliases": [
              {
                "name": "RetailWebNamespace",
                "selector": { "kind": "Namespace", "pattern": "Retail.Web", "assemblyName": "Retail.Web" },
                "canonicalRoleId": "Json.Namespace.Web",
                "precedence": { "specificity": "Namespace", "authority": "Alias" },
                "behavior": "Apply",
                "reason": "Friendly JSON alias for the web namespace."
              }
            ]
          }
        }
        """;

    private static BrickDependency Dependency(BrickElement source, BrickElement target, BrickScope scope) =>
        new(
            source,
            target,
            BrickDependencyKindId.From(BrickDependencyKinds.TypeReference),
            scope,
            BrickDependencyLayer.Static,
            BrickDependencyStrength.Direct,
            BrickEvidenceLevel.AnalyzerInferred);

    private static BrickResolvedRoles Resolved(BrickElement element, string role)
    {
        var assignment = new BrickRoleAssignment(
            new BrickElementSelector(element.Kind, element.Id.Value, element.AssemblyName),
            RoleId.From(role),
            BrickAssignmentMode.ExternalConfiguration,
            BrickAssignmentSource.PolicyFile,
            new BrickAssignmentPrecedence(BrickAssignmentSpecificity.Element, BrickAssignmentAuthority.External),
            BrickAssignmentBehavior.Apply);

        return new BrickResolvedRoles(element, new[] { assignment }, new[] { assignment }, null, null);
    }

    private static BrickElement Element(string id, BrickElementKind kind, string displayName, string fullName) =>
        new(BrickElementId.From(id), kind, displayName, displayName, fullName, fullName);
}
