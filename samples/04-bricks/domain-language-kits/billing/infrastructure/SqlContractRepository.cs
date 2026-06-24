namespace Samples.Block04.Bricks
{
    /// <summary>
    /// Infrastructure implementation of <see cref="IContractRepository"/>.
    /// </summary>
    [BillingInfrastructureRole]
    public sealed class SqlContractRepository : IContractRepository
    {
        /// <summary>
        /// Stores contract data using an infrastructure-specific persistence implementation.
        /// </summary>
        /// <param name="customerName">Customer name to store.</param>
        public void Store(string customerName)
        {
        }
    }
}
