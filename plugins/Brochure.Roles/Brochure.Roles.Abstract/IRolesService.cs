using Proto.RolesGrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Roles.Abstract
{
    public interface IRolesService
    {
        ValueTask<RoleServiceModel> GetRole(string id);

        ValueTask<IEnumerable<RoleServiceModel>> GetRoles(IEnumerable<string> ids);

        ValueTask<IEnumerable<RoleServiceModel>> GetRolesByUserId(string userid);
    }
}