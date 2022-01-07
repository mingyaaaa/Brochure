using Brochure.Abstract;
using Brochure.Organization.Abstract.RequestModel;
using Brochure.Organization.Entrities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Organization.DAL.Interfaces
{
    /// <summary>
    /// The orgs dal.
    /// </summary>
    public interface IOrgsDal
    {
        /// <summary>
        /// Inserts the orgs.
        /// </summary>
        /// <param name="reqAddOrgModels">The req add org models.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> InsertOrgs(IEnumerable<ReqAddOrgModel> reqAddOrgModels);

        /// <summary>
        /// Gets the orgs.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<OrganizationEntiry>> GetOrgs(IEnumerable<string> ids);

        /// <summary>
        /// Gets the orgs.
        /// </summary>
        /// <param name="queryParams">The query params.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<IRecord>> GetOrgs(QueryParams<OrganizationEntiry> queryParams);

        /// <summary>
        /// Gets the org.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<OrganizationEntiry> GetOrg(string id);

        /// <summary>
        /// Updates the org.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="record">The record.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> UpdateOrg(string id, IRecord record);

        /// <summary>
        /// Deletes the org.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> DeleteOrg(string id);

        /// <summary>
        /// Deletes the org rtn error ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<string>> DeleteOrgRtnErrorIds(IEnumerable<string> ids);

        /// <summary>
        /// Deletes the orgs.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> DeleteOrgs(IEnumerable<string> ids);
    }
}