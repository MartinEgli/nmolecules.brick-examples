using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.TypeCases;


/// <summary>Rule identifiers used by the type-level sample.</summary>
public static class TypeCaseRules
{
    public const string PolicyId = "SOURCE-TARGET-TYPE-POLICY";
    public const string EndpointRequiresApplication = "SOURCE-TARGET-TYPE-001";
    public const string ApplicationRequiresRepositoryContract = "SOURCE-TARGET-TYPE-002";
    public const string ValueObjectMustNotUseInfrastructure = "SOURCE-TARGET-TYPE-003";
}
