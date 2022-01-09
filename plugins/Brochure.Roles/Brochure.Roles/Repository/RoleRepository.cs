using System;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.Roles.Entrities;

namespace Brochure.Roles.Repository
{
    /// <summary>
    /// The role repository.
    /// </summary>
    public class RoleRepository : RepositoryBase<RoleEntity, string>, IRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="dbData">The db data.</param>
        /// <param name="dbContext">The db context.</param>
        public RoleRepository(DbData dbData, DbContext dbContext) : base(dbData, dbContext)
        {
        }
    }
}