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
        Task<T> Get(IQuery query);

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        Task<T> Get(string id);

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> Update(IWhereQuery query, T entity);

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> Update(string id, T entity);

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        Task<int> Delete(IWhereQuery query);

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        Task<int> Delete(string id);

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<int> DeleteMany(IEnumerable<string> ids);

        /// <summary>
        /// Deletes the many return error.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<string>> DeleteManyReturnError(IEnumerable<string> ids);

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<T>> List(IQuery query);

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<T>> List(IEnumerable<string> ids);

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> Insert(T entity);

        /// <summary>
        /// Insets the and get.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<T> InsetAndGet(T entity);

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<int> Insert(IEnumerable<T> entity);
    }
}