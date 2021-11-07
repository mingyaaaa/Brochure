using Brochure.ORM;
using Brochure.User.Entrities;

namespace Brochure.User.Repository
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public class UserRepository : RepositoryBase<UserEntrity, string>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}