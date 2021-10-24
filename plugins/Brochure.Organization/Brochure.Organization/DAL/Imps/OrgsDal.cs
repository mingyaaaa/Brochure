using Brochure.Abstract;
using Brochure.Organization.Abstract.Models;
using Brochure.Organization.Abstract.RequestModel;
using Brochure.Organization.DAL.Interfaces;
using Brochure.Organization.Entrities;
using Brochure.Organization.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Organization.DAL.Imps
{
    internal class OrgsDal : IOrgsDal
    {
        private readonly IObjectFactory _objectFactory;
        private readonly IOrganizationRepository _oraginzationRepository;

        public OrgsDal(IObjectFactory objectFactory,IOrganizationRepository oraginzationRepository) 
        {
            _objectFactory = objectFactory;
            _oraginzationRepository = oraginzationRepository;
        }

        public  async ValueTask<int> DeleteOrg(string id)
        {
           return await _oraginzationRepository.Delete(id);
        }

        public async ValueTask<IEnumerable<string>> DeleteOrgRtnErrorIds(IEnumerable<string> ids)
        {
            return await _oraginzationRepository.DeleteManyReturnError(ids);
        }

        public async ValueTask<int> DeleteOrgs(IEnumerable<string> ids)
        {
            return await  _oraginzationRepository.DeleteMany(ids);
        }

        public async ValueTask<OrganizationModel> GetOrg(string id)
        {
            var org = await _oraginzationRepository.Get(id);
            if (org != null) 
               return _objectFactory.Create<OrganizationEntiry,OrganizationModel> (org);
            return null;
        }

        public async ValueTask<IEnumerable<OrganizationModel>> GetOrgs(IEnumerable<string> ids)
        {
            var orgs = await _oraginzationRepository.List(ids);
            return orgs.Select(t => _objectFactory.Create<OrganizationEntiry, OrganizationModel>(t)).ToList();
        }

        public async ValueTask<int> InsertOrgs(IEnumerable<ReqAddOrgModel> reqAddOrgModels)
        {
            var list = reqAddOrgModels.Select(t => _objectFactory.Create<ReqAddOrgModel, OrganizationEntiry>(t));
            return await _oraginzationRepository.Insert(list);
        }

        public async ValueTask<int> UpdateOrg(string id, IRecord record)
        {
            var org = _objectFactory.Create<OrganizationEntiry>(record);
            return await _oraginzationRepository.Update(id,org);
        }
    }
}
