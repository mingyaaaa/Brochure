using System.Data;
namespace LinqDbQuery.Database
{
    public interface ITransaction
    {
        IsolationLevel IsolationLevel { get; }
        bool IsComplete { get; }

        IDbTransaction BegiTransaction ();

        void Commit ();
        void Rollback ();
    }

    public class Transaction : ITransaction
    {
        private readonly IDbTransaction dbTransaction;
        private readonly DbOption dbOption;

        public Transaction (DbOption dbOption)
        {
            this.dbOption = dbOption;
        }

        public bool IsComplete { get; set; }

        public IsolationLevel IsolationLevel => dbTransaction.IsolationLevel;

        public void Commit ()
        {
            dbTransaction.Commit ();
            IsComplete = true;
        }

        public void Rollback ()
        {
            dbTransaction.Rollback ();
            IsComplete = true;
        }
        public void Open (IDbConnection dbConnection)
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open ();
        }

        public IDbTransaction BegiTransaction ()
        {
            var connect = this.dbOption.GetDbConnection ();
            Open (connect);
            return connect.BeginTransaction (dbOption.TransactionLevel);
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

        public IDbTransaction BegiTransaction ()
        {
            return null;
        }

        public void Commit ()
        {
            IsComplete = true;
        }

        public void Rollback ()
        {
            IsComplete = true;
        }
    }
}