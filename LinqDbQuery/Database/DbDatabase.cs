using System.Threading.Tasks;
namespace LinqDbQuery.Database
{
    public abstract class DbDatabase
    {
        protected DbOption Option;
        private readonly DbSql _dbSql;

        protected DbDatabase (DbOption option, DbSql dbSql)
        {
            Option = option;
            this._dbSql = dbSql;
        }

        public Task<long> CreateDatabaseAsync (string databaseName)
        {
            return Task.Run<long> (() => CreateDatabase (databaseName));
        }

        public virtual long CreateDatabase (string databaseName)
        {
            var sql = _dbSql.GetCreateDatabaseSql (databaseName);
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = sql;
            return command.ExecuteNonQuery ();
        }

        public Task<long> DeleteDatabaseAsync (string databaseName)
        {
            return Task.Run<long> (() => DeleteDatabase (databaseName));
        }

        public virtual long DeleteDatabase (string databaseName)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = _dbSql.GetDeleteDatabaseSql (databaseName);
            return command.ExecuteNonQuery ();
        }

        public Task<bool> IsExistDataBaseAsync (string databaseName)
        {
            return Task.Run (() => IsExistDataBase (databaseName));
        }

        public virtual bool IsExistDataBase (string databaseName)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = _dbSql.GetDataBaseNameCountSql (databaseName);
            var rr = (int) command.ExecuteScalar ();
            return rr >= 1;
        }

        public virtual void ChangeDatabase (string databaseName)
        {
            var connection = Option.GetDbConnection ();
            connection.ChangeDatabase (databaseName);
        }
    }
}