using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.TypeCases;


/// <summary>Role identifiers used by the type-level sample.</summary>
public static class TypeCaseRoles
{
    public const string Endpoint = "Sample.Type.Endpoint";
    public const string ApplicationService = "Sample.Type.ApplicationService";
    public const string RepositoryContract = "Sample.Type.RepositoryContract";
    public const string ValueObject = "Sample.Type.ValueObject";
    public const string InfrastructureAdapter = "Sample.Type.InfrastructureAdapter";
}
