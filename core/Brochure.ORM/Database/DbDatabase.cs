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

        public IsolationLevel IsolationLevel => _dbContext.IsolationLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDatabase"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="connectFactory">The connect factory.</param>
        protected DbDatabase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Tries the create database async.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public async Task<int> TryCreateDatabaseAsync(string databaseName)
        {
            var exist = await IsExistDataBaseAsync(databaseName);
            if (!exist)
                return await CreateDatabaseAsync(databaseName);
            return -1;
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
        /// Tries the delete database async.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public async Task<int> TryDeleteDatabaseAsync(string databaseName)
        {
            var exist = await IsExistDataBaseAsync(databaseName);
            if (exist)
                return await DeleteDatabaseAsync(databaseName);
            return -1;
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

        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }

        public Task CommitAsync()
        {
            return _dbContext.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return _dbContext.RollbackAsync();
        }
    }
}