using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.PolicyVariants.CodePolicy;


public static class CodePolicyIds
{
    public const string Policy = "CODE-POLICY-CLOSED";
    public const string CommandMemberMustNotUseInfrastructure = "CODE-POLICY-001";
    public const string CompositionRootRequiresExternalQueue = "CODE-POLICY-002";
    public const string CompositionRootMayRegisterExternalQueue = "CODE-POLICY-003";
}
