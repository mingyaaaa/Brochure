namespace Brochure.ORM.Database
{
    /// <summary>
    /// The transaction factory.
    /// </summary>
    public interface ITransactionFactory
    {
        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <returns>An ITransaction.</returns>
        ITransaction GetTransaction();
    }

    /// <summary>
    /// The transaction factory.
    /// </summary>
    public class TransactionFactory : ITransactionFactory
    {
        private readonly ITransactionManager transactionManager;
        private readonly IConnectFactory connectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionFactory"/> class.
        /// </summary>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="connectFactory">The connect factory.</param>
        public TransactionFactory(ITransactionManager transactionManager, IConnectFactory connectFactory)
        {
            this.transactionManager = transactionManager;
            this.connectFactory = connectFactory;
        }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <returns>An ITransaction.</returns>
        public ITransaction GetTransaction()
        {
            if (transactionManager.IsEmpty)
            {
                return new Transaction(connectFactory);
            }
            else
            {
                return new InnerTransaction();
            }
        }
    }
}