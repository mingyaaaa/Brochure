using System;
using Brochure.ORM;

namespace Brochure.Roles.Entrities
{
    /// <summary>
    /// The role entity.
    /// </summary>
    public class RoleEntity : EntityBase, IEntityKey<string>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }
    }
}