namespace Samples.Block04.Bricks
{
    /// <summary>
    /// Domain policy containing a business decision for contract creation.
    /// </summary>
    [BillingDomainRole]
    public sealed class ContractPolicy
    {
        /// <summary>
        /// Validates whether a contract can be created for the given customer name.
        /// </summary>
        /// <param name="customerName">Candidate customer display name.</param>
        /// <returns>
        /// <see langword="true"/> if a non-empty customer name is provided; otherwise <see langword="false"/>.
        /// </returns>
        public bool CanCreateContract(string customerName)
        {
            return !string.IsNullOrWhiteSpace(customerName);
        }
    }
}
