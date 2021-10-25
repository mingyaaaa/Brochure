using Brochure.Abstract;
using Brochure.Organization.Abstract.RequestModel;
using Brochure.Organization.Abstract.ResponesModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace Brochure.Organization.WebApi
{
    public interface IOrgWebApi
    {
        [HttpGet]
        Task<RspOrgModel> GetOrgModel(string id);

        Task<IEnumerable<RspOrgModel>> List();

        Task<RspOrgModel> AddOrgModel(ReqAddOrgModel reqAddOrgModel);

        Task<int> DeleteOrgModel(IEnumerable<string> ids);

        Task<RspOrgModel> UpdateOrg(string id,ReqUpdateOrgModel updateOrgModel);

    }
}
