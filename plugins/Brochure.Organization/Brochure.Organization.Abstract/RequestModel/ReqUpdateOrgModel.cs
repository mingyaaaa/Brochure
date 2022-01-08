using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Organization.Abstract.RequestModel
{
    /// <summary>
    /// The req update org model.
    /// </summary>
    public class ReqUpdateOrgModel
    {
        /// <summary>
        /// Gets or sets the org id.
        /// </summary>
        public string OrgId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public string ParentId { get; set; }
    }
}
