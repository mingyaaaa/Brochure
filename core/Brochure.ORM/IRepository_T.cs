using Brochure.Abstract;
using Brochure.ORM.Querys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    /// <summary>
    /// The repository.
    /// </summary>
    public interface IRepository<T> where T : EntityBase
    {
        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        Task<T> GetAsync(IWhereQuery<T> query);

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> UpdateAsync(IWhereQuery query, T entity);

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        Task<int> DeleteAsync(IWhereQuery query);

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<T>> ListAsync(IQuery<T> query);

        /// <summary>
        /// Lists the async.
        /// </summary>
        /// <param name="queryParams">The query params.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<IRecord>> ListAsync(QueryParams<T> queryParams);

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> InsertAsync(T entity);

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> InsertAsync(IEnumerable<T> entity);
    }

    /// <summary>
    /// The repository.
    /// </summary>
    public interface IRepository<T1, T2> : IRepository<T1> where T1 : EntityBase, IEntityKey<T2>
         where T2 : class, IComparable<T2>
    {
        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        Task<int> DeleteAsync(T2 id);

        /// <summary>
        /// Deletes the many return error.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<T2>> DeleteManyReturnErrorAsync(IEnumerable<T2> ids);

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<int> DeleteManyAsync(IEnumerable<T2> ids);

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        Task<T1> GetAsync(T2 id);

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<IList<T1>> ListAsync(IEnumerable<T2> ids);

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="org">The org.</param>
        /// <returns>A Task.</returns>
        Task<int> UpdateAsync(T2 id, T1 org);

        /// <summary>
        /// Insert And Get
        /// </summary>
        /// <param name="userEntrity"></param>
        /// <returns></returns>
        Task<T1> InsertAndGetAsync(T1 userEntrity);
    }
}