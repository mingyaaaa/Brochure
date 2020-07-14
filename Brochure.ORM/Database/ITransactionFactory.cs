namespace Brochure.ORM.Database
{
    public interface ITransactionFactory
    {
        ITransaction GetTransaction ();
    }

    public class TransactionFactory : ITransactionFactory
    {
        private readonly ITransactionManager transactionManager;
        private readonly DbOption dbOption;

        public TransactionFactory (ITransactionManager transactionManager, DbOption dbOption)
        {
            this.transactionManager = transactionManager;
            this.dbOption = dbOption;
        }
        public ITransaction GetTransaction ()
        {
            if (transactionManager.IsEmpty)
            {
                var connect = dbOption.GetDbConnection ();
                connect.Open ();
                var transaction = connect.BeginTransaction ();
                return new Transaction (transaction);
            }
            else
            {
                return new InnerTransaction (dbOption);
            }
        }
    }
}