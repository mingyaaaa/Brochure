using Brochure.ORM.Atrributes;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db columns.
    /// </summary>
    public abstract class DbColumns : IAsyncDisposable, ITransaction
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        public IsolationLevel IsolationLevel => _dbContext.IsolationLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbColumns"/> class.
        /// </summary>
        /// <param name="dbContext"></param>
        protected DbColumns(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Are the exist column async.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="databaseName"></param>
        /// <returns>A Task.</returns>
        public Task<bool> IsExistColumnAsync<T>(string columnName, string databaseName = "")
        {
            var tableName = TableUtlis.GetTableName<T>();
            return IsExistColumnAsync(tableName, columnName, databaseName);
        }

        /// <summary>
        /// Are the exist column.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="databaseName"></param>
        /// <returns>A bool.</returns>
        public virtual async Task<bool> IsExistColumnAsync(string tableName, string columnName, string databaseName = "")
        {
            var sql = Sql.GetColumnsCount(tableName, columnName, databaseName);
            var r = await _dbContext.ExecuteScalarAsync(sql);
            var rr = (long)r;
            return rr >= 1;
        }

        /// <summary>
        /// Renames the column async.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="newcolumnName">The newcolumn name.</param>
        /// <param name="typeName">The type name.</param>
        /// <param name="isNotNull"></param>
        /// <param name="length"></param>
        /// <param name="databaseName"></param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<int> RenameColumnAsync<T>(string columnName, string newcolumnName, TypeCode typeName, bool isNotNull, int length, string databaseName = "")
        {
            var tableName = TableUtlis.GetTableName<T>();
            return RenameColumnAsync(tableName, columnName, newcolumnName, typeName, isNotNull, length, databaseName);
        }

        /// <summary>
        /// Renames the column.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="newcolumnName">The newcolumn name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull"></param>
        /// <param name="length"></param>
        /// <param name="databaseName"></param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> RenameColumnAsync(string tableName, string columnName, string newcolumnName, TypeCode typeCode, bool isNotNull, int length, string databaseName = "")
        {
            var sql = Sql.GetRenameColumnNameSql(tableName, columnName, newcolumnName, typeCode, length, databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Updates the column async.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length"></param>
        /// <param name="databaseName"></param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<int> UpdateColumnAsync<T>(string columnName, TypeCode typeCode, bool isNotNull, int length, string databaseName = "")
        {
            var tableName = TableUtlis.GetTableName<T>();
            return UpdateColumnAsync(tableName, columnName, typeCode, isNotNull, length, databaseName);
        }

        /// <summary>
        /// Updates the column.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length"></param>
        /// <param name="databaseName"></param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> UpdateColumnAsync(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length, string databaseName = "")
        {
            var sql = Sql.GetUpdateColumnSql(tableName, columnName, typeCode, isNotNull, length, databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Deletes the column async.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="databaseName"></param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<int> DeleteColumnAsync<T>(string columnName, string databaseName = "")
        {
            var tableName = TableUtlis.GetTableName<T>();
            return DeleteColumnAsync(tableName, columnName, databaseName);
        }

        /// <summary>
        /// Deletes the column.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="databaseName"></param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> DeleteColumnAsync(string tableName, string columnName, string databaseName = "")
        {
            var sql = Sql.GetDeleteColumnSql(tableName, columnName, databaseName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Adds the columns async.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length"></param>
        /// <param name="databaseName"></param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<int> AddColumnsAsync<T>(string columnName, TypeCode typeCode, bool isNotNull, int length, string databaseName = "")
        {
            var tableName = TableUtlis.GetTableName<T>();
            return AddColumnsAsync(tableName, columnName, typeCode, isNotNull, length, databaseName);
        }

        /// <summary>
        /// Adds the columns.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length"></param>
        /// <param name="databaseName"></param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> AddColumnsAsync(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length, string databaseName = "")
        {
            var sql = Sql.GetAddColumnSql(tableName, columnName, typeCode, isNotNull, length, databaseName);
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