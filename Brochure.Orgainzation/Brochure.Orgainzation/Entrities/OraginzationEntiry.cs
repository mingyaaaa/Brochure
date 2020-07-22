using System;
using Brochure.ORM;

namespace Brochure.Orgainzation.Entrities
{
    public class OraginzationEntiry : EntityBase
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
    }
}