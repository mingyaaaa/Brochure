using Brochure.Organization.Entrities;
using Brochure.ORM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Organization.Repository
{
    /// <summary>
    /// The organization repository.
    /// </summary>
    public interface IOrganizationRepository : IRepository<OrganizationEntiry>
    {
        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        Task<int> Delete(string id);

        /// <summary>
        /// Deletes the many return error.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<string>> DeleteManyReturnError(IEnumerable<string> ids);

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<int> DeleteMany(IEnumerable<string> ids);

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        Task<OrganizationEntiry> Get(string id);

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<IList<OrganizationEntiry>> List(IEnumerable<string> ids);

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="org">The org.</param>
        /// <returns>A Task.</returns>
        Task<int> Update(string id, OrganizationEntiry org);
    }
}