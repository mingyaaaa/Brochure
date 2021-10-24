using System;
using Brochure.ORM;

namespace Brochure.Organization.Entrities
{
    public class OrganizationEntiry : EntityBase
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
    }
}