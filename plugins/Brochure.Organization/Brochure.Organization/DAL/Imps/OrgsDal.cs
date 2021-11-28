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
    internal class OrgsDal : IOrgsDal
    {
        private readonly IObjectFactory _objectFactory;
        private readonly IOrganizationRepository _oraginzationRepository;

        public OrgsDal(IObjectFactory objectFactory, IOrganizationRepository oraginzationRepository)
        {
            _objectFactory = objectFactory;
            _oraginzationRepository = oraginzationRepository;
        }

        public async ValueTask<int> DeleteOrg(string id)
        {
            return await _oraginzationRepository.DeleteAsync(id);
        }

        public async ValueTask<IEnumerable<string>> DeleteOrgRtnErrorIds(IEnumerable<string> ids)
        {
            return await _oraginzationRepository.DeleteManyReturnErrorAsync(ids);
        }

        public async ValueTask<int> DeleteOrgs(IEnumerable<string> ids)
        {
            return await _oraginzationRepository.DeleteManyAsync(ids);
        }

        public async ValueTask<OrganizationModel> GetOrg(string id)
        {
            var org = await _oraginzationRepository.GetAsync(id);
            if (org != null)
                return _objectFactory.Create<OrganizationEntiry, OrganizationModel>(org);
            return null;
        }

        public async ValueTask<IEnumerable<OrganizationModel>> GetOrgs(IEnumerable<string> ids)
        {
            var orgs = await _oraginzationRepository.ListAsync(ids);
            return orgs.Select(t => _objectFactory.Create<OrganizationEntiry, OrganizationModel>(t)).ToList();
        }

        public async ValueTask<int> InsertOrgs(IEnumerable<ReqAddOrgModel> reqAddOrgModels)
        {
            var list = reqAddOrgModels.Select(t => _objectFactory.Create<ReqAddOrgModel, OrganizationEntiry>(t));
            return await _oraginzationRepository.InsertAsync(list);
        }

        public async ValueTask<int> UpdateOrg(string id, IRecord record)
        {
            var org = _objectFactory.Create<OrganizationEntiry>(record);
            return await _oraginzationRepository.UpdateAsync(id, org);
        }
    }
}