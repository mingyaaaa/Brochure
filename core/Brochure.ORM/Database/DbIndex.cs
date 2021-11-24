using System.Threading.Tasks;
using Brochure.ORM.Atrributes;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db index.
    /// </summary>
    public abstract class DbIndex
    {
        protected DbOption Option;
        private readonly DbSql _dbSql;
        private readonly IConnectFactory connectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbIndex"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="connectFactory">The connect factory.</param>
        protected DbIndex(DbOption option, DbSql dbSql, IConnectFactory connectFactory)
        {
            Option = option;
            this._dbSql = dbSql;
            this.connectFactory = connectFactory;
        }

        /// <summary>
        /// Creates the index async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="sqlIndex">The sql index.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<long> CreateIndexAsync(string tableName, string[] columnNames, string indexName, string sqlIndex)
        {
            return Task.Run(() => CreateIndex(tableName, columnNames, indexName, sqlIndex));
        }

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="sqlIndex">The sql index.</param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual long CreateIndex(string tableName, string[] columnNames, string indexName, string sqlIndex)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = _dbSql.GetCreateIndexSql(tableName, columnNames, indexName, sqlIndex).SQL;
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes the index async.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public Task<long> DeleteIndexAsync(string tableName, string indexName)
        {
            return Task.Run(() => DeleteIndex(tableName, indexName));
        }

        /// <summary>
        /// Deletes the index.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual long DeleteIndex(string tableName, string indexName)
        {
            var connection = connectFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = _dbSql.GetDeleteIndexSql(tableName, indexName).SQL;
            return command.ExecuteNonQuery();
        }
    }
}