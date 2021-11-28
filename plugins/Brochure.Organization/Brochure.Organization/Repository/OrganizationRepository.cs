using Brochure.Organization.Entrities;
using Brochure.ORM;
using Brochure.ORM.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Organization.Repository
{
    public class OrganizationRepository : RepositoryBase<OrganizationEntiry, string>, IOrganizationRepository
    {
        public OrganizationRepository(DbData dbData, DbContext dbContext) : base(dbData, dbContext)
        {
        }
    }
}