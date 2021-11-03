using Brochure.Organization.Entrities;
using Brochure.ORM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Organization.Repository
{
    public class OrganizationRepository : RepositoryBase<OrganizationEntiry>, IOrganizationRepository
    {
        public OrganizationRepository(DbContext context) : base(context) { }

        public Task<int> Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteMany(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<string>> DeleteManyReturnError(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }

        public Task<OrganizationEntiry> Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<OrganizationEntiry>> List(IEnumerable<string> ids)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> Update(string id, OrganizationEntiry org)
        {
            throw new System.NotImplementedException();
        }
    }
}