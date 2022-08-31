using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Abstract
{
    public class LoginUserModel
    {
        public LoginUserModel()
        {
            Roles = new List<AccountRoleModel>();
            Depts = new List<AccountDeptModel>();
        }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public IEnumerable<AccountRoleModel> Roles { get; set; }

        public IEnumerable<AccountDeptModel> Depts { get; set; }
    }

    public class AccountRoleModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class AccountDeptModel
    {
        public string DeptId { get; set; }
        public string DeptName { get; set; }
    }
}