using Brochure.ORM.Atrributes;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db table.
    /// </summary>
    public abstract class DbTable : IAsyncDisposable, ITransaction
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        public IsolationLevel IsolationLevel => _dbContext.IsolationLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTable"/> class.
        /// </summary>
        protected DbTable(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates the table force async.
        /// </summary>
        /// <returns>A Task.</returns>
        [Transaction]
        public async Task<long> CreateTableForceAsync<T>(string databaseName = "")
        {
            var tableName = TableUtlis.GetTableName<T>();
            await DeleteTableAsync(tableName, databaseName);
            return await CreateTableAsync<T>();
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> CreateTableAsync<T>(string databaseName = "")
        {
            var sql = Sql.CreateTable<T>(databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Are the exist table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="databaseName"></param>
        /// <returns>A bool.</returns>
        public virtual async Task<bool> IsExistTableAsync(string tableName, string databaseName = "")
        {
            var sql = Sql.GetCountTable(tableName, databaseName);
            var rr = await _dbContext.ExecuteScalarAsync(sql);
            var r = (long)rr;
            return r >= 1;
        }

        /// <summary>
        /// Are the exist table.
        /// </summary>
        /// <returns>A bool.</returns>
        public virtual Task<bool> IsExistTableAsync<T>(string databaseName = "")
        {
            var tableName = TableUtlis.GetTableName<T>();
            return IsExistTableAsync(tableName, databaseName);
        }

        /// <summary>
        /// Deletes the table async.
        /// </summary>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<int> DeleteTableAsync<T>(string databaseName = "")
        {
            var tableName = TableUtlis.GetTableName<T>();
            return DeleteTableAsync(tableName, databaseName);
        }

        /// <summary>
        /// Deletes the table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="databaseName"></param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> DeleteTableAsync(string tableName, string databaseName = "")
        {
            var sql = Sql.DeleteTable(tableName, databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Updates the table name.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="newTableName">The new table name.</param>
        /// <param name="databaseName"></param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> UpdateTableNameAsync(string tableName, string newTableName, string databaseName = "")
        {
            var sql = Sql.UpdateTableName(tableName, newTableName, databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
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

        /// <summary>
        /// Disposes the async.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }
    }
}