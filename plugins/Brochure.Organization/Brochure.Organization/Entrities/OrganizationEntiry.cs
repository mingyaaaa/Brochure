using System;
using Brochure.ORM;

namespace Brochure.Organization.Entrities
{
    public class OrganizationEntiry : EntityBase
    {
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