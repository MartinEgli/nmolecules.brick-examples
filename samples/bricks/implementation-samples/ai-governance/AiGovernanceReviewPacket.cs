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
        BrickRuleProposalQueue proposalQueue,
        string markdownReview,
        BrickRuleProposalReviewResult reviewedPromotion,
        BrickGovernanceReport governance)
    {
        TrustBoundary = trustBoundary;
        Comments = comments;
        Proposal = proposal;
        ProposalQueue = proposalQueue;
        MarkdownReview = markdownReview ?? string.Empty;
        ReviewedPromotion = reviewedPromotion;
        Governance = governance;
    }

    public BrickAiTrustBoundary TrustBoundary { get; }
    public BrickAiCommentDocument Comments { get; }
    public BrickRuleProposal Proposal { get; }
    public BrickRuleProposalQueue ProposalQueue { get; }
    public string MarkdownReview { get; }
    public BrickRuleProposalReviewResult ReviewedPromotion { get; }
    public BrickGovernanceReport Governance { get; }
}
