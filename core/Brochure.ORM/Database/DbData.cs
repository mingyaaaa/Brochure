using Brochure.ORM.Atrributes;
using Brochure.ORM.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db data.
    /// </summary>
    public abstract class DbData : IAsyncDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbData"/> class.
        /// </summary>
        /// <param name="dbOption">The db option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="connectFactory">The connect factory.</param>
        /// <param name="objectFactory">The object factory.</param>
        protected DbData(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly DbContext _dbContext;

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual Task<int> InsertAsync<T>(T obj) where T : class
        {
            return InsertManyAsync<T>(new List<T>() { obj });
        }

        /// <summary>
        /// Inserts the many.
        /// </summary>
        /// <param name="objs">The objs.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual Task<int> InsertManyAsync<T>(IEnumerable<T> objs) where T : class
        {
            var list = objs.ToList();
            var sqlList = new List<ISql>();
            foreach (var item in list)
            {
                var t_sql = Sql.InsertSql(item);
                sqlList.Add(t_sql);
            }
            return _dbContext.ExcuteNoQueryAsync(sqlList);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual Task<int> UpdateAsync<T>(object obj, Expression<Func<T, bool>> whereFunc)
        {
            return UpdateAsync<T>(obj, Query.Where(whereFunc));
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="query">The query.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual Task<int> UpdateAsync<T>(object obj, IWhereQuery query)
        {
            var sql = Sql.UpdateSql<T>(obj, query);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="whereFunc">The where func.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual Task<int> DeleteAsync<T>(Expression<Func<T, bool>> whereFunc)
        {
            return DeleteAsync<T>(Query.Where(whereFunc));
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual Task<int> DeleteAsync<T>(IWhereQuery query)
        {
            var sql = Sql.DeleteSql<T>(query);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        /// <summary>
        /// Queries the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A list of TS.</returns>
        public virtual Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query) where T : class, new()
        {
            return _dbContext.ExcuteQueryAsync<T>(query);
        }

        /// <summary>
        /// Queries the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A list of TS.</returns>
        public virtual Task<IEnumerable<T>> FindAsync<T>(IQuery query) where T : class, new()
        {
            return _dbContext.ExcuteQueryAsync<T>(query);
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