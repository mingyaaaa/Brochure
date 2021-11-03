using Brochure.ORM;
using Brochure.User.Entrities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.User.Repository
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public interface IUserRepository : IRepository<UserEntrity, string>
    {
    }
}