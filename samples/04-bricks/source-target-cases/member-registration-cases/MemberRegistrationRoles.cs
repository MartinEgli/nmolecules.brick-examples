using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;

namespace Samples.Block04.Bricks.SourceTarget.MemberRegistrationCases;


/// <summary>Role identifiers used by the member and registration sample.</summary>
public static class MemberRegistrationRoles
{
    public const string CommandHandlerMember = "Sample.Member.CommandHandler";
    public const string InfrastructureType = "Sample.Type.Infrastructure";
    public const string CompositionRootRegistration = "Sample.Registration.CompositionRoot";
    public const string ExternalMessageBus = "Sample.External.MessageBus";
}
