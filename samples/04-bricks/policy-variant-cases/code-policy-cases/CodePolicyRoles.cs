using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.CodePolicy;


public static class CodePolicyRoles
{
    public const string CommandMember = "Code.Member.Command";
    public const string InfrastructureType = "Code.Type.Infrastructure";
    public const string CompositionRoot = "Code.Registration.CompositionRoot";
    public const string ExternalQueue = "Code.External.Queue";
    public const string UnclassifiedSource = "Code.Type.UnclassifiedSource";
    public const string DomainType = "Code.Type.Domain";
}
