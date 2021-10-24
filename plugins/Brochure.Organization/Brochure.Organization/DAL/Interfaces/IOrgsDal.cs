using Brochure.Abstract;
using Brochure.Organization.Abstract.Models;
using Brochure.Organization.Abstract.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Organization.DAL.Interfaces
{
    public interface IOrgsDal
    {
        ValueTask<int> InsertOrgs(IEnumerable<ReqAddOrgModel> reqAddOrgModels);

        ValueTask<IEnumerable<OrganizationModel>> GetOrgs(IEnumerable<string> ids);

        ValueTask<OrganizationModel> GetOrg(string id);

        ValueTask<int> UpdateOrg(string id, IRecord record);

        ValueTask<int> DeleteOrg(string id);

        ValueTask<IEnumerable<string>> DeleteOrgRtnErrorIds(IEnumerable<string> ids);

        ValueTask<int> DeleteOrgs(IEnumerable<string> ids);
    }
}
