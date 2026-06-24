namespace Samples.Block04.Bricks
{
    using NMolecules.Bricks;

    /// <summary>
    /// Central catalog of rule identifiers used by the billing brick model.
    /// </summary>
    public static class BillingRules
    {
        /// <summary>
        /// Rule id: domain role must not depend on infrastructure role.
        /// </summary>
        public const string DomainMustNotDependOnInfrastructure = "BILL-BRICK-001";

        /// <summary>
        /// Rule id: same dependency rule with explicit excluded source-name token example.
        /// </summary>
        public const string DomainMustNotDependOnInfrastructureExceptLegacySource = "BILL-BRICK-002";

        /// <summary>
        /// Rule id: same dependency rule with explicit excluded target-name token example.
        /// </summary>
        public const string DomainMustNotDependOnInfrastructureExceptFacadeTarget = "BILL-BRICK-003";

        /// <summary>
        /// Rule id: same dependency rule with explicit excluded member-name token example.
        /// </summary>
        public const string DomainMustNotDependOnInfrastructureExceptAllowMembers = "BILL-BRICK-004";

        /// <summary>
        /// Rule id: same dependency rule with required source-name token example.
        /// </summary>
        public const string DomainMustNotDependOnInfrastructureForContractSources = "BILL-BRICK-005";

        /// <summary>
        /// Rule id: same dependency rule with required target-name token example.
        /// </summary>
        public const string DomainMustNotDependOnInfrastructureForRepositoryTargets = "BILL-BRICK-006";

        /// <summary>
        /// Typed wrapper for the baseline domain-to-infrastructure dependency rule identifier.
        /// </summary>
        public static RuleId DomainMustNotDependOnInfrastructureId =>
            RuleId.From(DomainMustNotDependOnInfrastructure);

        /// <summary>
        /// Typed wrapper for the legacy-source exclusion example rule identifier.
        /// </summary>
        public static RuleId DomainMustNotDependOnInfrastructureExceptLegacySourceId =>
            RuleId.From(DomainMustNotDependOnInfrastructureExceptLegacySource);

        /// <summary>
        /// Typed wrapper for the facade-target exclusion example rule identifier.
        /// </summary>
        public static RuleId DomainMustNotDependOnInfrastructureExceptFacadeTargetId =>
            RuleId.From(DomainMustNotDependOnInfrastructureExceptFacadeTarget);

        /// <summary>
        /// Typed wrapper for the member exclusion example rule identifier.
        /// </summary>
        public static RuleId DomainMustNotDependOnInfrastructureExceptAllowMembersId =>
            RuleId.From(DomainMustNotDependOnInfrastructureExceptAllowMembers);

        /// <summary>
        /// Typed wrapper for the required source-name token example rule identifier.
        /// </summary>
        public static RuleId DomainMustNotDependOnInfrastructureForContractSourcesId =>
            RuleId.From(DomainMustNotDependOnInfrastructureForContractSources);

        /// <summary>
        /// Typed wrapper for the required target-name token example rule identifier.
        /// </summary>
        public static RuleId DomainMustNotDependOnInfrastructureForRepositoryTargetsId =>
            RuleId.From(DomainMustNotDependOnInfrastructureForRepositoryTargets);
    }
}
