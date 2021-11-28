using Brochure.ORM.Atrributes;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db table.
    /// </summary>
    public abstract class DbTable
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTable"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="connectFactory">The connect factory.</param>
        protected DbTable(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Tries the create table async.
        /// </summary>
        /// <returns>A Task.</returns>
        [Transaction]
        public async Task<long> TryCreateTableAsync<T>(string databaseName)
        {
            var tableName = TableUtlis.GetTableName<T>();
            var isExist = await IsExistTableAsync(tableName, databaseName);
            if (!isExist)
                return await CreateTableAsync<T>();
            return -1;
        }

        /// <summary>
        /// Creates the table force async.
        /// </summary>
        /// <returns>A Task.</returns>
        [Transaction]
        public async Task<long> CreateTableForceAsync<T>()
        {
            var tableName = TableUtlis.GetTableName<T>();
            await DeleteTableAsync(tableName);
            return await CreateTableAsync<T>();
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> CreateTableAsync<T>()
        {
            var sql = Sql.CreateTable<T>();
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Are the exist table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A bool.</returns>
        protected virtual async Task<bool> IsExistTableAsync(string tableName, string databaseName)
        {
            var sql = Sql.GetCountTable(tableName, databaseName);
            var rr = await _dbContext.ExecuteScalarAsync(sql);
            var r = (int)rr;
            return r >= 1;
        }

        /// <summary>
        /// Are the exist table.
        /// </summary>
        /// <returns>A bool.</returns>
        public virtual Task<bool> IsExistTableAsync<T>(string databaseName)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return IsExistTableAsync(tableName, databaseName);
        }

        /// <summary>
        /// Deletes the table async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<int> DeleteTableAsync<T>()
        {
            var tableName = TableUtlis.GetTableName<T>();
            return DeleteTableAsync(tableName);
        }

        /// <summary>
        /// Deletes the table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> DeleteTableAsync(string tableName)
        {
            var sql = Sql.DeleteTable(tableName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Updates the table name async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="newTableName">The new table name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public async Task<int> TryUpdateTableNameAsync(string tableName, string newTableName, string databaseName)
        {
            var isExist = await IsExistTableAsync(tableName, databaseName);
            if (!isExist)
                return -1;
            return await UpdateTableNameAsync(tableName, newTableName);
        }

        /// <summary>
        /// Updates the table name.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="newTableName">The new table name.</param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> UpdateTableNameAsync(string tableName, string newTableName)
        {
            var sql = Sql.UpdateTableName(tableName, newTableName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }
    }
}