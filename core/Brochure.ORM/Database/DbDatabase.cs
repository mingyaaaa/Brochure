using System;
using System.Data;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db database.
    /// </summary>
    public abstract class DbDatabase : ITransaction, IAsyncDisposable
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        public IsolationLevel IsolationLevel => _dbContext.IsolationLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDatabase"/> class.
        /// </summary>
        protected DbDatabase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A long.</returns>
        public virtual Task<int> CreateDatabaseAsync(string databaseName)
        {
            var sql = Sql.CreateDatabase(databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A long.</returns>
        public virtual Task<int> DeleteDatabaseAsync(string databaseName)
        {
            var sql = Sql.DeleteDatabase(databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Are the exist data base.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A bool.</returns>
        public virtual async Task<bool> IsExistDataBaseAsync(string databaseName)
        {
            var sql = Sql.DatabaseCount(databaseName);
            var r = await _dbContext.ExecuteScalarAsync(sql);
            var rr = (long)r;
            return rr >= 1;
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
        /// Commits the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public Task CommitAsync()
        {
            return _dbContext.CommitAsync();
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