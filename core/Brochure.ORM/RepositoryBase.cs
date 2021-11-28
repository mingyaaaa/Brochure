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
        private readonly DbData _dbData;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public RepositoryBase(DbData dbData)
        {
            _dbData = dbData;
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        public Task<int> DeleteAsync(IWhereQuery query)
        {
            return _dbData.DeleteAsync<T>(query);
        }

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        public async Task<T> GetAsync(IWhereQuery<T> query)
        {
            var tQuery = Query.From<T>(query).Take(1);
            var t = await _dbData.FindAsync<T>((IQuery)tQuery).ConfigureAwait(false);
            return t.FirstOrDefault();
        }

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public Task<int> InsertAsync(T entity)
        {
            return InsertAsync(new List<T>() { entity });
        }

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public Task<int> InsertAsync(IEnumerable<T> entity)
        {
            return _dbData.InsertManyAsync(entity);
        }

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        public Task<IEnumerable<T>> ListAsync(IQuery<T> query)
        {
            query.Select<T>();
            return _dbData.FindAsync<T>((IQuery)query);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        public Task<int> UpdateAsync(IWhereQuery query, T entity)
        {
            if (query == null)
                throw new Exception("query不能为null");
            return _dbData.UpdateAsync<T>(entity, query);
        }
    }

    /// <summary>
    /// The repository base.
    /// </summary>
    public abstract class RepositoryBase<T1, T2> : RepositoryBase<T1>, IRepository<T1, T2>
        where T1 : EntityBase, IEntityKey<T2>, new()
        where T2 : class, IComparable<T2>
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected RepositoryBase(DbData dbData, DbContext dbContext) : base(dbData)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public Task<int> DeleteAsync(T2 id)
        {
            return base.DeleteAsync(Query.Where<T1>(t => t.Id == id));
        }

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        public Task<int> DeleteManyAsync(IEnumerable<T2> ids)
        {
            return base.DeleteAsync(Query.Where<T1>(t => ids.Contains(t.Id)));
        }

        /// <summary>
        /// Deletes the many return error.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<T2>> DeleteManyReturnErrorAsync(IEnumerable<T2> ids)
        {
            var list = new List<T2>();
            foreach (var item in ids)
            {
                var r = await DeleteAsync(Query.Where<T1>(t => t.Id == item));
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
        public Task<T1> GetAsync(T2 id)
        {
            return base.GetAsync(Query.Where<T1>(t => t.Id == id));
        }

        /// <summary>
        /// Inserts the and get.
        /// </summary>
        /// <param name="userEntrity">The user entrity.</param>
        /// <returns>A Task.</returns>
        public async Task<T1> InsertAndGetAsync(T1 userEntrity)
        {
            var sql = Sql.InsertSql<T1>(userEntrity).Continue(Query.From<T1>().Where(t => t.Id == userEntrity.Id).Take(1));
            var list = await _dbContext.ExcuteQueryAsync<T1>(sql.ToArray());
            return list.FirstOrDefault();
        }

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        public async Task<IList<T1>> ListASync(IEnumerable<T2> ids)
        {
            var list = await base.ListAsync(Query.Where<T1>(t => ids.Contains(t.Id)));
            return list.ToList();
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>A Task.</returns>
        public Task<int> UpdateAsync(T2 id, T1 obj)
        {
            return base.UpdateAsync(Query.Where<T1>(t => t.Id == id), obj);
        }
    }
}