using System;
using System.Data;
using System.Threading.Tasks;
using Brochure.ORM.Atrributes;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db columns.
    /// </summary>
    public abstract class DbColumns : IAsyncDisposable, ITransaction
    {
        private readonly DbContext _dbContext;

        public IsolationLevel IsolationLevel => _dbContext.IsolationLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbColumns"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="connectFactory">The connect factory.</param>
        protected DbColumns(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Are the exist column async.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <returns>A Task.</returns>
        public Task<bool> IsExistColumnAsync<T>(string columnName)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return IsExistColumnAsync(tableName, columnName);
        }

        /// <summary>
        /// Are the exist column.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A bool.</returns>
        public virtual async Task<bool> IsExistColumnAsync(string tableName, string columnName)
        {
            var sql = Sql.GetColumnsCount(tableName, columnName);
            var r = await _dbContext.ExecuteScalarAsync(sql);
            var rr = (int)r;
            return rr >= 1;
        }

        /// <summary>
        /// Renames the column async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="newcolumnName">The newcolumn name.</param>
        /// <param name="typeName">The type name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<int> RenameColumnAsync<T>(string columnName, string newcolumnName, TypeCode typeName)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return RenameColumnAsync(tableName, columnName, newcolumnName, typeName);
        }

        /// <summary>
        /// Renames the column.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="newcolumnName">The newcolumn name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> RenameColumnAsync(string tableName, string columnName, string newcolumnName, TypeCode typeCode)
        {
            var sql = Sql.GetRenameColumnNameSql(tableName, columnName, newcolumnName, typeCode);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Updates the column async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<int> UpdateColumnAsync<T>(string columnName, TypeCode typeCode, bool isNotNull)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return UpdateColumnAsync(tableName, columnName, typeCode, isNotNull);
        }

        /// <summary>
        /// Updates the column.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> UpdateColumnAsync(string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            var sql = Sql.GetUpdateColumnSql(tableName, columnName, typeCode, isNotNull);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Deletes the column async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<int> DeleteColumnAsync<T>(string columnName)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return DeleteColumnAsync(tableName, columnName);
        }

        /// <summary>
        /// Deletes the column.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A long.</returns>
        public virtual Task<int> DeleteColumnAsync(string tableName, string columnName)
        {
            var sql = Sql.GetDeleteColumnSql(tableName, columnName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Adds the columns async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <returns>A Task.</returns>
        public Task<int> AddColumnsAsync<T>(string columnName, TypeCode typeCode, bool isNotNull)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return AddColumnsAsync(tableName, columnName, typeCode, isNotNull);
        }

        /// <summary>
        /// Adds the columns.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <returns>A long.</returns>
        public virtual Task<int> AddColumnsAsync(string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            var sql = Sql.GetAddColumnSql(tableName, columnName, typeCode, isNotNull);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        public Task CommitAsync()
        {
            return _dbContext.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return _dbContext.RollbackAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }
    }
}