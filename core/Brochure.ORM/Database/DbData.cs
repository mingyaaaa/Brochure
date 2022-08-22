using Brochure.Abstract;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Querys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db data.
    /// </summary>
    public abstract class DbData : ITransaction, IAsyncDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbData"/> class.
        /// </summary>
        /// <param name="dbContext"></param>
        protected DbData(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected readonly DbContext _dbContext;

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        public IsolationLevel IsolationLevel => _dbContext.IsolationLevel;

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual Task<int> InsertAsync<T>(T obj) where T : class
        {
            return InsertMany<T>(new List<T>() { obj });
        }

        /// <summary>
        /// Inserts the many.
        /// </summary>
        /// <param name="objs">The objs.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual Task<int> InsertManyAsync<T>(IEnumerable<T> objs) where T : class
        {
            return InsertMany(objs);
        }

        /// <summary>
        /// Sqls the bulk copy async.
        /// </summary>
        /// <param name="objs">The objs.</param>
        /// <returns>A Task.</returns>
        [Transaction]
        public virtual Task<int> SqlBulkCopyAsync<T>(IEnumerable<T> objs) where T : class
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Inserts the many.
        /// </summary>
        /// <param name="objs">The objs.</param>
        /// <returns>A Task.</returns>
        private Task<int> InsertMany<T>(IEnumerable<T> objs) where T : class
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
            return Update<T>(obj, Query.Where(whereFunc));
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
            return Update<T>(obj, query);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        private Task<int> Update<T>(object obj, IWhereQuery query)
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
            return Delete<T>(Query.Where(whereFunc));
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual Task<int> DeleteAsync<T>(IWhereQuery query)
        {
            return Delete<T>(query);
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        private Task<int> Delete<T>(IWhereQuery query)
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
        public virtual Task<IEnumerable<IRecord>> FindAsync(IQuery query)
        {
            return _dbContext.ExcuteQueryAsync(query);
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

        /// <summary>
        /// Commits the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public Task CommitAsync()
        {
            return _dbContext.CommitAsync();
        }

        /// <summary>
        /// Rollbacks the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public Task RollbackAsync()
        {
            return _dbContext.RollbackAsync();
        }
    }
}