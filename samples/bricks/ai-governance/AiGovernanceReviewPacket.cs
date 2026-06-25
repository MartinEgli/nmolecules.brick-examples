using System;
using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;
using Samples.Block04.Bricks.DddBuilding;
using Samples.Block04.Bricks.FunctionCoverage;

namespace Samples.Block04.Bricks.AiGovernance;


/// <summary>
/// Review artifact passed from deterministic Bricks output to an architecture
/// owner, AI coding agent or analyzer maintainer.
/// </summary>
internal sealed class AiGovernanceReviewPacket
{
    public AiGovernanceReviewPacket(
        BrickAiTrustBoundary trustBoundary,
        BrickAiCommentDocument comments,
        BrickRuleProposal proposal,
        BrickGovernanceReport governance)
    {
        TrustBoundary = trustBoundary;
        Comments = comments;
        Proposal = proposal;
        Governance = governance;
    }

    public BrickAiTrustBoundary TrustBoundary { get; }
    public BrickAiCommentDocument Comments { get; }
    public BrickRuleProposal Proposal { get; }
    public BrickGovernanceReport Governance { get; }
}
