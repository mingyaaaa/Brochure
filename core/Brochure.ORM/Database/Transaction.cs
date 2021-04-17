using System;
using System.Data;
namespace Brochure.ORM.Database
{
    /// <summary>
    /// The transaction.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        IsolationLevel IsolationLevel { get; }
        /// <summary>
        /// Gets a value indicating whether is complete.
        /// </summary>
        bool IsComplete { get; }
        /// <summary>
        /// Commits the.
        /// </summary>
        void Commit();
        /// <summary>
        /// Rollbacks the.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Gets the db transaction.
        /// </summary>
        /// <returns>An IDbTransaction.</returns>
        IDbTransaction GetDbTransaction();
    }

    /// <summary>
    /// The transaction.
    /// </summary>
    public class Transaction : ITransaction
    {
        private readonly IDbTransaction dbTransaction;
        private readonly IConnectFactory _connectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="connectFactory">The connect factory.</param>
        public Transaction(IConnectFactory connectFactory)
        {
            var dbConnection = connectFactory.CreateAndOpenConnection();
            this.dbTransaction = dbConnection.BeginTransaction();
            _connectFactory = connectFactory;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is complete.
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        public IsolationLevel IsolationLevel => dbTransaction.IsolationLevel;

        /// <summary>
        /// Commits the.
        /// </summary>
        public void Commit()
        {
            dbTransaction.Commit();
            Dispose();
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
            _connectFactory.Dispose();
            IsComplete = true;
        }

        /// <summary>
        /// Gets the db transaction.
        /// </summary>
        /// <returns>An IDbTransaction.</returns>
        public IDbTransaction GetDbTransaction()
        {
            return dbTransaction;
        }

        /// <summary>
        /// Rollbacks the.
        /// </summary>
        public void Rollback()
        {
            dbTransaction.Rollback();
            Dispose();
        }
    }

    /// <summary>
    /// The inner transaction.
    /// </summary>
    public class InnerTransaction : ITransaction
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerTransaction"/> class.
        /// </summary>
        /// <param name="dbOption">The db option.</param>
        public InnerTransaction(DbOption dbOption)
        {
            IsolationLevel = dbOption.TransactionLevel;
        }

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        public IsolationLevel IsolationLevel { get; }

        /// <summary>
        /// Gets or sets a value indicating whether is complete.
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Commits the.
        /// </summary>
        public void Commit()
        {
            Dispose();
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
            IsComplete = true;
        }

        /// <summary>
        /// Gets the db transaction.
        /// </summary>
        /// <returns>An IDbTransaction.</returns>
        public IDbTransaction GetDbTransaction()
        {
            return null;
        }

        /// <summary>
        /// Rollbacks the.
        /// </summary>
        public void Rollback()
        {
            Dispose();
        }
    }

}