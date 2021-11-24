using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Brochure.ORM.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    public abstract class RepositoryBase<T> : IRepository<T> where T : EntityBase, new()
    {
        protected readonly DbContext context;
        protected readonly DbSql _dbSql;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public RepositoryBase(DbContext context, DbSql dbSql)
        {
            this.context = context;
            _dbSql = dbSql;
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        public async Task<int> Delete(IWhereQuery query)
        {
            var r = await Task.Run(() => context.Datas.Delete<T>(query)).ConfigureAwait(false);
            return r;
        }

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        public async Task<T> Get(IWhereQuery<T> query)
        {
            var tQuery = Query.From<T>(query).Take(1);
            var t = await Task.Run(() => context.Datas.Find<T>(tQuery)).ConfigureAwait(false);
            return t.FirstOrDefault();
        }

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public Task<int> Insert(T entity)
        {
            return Insert(new List<T>() { entity });
        }

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public async Task<int> Insert(IEnumerable<T> entity)
        {
            var r = await Task.Run(() => context.Datas.InsertMany(entity)).ConfigureAwait(false);
            return r;
        }

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<T>> List(IQuery<T> query)
        {
            query.Select<T>();
            var t = await Task.Run(() => context.Datas.Find<T>(query)).ConfigureAwait(false);
            return t;
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
            var t = await Task.Run(() => context.Datas.Update<T>(entity, query)).ConfigureAwait(false);
            return t;
        }
    }

    /// <summary>
    /// The repository base.
    /// </summary>
    public abstract class RepositoryBase<T1, T2> : RepositoryBase<T1>, IRepository<T1, T2>
        where T1 : EntityBase, IEntityKey<T2>, new()
        where T2 : class, IComparable<T2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected RepositoryBase(DbContext context, DbSql dbSql) : base(context, dbSql)
        {
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public Task<int> Delete(T2 id)
        {
            return base.Delete(Query.Where<T1>(t => t.Id == id));
        }

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        public Task<int> DeleteMany(IEnumerable<T2> ids)
        {
            return base.Delete(Query.Where<T1>(t => ids.Contains(t.Id)));
        }

        /// <summary>
        /// Deletes the many return error.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<T2>> DeleteManyReturnError(IEnumerable<T2> ids)
        {
            var list = new List<T2>();
            foreach (var item in ids)
            {
                var r = await Delete(Query.Where<T1>(t => t.Id == item));
                if (r < 0)
                    list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public Task<T1> Get(T2 id)
        {
            return base.Get(Query.Where<T1>(t => t.Id == id));
        }

        /// <summary>
        /// Inserts the and get.
        /// </summary>
        /// <param name="userEntrity">The user entrity.</param>
        /// <returns>A Task.</returns>
        public Task<T1> InsertAndGet(T1 userEntrity)
        {
            var sql = _dbSql.GetInsertSql<T1>(userEntrity).Continue(Query.From<T1>().Where(t => t.Id == userEntrity.Id).Take(1));
            return Task.Run(() => context.Datas.ExcuteQuery<T1>(sql).FirstOrDefault());
        }

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        public async Task<IList<T1>> List(IEnumerable<T2> ids)
        {
            var list = await base.List(Query.Where<T1>(t => ids.Contains(t.Id)));
            return list.ToList();
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>A Task.</returns>
        public Task<int> Update(T2 id, T1 obj)
        {
            return base.Update(Query.Where<T1>(t => t.Id == id), obj);
        }
    }
}