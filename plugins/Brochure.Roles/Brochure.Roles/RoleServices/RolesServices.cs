using Brochure.Roles.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Roles.RoleServices
{
    internal class RolesServices : IRolesService
    {
        public ValueTask<RoleServiceModel> GetRole(string id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<RoleServiceModel>> GetRoles(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<RoleServiceModel>> GetRolesByUserId(string userid)
        {
            throw new NotImplementedException();
        }
    }
}