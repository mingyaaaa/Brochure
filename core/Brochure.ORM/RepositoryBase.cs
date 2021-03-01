using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Brochure.ORM.Querys;

namespace Brochure.ORM
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    public abstract class RepositoryBase<T> : IRepository<T> where T : EntityBase
    {
        protected readonly DbContext context;
        private readonly IQueryBuilder _queryBuilder;
        protected readonly DbData dbData;
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public RepositoryBase(DbContext context, IQueryBuilder queryBuilder)
        {
            this.context = context;
            _queryBuilder = queryBuilder;
            dbData = context.GetDbData();
        }
        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        public async Task<int> Delete(IWhereQuery query)
        {
            var r = await Task.Run(() => dbData.Delete<T>(query)).ConfigureAwait(false);
            return r;
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<int> Delete(string id)
        {
            var query = _queryBuilder.Build<T>();
            var q = query.WhereAnd(t => t.Id == id) as IWhereQuery;
            return await Delete(q);
        }

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        public async Task<int> DeleteMany(IEnumerable<string> ids)
        {
            var query = _queryBuilder.Build<T>();
            var q = query.WhereAnd(t => ids.Contains(t.Id)) as IWhereQuery;
            return await Delete(q);
        }

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<string>> DeleteManyReturnError(IEnumerable<string> ids)
        {
            var query = _queryBuilder.Build<T>();
            var errorIds = new List<string>();
            foreach (var item in ids)
            {
                var q = query.WhereAnd(t => ids.Contains(t.Id)) as IWhereQuery;
                var r = await Delete(q);
                if (r > 0)
                {
                    errorIds.Add(item);
                }
            }
            return errorIds;
        }

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        public async Task<T> Get(IQuery query)
        {
            var t = await Task.Run(() => dbData.Query<T>(query)).ConfigureAwait(false);
            return t.FirstOrDefault();
        }

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<T> Get(string id)
        {
            var query = _queryBuilder.Build<T>();
            var q = query.WhereAnd(t => t.Id == id);
            return await Get(q);
        }

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public async Task<int> Insert(T entity)
        {
            var r = await Task.Run(() => dbData.Insert(entity)).ConfigureAwait(false);
            return r;
        }

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public async Task<int> Insert(IEnumerable<T> entity)
        {
            var r = await Task.Run(() => dbData.InsertMany(entity)).ConfigureAwait(false);
            return r;
        }

        /// <summary>
        /// Insets the and get.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public async Task<T> InsetAndGet(T entity)
        {
            var r = await Insert(entity);
            if (r <= 0)
                return null;
            var e = await Get(entity.Id);
            return e;
        }

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<T>> List(IQuery query)
        {
            var t = await Task.Run(() => dbData.Query<T>(query)).ConfigureAwait(false);
            return t;
        }

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<T>> List(IEnumerable<string> ids)
        {
            var query = _queryBuilder.Build<T>();
            var q = query.WhereAnd(t => ids.Contains(t.Id));
            return await List(q);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public async Task<int> Update(IWhereQuery query, T entity)
        {
            if (query == null)
                throw new Exception("query不能为null");
            var t = await Task.Run(() => dbData.Update<T>(entity, query)).ConfigureAwait(false);
            return t;
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public async Task<int> Update(string id, T entity)
        {
            var query = _queryBuilder.Build<T>();
            var q = query.WhereAnd(t => t.Id == id) as IWhereQuery;
            return await Update(q, entity);
        }

    }
}