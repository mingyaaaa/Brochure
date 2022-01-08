using System;
using Brochure.ORM;

namespace Brochure.Organization.Entrities
{
    /// <summary>
    /// The organization entiry.
    /// </summary>
    public class OrganizationEntiry : EntityBase, IEntityKey<string>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}