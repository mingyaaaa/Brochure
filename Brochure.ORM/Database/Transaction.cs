using System.Data;
namespace LinqDbQuery.Database
{
    public interface ITransaction
    {
        IsolationLevel IsolationLevel { get; }
        bool IsComplete { get; }
        void Commit ();
        void Rollback ();

        IDbTransaction GetDbTransaction ();
    }

    public class Transaction : ITransaction
    {
        private readonly IDbTransaction dbTransaction;

        public Transaction (IDbTransaction dbTransaction)
        {
            this.dbTransaction = dbTransaction;
        }

        public bool IsComplete { get; set; }

        public IsolationLevel IsolationLevel => dbTransaction.IsolationLevel;

        public void Commit ()
        {
            dbTransaction.Commit ();
            IsComplete = true;
        }

        public IDbTransaction GetDbTransaction ()
        {
            return dbTransaction;
        }

        public void Rollback ()
        {
            dbTransaction.Rollback ();
            IsComplete = true;
        }
    }

    public class InnerTransaction : ITransaction
    {

        public InnerTransaction (DbOption dbOption)
        {
            IsolationLevel = dbOption.TransactionLevel;
        }

        public IsolationLevel IsolationLevel { get; }

        public bool IsComplete { get; set; }

        public void Commit ()
        {
            IsComplete = true;
        }

        public IDbTransaction GetDbTransaction ()
        {
            return null;
        }

        public void Rollback ()
        {
            IsComplete = true;
        }
    }

}