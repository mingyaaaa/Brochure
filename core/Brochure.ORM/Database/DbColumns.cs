using System;
using System.Threading.Tasks;
using Brochure.ORM.Atrributes;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db columns.
    /// </summary>
    public abstract class DbColumns
    {
        protected DbOption Option;
        private readonly DbSql _dbSql;
        private readonly IConnectFactory connectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbColumns"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="connectFactory">The connect factory.</param>
        protected DbColumns(DbOption option, DbSql dbSql, IConnectFactory connectFactory)
        {
            Option = option;
            this._dbSql = dbSql;
            this.connectFactory = connectFactory;
        }

        /// <summary>
        /// Are the exist column async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A Task.</returns>
        public Task<bool> IsExistColumnAsync(string tableName, string columnName)
        {
            return Task.Run(() => IsExistColumn(tableName, columnName));
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
        public virtual bool IsExistColumn(string tableName, string columnName)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = _dbSql.GetColumsNameCountSql(Option.DatabaseName, tableName, columnName).SQL;
            var r = (int)command.ExecuteScalar();
            return r >= 1;
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
        public Task<long> RenameColumnAsync(string tableName, string columnName, string newcolumnName, TypeCode typeName)
        {
            return Task.Run(() => RenameColumn(tableName, columnName, newcolumnName, typeName));
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
        public Task<long> RenameColumnAsync<T>(string columnName, string newcolumnName, TypeCode typeName)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return Task.Run(() => RenameColumn(tableName, columnName, newcolumnName, typeName));
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
        public virtual long RenameColumn(string tableName, string columnName, string newcolumnName, TypeCode typeCode)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = _dbSql.GetRenameColumnNameSql(tableName, columnName, newcolumnName, typeCode).SQL;
            return command.ExecuteNonQuery();
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
        public Task<long> UpdateColumnAsync(string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            return Task.Run(() => UpdateColumn(tableName, columnName, typeCode, isNotNull));
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
        public Task<long> UpdateColumnAsync<T>(string columnName, TypeCode typeCode, bool isNotNull)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return Task.Run(() => UpdateColumn(tableName, columnName, typeCode, isNotNull));
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
        public virtual long UpdateColumn(string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = _dbSql.GetUpdateColumnSql(tableName, columnName, typeCode, isNotNull).SQL;
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes the column async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A Task.</returns>
        public Task<long> DeleteColumnAsync(string tableName, string columnName)
        {
            return Task.Run(() => DeleteColumn(tableName, columnName));
        }

        /// <summary>
        /// Deletes the column async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<long> DeleteColumnAsync<T>(string columnName)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return Task.Run(() => DeleteColumn(tableName, columnName));
        }

        /// <summary>
        /// Deletes the column.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A long.</returns>
        public virtual long DeleteColumn(string tableName, string columnName)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = _dbSql.GetDeleteColumnSql(tableName, columnName).SQL;
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds the columns async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <returns>A Task.</returns>
        public Task<long> AddColumnsAsync(string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            return Task.Run(() => AddColumns(tableName, columnName, typeCode, isNotNull));
        }

        /// <summary>
        /// Adds the columns async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <returns>A Task.</returns>
        public Task<long> AddColumnsAsync<T>(string columnName, TypeCode typeCode, bool isNotNull)
        {
            var tableName = TableUtlis.GetTableName<T>();
            return Task.Run(() => AddColumns(tableName, columnName, typeCode, isNotNull));
        }

        /// <summary>
        /// Adds the columns.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <returns>A long.</returns>
        public virtual long AddColumns(string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = _dbSql.GetAddllColumnSql(tableName, columnName, typeCode, isNotNull).SQL;
            return command.ExecuteNonQuery();
        }
    }
}