using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Organization.Abstract.RequestModel
{
    public class ReqUpdateOrgModel
    {
        public string OrgId { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
    }
}
