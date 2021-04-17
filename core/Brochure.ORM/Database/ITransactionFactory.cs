namespace Brochure.ORM.Database
{
    public interface ITransactionFactory
    {
        ITransaction GetTransaction();
    }

    public class TransactionFactory : ITransactionFactory
    {
        private readonly ITransactionManager transactionManager;
        private readonly DbOption dbOption;
        private readonly IConnectFactory connectFactory;

        public TransactionFactory(ITransactionManager transactionManager, DbOption dbOption, IConnectFactory connectFactory)
        {
            this.transactionManager = transactionManager;
            this.dbOption = dbOption;
            this.connectFactory = connectFactory;
        }
        public ITransaction GetTransaction()
        {
            if (transactionManager.IsEmpty)
            {
                return new Transaction(connectFactory);
            }
            else
            {
                return new InnerTransaction(dbOption);
            }
        }
    }
}