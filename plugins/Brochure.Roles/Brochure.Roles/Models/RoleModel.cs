using System;
using Brochure.Roles.Entrities;

namespace Brochure.Roles.Models
{
    public class RoleModel
    {
        public string Name { get; set; }

        public RoleEntiry GetEntrity ()
        {
            return new RoleEntiry ()
            {
                Name = this.Name,
            };
        }
    }
}