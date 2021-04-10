using System.Threading.Tasks;
using Brochure.ORM.Atrributes;

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

        [Transaction]
        public Task<long> CreateTableAsync<T>()
        {
            return Task.Run(() => CreateTable<T>());
        }

        [Transaction]
        public virtual long CreateTable<T>()
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = dbSql.GetCreateTableSql<T>();
            return command.ExecuteNonQuery();
        }

        public Task<bool> IsExistTableAsync(string tableName)
        {
            return Task.Run(() => IsExistTable(tableName));
        }

        public virtual bool IsExistTable(string tableName)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = dbSql.GetTableNameCountSql(tableName);
            var r = (int)command.ExecuteScalar();
            return r >= 1;
        }

        [Transaction]
        public Task<long> DeleteTableAsync(string tableName)
        {
            return Task.Run(() => DeleteTable(tableName));
        }

        [Transaction]
        public virtual long DeleteTable(string tableName)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = dbSql.GetDeleteTableSql(tableName);
            return command.ExecuteNonQuery();
        }

        [Transaction]
        public Task<long> UpdateTableNameAsync(string tableName, string newTableName)
        {
            return Task.Run(() => UpdateTableName(tableName, newTableName));
        }

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