using Brochure.ORM.Atrributes;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db index.
    /// </summary>
    public abstract class DbIndex : IAsyncDisposable, ITransaction
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbIndex"/> class.
        /// </summary>
        protected DbIndex(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        public IsolationLevel IsolationLevel => _dbContext.IsolationLevel;

        /// <summary>
        /// Commits the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public Task CommitAsync()
        {
            return _dbContext.CommitAsync();
        }

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="sqlIndex">The sql index.</param>
        /// <param name="databaseName"></param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> CreateIndexAsync(string tableName, string[] columnNames, string indexName, string sqlIndex, string databaseName = "")
        {
            var sql = Sql.CreateIndex(tableName, columnNames, indexName, sqlIndex, databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Deletes the index.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="databaseName"></param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> DeleteIndexAsync(string tableName, string indexName, string databaseName = "")
        {
            var sql = Sql.DeleteIndex(tableName, indexName, databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Disposes the async.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }

        /// <summary>
        /// Rollbacks the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public Task RollbackAsync()
        {
            return _dbContext.RollbackAsync();
        }
    }
}