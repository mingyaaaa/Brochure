using System;
using Brochure.Orgainzation.Entrities;

namespace Brochure.Orgainzation.Models
{
    public class OraginzationModel
    {
        public string Name { get; set; }

        public string ParentId { get; set; }
        public OraginzationEntiry GetEntrity ()
        {
            return new OraginzationEntiry ()
            {
                Name = this.Name,
                    ParentId = this.ParentId,
            };
        }
    }
}