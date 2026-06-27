using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases;


/// <summary>Rule identifiers used by the member and registration sample.</summary>
public static class MemberRegistrationRules
{
    public const string PolicyId = "SOURCE-TARGET-MEMBER-REGISTRATION-POLICY";
    public const string CommandHandlerMustNotCallInfrastructure = "SOURCE-TARGET-MEMBER-001";
    public const string CompositionRootRequiresExternalMessageBus = "SOURCE-TARGET-REGISTRATION-001";
}
