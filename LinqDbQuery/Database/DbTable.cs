using System.Threading.Tasks;
namespace LinqDbQuery.Database
{
    public abstract class DbTable
    {
        protected DbOption Option;
        private readonly DbSql dbSql;

        protected DbTable (DbOption option, DbSql dbSql)
        {
            Option = option;
            this.dbSql = dbSql;
        }

        public Task<long> CreateTableAsync<T> ()
        {
            return Task.Run (() => CreateTable<T> ());
        }

        public virtual long CreateTable<T> ()
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = dbSql.GetCreateTableSql<T> ();
            return command.ExecuteNonQuery ();
        }

        public Task<bool> IsExistTableAsync (string tableName)
        {
            return Task.Run (() => IsExistTable (tableName));
        }

        public virtual bool IsExistTable (string tableName)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = dbSql.GetTableNameCountSql (tableName);
            var r = (int) command.ExecuteScalar ();
            return r >= 1;
        }

        public Task<long> DeleteTableAsync (string tableName)
        {
            return Task.Run (() => DeleteTable (tableName));
        }

        public virtual long DeleteTable (string tableName)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = dbSql.GetDeleteTableSql (tableName);
            return command.ExecuteNonQuery ();
        }

        public Task<long> UpdateTableNameAsync (string tableName, string newTableName)
        {
            return Task.Run (() => UpdateTableName (tableName, newTableName));
        }

        public virtual long UpdateTableName (string tableName, string newTableName)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = dbSql.GetUpdateTableNameSql (tableName, newTableName);
            return command.ExecuteNonQuery ();
        }
    }
}