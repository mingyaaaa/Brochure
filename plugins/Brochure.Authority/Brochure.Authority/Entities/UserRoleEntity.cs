using Brochure.ORM;
using System;

namespace Brochure.Authority.Entities
{
    /// <summary>
    /// The user role entity.
    /// </summary>
    public class UserRoleEntity : EntityBase, IEntityKey<string>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public string RoleId { get; set; }
    }
}