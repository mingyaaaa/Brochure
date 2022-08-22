using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Models
{
    public class UserInfoModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public IEnumerable<string> RoleIds { get; set; }

        public IEnumerable<string> DepIds { get; set; }
    }
}