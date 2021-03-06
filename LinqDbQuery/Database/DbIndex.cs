using System.Threading.Tasks;
using LinqDbQuery.Atrributes;

namespace LinqDbQuery.Database
{
    public abstract class DbIndex
    {
        protected DbOption Option;
        private readonly DbSql _dbSql;

        protected DbIndex (DbOption option, DbSql dbSql)
        {
            Option = option;
            this._dbSql = dbSql;
        }

        [Transaction]
        public Task<long> CreateIndexAsync (string tableName, string[] columnNames, string indexName, string sqlIndex)
        {
            return Task.Run (() => CreateIndex (tableName, columnNames, indexName, sqlIndex));
        }

        [Transaction]
        public virtual long CreateIndex (string tableName, string[] columnNames, string indexName, string sqlIndex)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = _dbSql.GetCreateIndexSql (tableName, columnNames, indexName, sqlIndex);
            return command.ExecuteNonQuery ();
        }

        [Transaction]
        public Task<long> DeleteIndexAsync (string tableName, string indexName)
        {
            return Task.Run (() => DeleteIndex (tableName, indexName));
        }

        [Transaction]
        public virtual long DeleteIndex (string tableName, string indexName)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = _dbSql.GetDeleteIndexSql (tableName, indexName);
            return command.ExecuteNonQuery ();
        }
    }
}