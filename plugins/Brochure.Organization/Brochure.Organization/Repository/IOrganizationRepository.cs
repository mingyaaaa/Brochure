using Brochure.Organization.Entrities;
using Brochure.ORM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Organization.Repository
{
    /// <summary>
    /// The organization repository.
    /// </summary>
    public interface IOrganizationRepository : IRepository<OrganizationEntiry, string>
    {
    }
}