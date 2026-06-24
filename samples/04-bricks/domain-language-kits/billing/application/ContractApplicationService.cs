namespace Samples.Block04.Bricks
{
    /// <summary>
    /// Application service that orchestrates the "open contract" use case.
    /// </summary>
    [BillingApplicationRole]
    public sealed class ContractApplicationService
    {
        private readonly IContractRepository _repository;
        private readonly ContractPolicy _policy;

        /// <summary>
        /// Initializes the service with required domain collaborators.
        /// </summary>
        /// <param name="repository">Repository contract for persistence actions.</param>
        /// <param name="policy">Domain policy for business validation.</param>
        public ContractApplicationService(IContractRepository repository, ContractPolicy policy)
        {
            _repository = repository;
            _policy = policy;
        }

        /// <summary>
        /// Opens a contract when policy checks are satisfied.
        /// </summary>
        /// <param name="customerName">Customer name used by policy and persistence flow.</param>
        /// <returns>
        /// <see langword="true"/> when the contract was accepted and stored; otherwise <see langword="false"/>.
        /// </returns>
        public bool Open(string customerName)
        {
            if (!_policy.CanCreateContract(customerName))
            {
                return false;
            }

            _repository.Store(customerName);
            return true;
        }
    }
}
