using Brochure.Organization.Entrities;
using Brochure.ORM;
using Brochure.ORM.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Organization.Repository
{
    /// <summary>
    /// The organization repository.
    /// </summary>
    public class OrganizationRepository : RepositoryBase<OrganizationEntiry, string>, IOrganizationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationRepository"/> class.
        /// </summary>
        /// <param name="dbData">The db data.</param>
        /// <param name="dbContext">The db context.</param>
        public OrganizationRepository(DbData dbData, DbContext dbContext) : base(dbData, dbContext)
        {
        }
    }
}