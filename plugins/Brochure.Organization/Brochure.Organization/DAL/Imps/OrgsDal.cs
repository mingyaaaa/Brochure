using Brochure.Abstract;
using Brochure.Organization.Abstract.Models;
using Brochure.Organization.Abstract.RequestModel;
using Brochure.Organization.DAL.Interfaces;
using Brochure.Organization.Entrities;
using Brochure.Organization.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brochure.Organization.DAL.Imps
{
    /// <summary>
    /// The orgs dal.
    /// </summary>
    internal class OrgsDal : IOrgsDal
    {
        private readonly IObjectFactory _objectFactory;
        private readonly IOrganizationRepository _oraginzationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrgsDal"/> class.
        /// </summary>
        /// <param name="objectFactory">The object factory.</param>
        /// <param name="oraginzationRepository">The oraginzation repository.</param>
        public OrgsDal(IObjectFactory objectFactory, IOrganizationRepository oraginzationRepository)
        {
            _objectFactory = objectFactory;
            _oraginzationRepository = oraginzationRepository;
        }

        /// <summary>
        /// Deletes the org.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<int> DeleteOrg(string id)
        {
            return await _oraginzationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Deletes the org rtn error ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IEnumerable<string>> DeleteOrgRtnErrorIds(IEnumerable<string> ids)
        {
            return await _oraginzationRepository.DeleteManyReturnErrorAsync(ids);
        }

        /// <summary>
        /// Deletes the orgs.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<int> DeleteOrgs(IEnumerable<string> ids)
        {
            return await _oraginzationRepository.DeleteManyAsync(ids);
        }

        /// <summary>
        /// Gets the org.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<OrganizationEntiry> GetOrg(string id)
        {
            return await _oraginzationRepository.GetAsync(id);
        }

        /// <summary>
        /// Gets the orgs.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IEnumerable<OrganizationEntiry>> GetOrgs(IEnumerable<string> ids)
        {
            return await _oraginzationRepository.ListAsync(ids);
        }

        /// <summary>
        /// Gets the orgs.
        /// </summary>
        /// <param name="queryParams">The query params.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IEnumerable<IRecord>> GetOrgs(QueryParams<OrganizationEntiry> queryParams)
        {
            var orgs = await _oraginzationRepository.ListAsync(queryParams);
            return orgs;
        }

        /// <summary>
        /// Inserts the orgs.
        /// </summary>
        /// <param name="reqAddOrgModels">The req add org models.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<int> InsertOrgs(IEnumerable<ReqAddOrgModel> reqAddOrgModels)
        {
            var list = reqAddOrgModels.Select(t => _objectFactory.Create<ReqAddOrgModel, OrganizationEntiry>(t));
            return await _oraginzationRepository.InsertAsync(list);
        }

        /// <summary>
        /// Updates the org.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="record">The record.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<int> UpdateOrg(string id, IRecord record)
        {
            var org = _objectFactory.Create<OrganizationEntiry>(record);
            return await _oraginzationRepository.UpdateAsync(id, org);
        }
    }
}