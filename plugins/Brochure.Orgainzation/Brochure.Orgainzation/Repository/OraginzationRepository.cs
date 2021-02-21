using System;
using Brochure.Orgainzation.Entrities;
using Brochure.ORM;

namespace Brochure.Orgainzation.Repository
{
    public class OraginzationRepository : RepositoryBase<OraginzationEntiry>, IOraginzationRepository
    {
        public OraginzationRepository (DbContext context) : base (context) { }
    }
}