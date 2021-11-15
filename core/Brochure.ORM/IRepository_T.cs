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
        Task<T> Get(IWhereQuery<T> query);

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> Update(IWhereQuery query, T entity);

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        Task<int> Delete(IWhereQuery query);

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<T>> List(IQuery<T> query);

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> Insert(T entity);

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> Insert(IEnumerable<T> entity);
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
        Task<int> Delete(T2 id);

        /// <summary>
        /// Deletes the many return error.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<T2>> DeleteManyReturnError(IEnumerable<T2> ids);

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<int> DeleteMany(IEnumerable<T2> ids);

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        Task<T1> Get(T2 id);

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<IList<T1>> List(IEnumerable<T2> ids);

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="org">The org.</param>
        /// <returns>A Task.</returns>
        Task<int> Update(T2 id, T1 org);

        /// <summary>
        /// Insert And Get
        /// </summary>
        /// <param name="userEntrity"></param>
        /// <returns></returns>
        Task<T1> InsertAndGet(T1 userEntrity);
    }
}