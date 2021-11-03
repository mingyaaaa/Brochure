using Brochure.ORM.Atrributes;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    public abstract class DbTable
    {
        protected DbOption Option;
        private readonly DbSql dbSql;
        private readonly IConnectFactory connectFactory;

        protected DbTable(DbOption option, DbSql dbSql, IConnectFactory connectFactory)
        {
            Option = option;
            this.dbSql = dbSql;
            this.connectFactory = connectFactory;
        }

        /// <summary>
        /// Creates the table async.
        /// </summary>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<long> CreateTableAsync<T>()
        {
            return Task.Run(() => CreateTable<T>());
        }

        /// <summary>
        /// Tries the create table async.
        /// </summary>
        /// <returns>A Task.</returns>
        [Transaction]
        public async Task<long> TryCreateTableAsync<T>()
        {
            var tableName = TableUtlis.GetTableName<T>();
            var isExist = await IsExistTableAsync(tableName);
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
        public virtual long CreateTable<T>()
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = dbSql.GetCreateTableSql<T>();
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Are the exist table async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A Task.</returns>
        public Task<bool> IsExistTableAsync(string tableName)
        {
            return Task.Run(() => IsExistTable(tableName));
        }

        /// <summary>
        /// Are the exist table async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A Task.</returns>
        public Task<bool> IsExistTableAsync<T>()
        {
            return Task.Run(() => IsExistTable<T>());
        }

        /// <summary>
        /// Are the exist table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A bool.</returns>
        protected virtual bool IsExistTable(string tableName)
        {
            var connection = connectFactory.CreateAndOpenConnection();
            var command = connection.CreateCommand();
            command.CommandText = dbSql.GetTableNameCountSql(tableName);
            var r = (long)(command.ExecuteScalar());
            return r >= 1;
        }

        /// <summary>
        /// Are the exist table.
        /// </summary>
        /// <returns>A bool.</returns>
        public virtual bool IsExistTable<T>()
        {
            var tableName = TableUtlis.GetTableName<T>();
            return IsExistTable(tableName);
        }

        /// <summary>
        /// Deletes the table async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<long> DeleteTableAsync(string tableName)
        {
            return Task.Run(() => DeleteTable(tableName));
        }


        /// <summary>
        /// Deletes the table async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<long> DeleteTableAsync<T>()
        {
            var tableName = TableUtlis.GetTableName<T>();
            return Task.Run(() => DeleteTable(tableName));
        }

        /// <summary>
        /// Deletes the table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual long DeleteTable(string tableName)
        {
            var connection = connectFactory.CreateAndOpenConnection();
            var command = connection.CreateCommand();
            command.CommandText = dbSql.GetDeleteTableSql(tableName);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the table name async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="newTableName">The new table name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<long> UpdateTableNameAsync(string tableName, string newTableName)
        {
            return Task.Run(() => UpdateTableName(tableName, newTableName));
        }


        /// <summary>
        /// Updates the table name async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="newTableName">The new table name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public async Task<long> TryUpdateTableNameAsync(string tableName, string newTableName)
        {
            var isExist = await IsExistTableAsync(tableName);
            if (!isExist)
                return -1;
            return await Task.Run(() => UpdateTableName(tableName, newTableName));
        }

        /// <summary>
        /// Updates the table name.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="newTableName">The new table name.</param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual long UpdateTableName(string tableName, string newTableName)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = dbSql.GetUpdateTableNameSql(tableName, newTableName);
            return command.ExecuteNonQuery();
        }
    }
}