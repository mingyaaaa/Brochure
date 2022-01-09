using System;
using Brochure.ORM;
using Brochure.Roles.Entrities;

namespace Brochure.Roles.Repository
{
    /// <summary>
    /// The role repository.
    /// </summary>
    public interface IRoleRepository : IRepository<RoleEntity, string>
    { }
}