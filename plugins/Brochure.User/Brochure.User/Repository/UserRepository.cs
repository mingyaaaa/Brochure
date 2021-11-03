using Brochure.ORM;
using Brochure.ORM.Querys;
using Brochure.User.Entrities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brochure.User.Repository
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public class UserRepository : RepositoryBase<UserEntrity, string>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}