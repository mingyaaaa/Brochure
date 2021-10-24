using System;
using Brochure.Organization.Entrities;
using Brochure.ORM;
using Brochure.ORM.Querys;

namespace Brochure.Organization.Repository
{
    public class OrganizationRepository : RepositoryBase<OrganizationEntiry>, IOrganizationRepository
    {
        public OrganizationRepository (DbContext context,IQueryBuilder queryBuilder) : base (context,queryBuilder) { }
    }
}