using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.ORM;
using Brochure.ORM.Extensions;
using Brochure.User.Entrities;

namespace Brochure.User.Repository
{
    internal class UserRepository : RepositoryBase<UserEntrity>, IUserRepository
    {
        internal UserRepository (DbContext dbContext) : base (dbContext) { }
    }
}