using System.Threading.Tasks;
using Brochure.ORM.Atrributes;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db index.
    /// </summary>
    public abstract class DbIndex
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbIndex"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="connectFactory">The connect factory.</param>
        protected DbIndex(DbContext dbContext)
        {
            _dbContext = dbContext;
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
        public virtual Task<int> CreateIndexAsync(string tableName, string[] columnNames, string indexName, string sqlIndex)
        {
            var sql = Sql.CreateIndex(tableName, columnNames, indexName, sqlIndex);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Deletes the index.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <returns>A long.</returns>
        [Transaction]
        public virtual Task<int> DeleteIndexAsync(string tableName, string indexName)
        {
            var sql = Sql.DeleteIndex(tableName, indexName);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }
    }
}