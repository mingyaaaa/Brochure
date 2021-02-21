using System;
using Brochure.ORM;
using Brochure.Roles.Entrities;

namespace Brochure.Roles.Repository
{
    public class RoleRepository : RepositoryBase<RoleEntiry>, IRoleRepository
    {
        public RoleRepository (DbContext context) : base (context) { }
    }
}