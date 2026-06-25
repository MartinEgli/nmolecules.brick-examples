using System;
using System.Collections.Generic;
using System.Linq;
using NMolecules.Bricks;
using Samples.Block04.Bricks.DddBuilding;
using Samples.Block04.Bricks.FunctionCoverage;

namespace Samples.Block04.Bricks.AiGovernance;


/// <summary>
/// Review packet for AI guidance that is derived from assembly-level policy,
/// rule and dependency attributes.
/// </summary>
internal sealed class AiAttributePolicyReviewPacket
{
    public AiAttributePolicyReviewPacket(
        string policyId,
        IEnumerable<string> ruleIds,
        IEnumerable<string> dependencyIds,
        BrickAiCommentDocument comments,
        BrickRuleProposal proposal,
        BrickGovernanceReport governance)
    {
        PolicyId = policyId ?? string.Empty;
        RuleIds = (ruleIds ?? Enumerable.Empty<string>()).ToArray();
        DependencyIds = (dependencyIds ?? Enumerable.Empty<string>()).ToArray();
        Comments = comments;
        Proposal = proposal;
        Governance = governance;
    }

    public string PolicyId { get; }
    public IReadOnlyList<string> RuleIds { get; }
    public IReadOnlyList<string> DependencyIds { get; }
    public BrickAiCommentDocument Comments { get; }
    public BrickRuleProposal Proposal { get; }
    public BrickGovernanceReport Governance { get; }
}
