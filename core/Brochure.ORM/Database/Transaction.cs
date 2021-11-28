using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The transaction.
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        IsolationLevel IsolationLevel { get; }

        /// <summary>
        /// Commits the.
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rollbacks the.
        /// </summary>
        Task RollbackAsync();
    }

    /// <summary>
    /// The transaction.
    /// </summary>
    public class Transaction : ITransaction, IDisposable
    {
        private DbTransaction dbTransaction;
        private readonly IConnectFactory _connectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="connectFactory">The connect factory.</param>
        public Transaction(IConnectFactory connectFactory)
        {
            _connectFactory = connectFactory;
        }

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        public IsolationLevel IsolationLevel => dbTransaction.IsolationLevel;

        /// <summary>
        /// Commits the.
        /// </summary>
        public async Task CommitAsync()
        {
            if (dbTransaction != null)
                await dbTransaction.CommitAsync();
            Dispose();
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
            dbTransaction?.Dispose();
        }

        /// <summary>
        /// Gets the db transaction.
        /// </summary>
        /// <returns>An IDbTransaction.</returns>
        public async Task<DbTransaction> GetDbTransactionAsync()
        {
            var dbConnection = await _connectFactory.CreateAndOpenConnectionAsync();
            if (dbTransaction == null)
                dbTransaction = await dbConnection.BeginTransactionAsync().ConfigureAwait(false);
            return dbTransaction;
        }

        /// <summary>
        /// Rollbacks the.
        /// </summary>
        public async Task RollbackAsync()
        {
            await dbTransaction?.RollbackAsync();
            Dispose();
        }
    }

    /// <summary>
    /// The inner transaction.
    /// </summary>
    public class InnerTransaction : ITransaction, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InnerTransaction"/> class.
        /// </summary>
        /// <param name="dbOption">The db option.</param>
        public InnerTransaction()
        {
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
        public Task CommitAsync()
        {
            Dispose();
            return Task.CompletedTask;
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
        public async Task<DbTransaction> GetDbTransactionAsync()
        {
            return null;
        }

        /// <summary>
        /// Rollbacks the.
        /// </summary>
        public Task RollbackAsync()
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}