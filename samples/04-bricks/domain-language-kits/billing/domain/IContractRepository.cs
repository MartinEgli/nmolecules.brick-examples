namespace Samples.Block04.Bricks
{
    /// <summary>
    /// Domain-side repository contract for contract persistence.
    /// </summary>
    [BillingDomainRole]
    public interface IContractRepository
    {
        /// <summary>
        /// Stores contract data for the given customer name.
        /// </summary>
        /// <param name="customerName">Customer name to store.</param>
        void Store(string customerName);
    }
}
