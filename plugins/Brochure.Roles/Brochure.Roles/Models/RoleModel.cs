using System;
using Brochure.Roles.Entrities;

namespace Brochure.Roles.Models
{
    /// <summary>
    /// The role model.
    /// </summary>
    public class RoleModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the entrity.
        /// </summary>
        /// <returns>A RoleEntity.</returns>
        public RoleEntity GetEntrity()
        {
            return new RoleEntity()
            {
                Name = this.Name,
            };
        }
    }
}