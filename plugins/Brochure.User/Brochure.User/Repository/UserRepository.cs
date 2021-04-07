using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.ORM;
using Brochure.ORM.Extensions;
using Brochure.ORM.Querys;
using Brochure.User.Entrities;

namespace Brochure.User.Repository
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public class UserRepository : RepositoryBase<UserEntrity>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="queryBuilder">The query builder.</param>
        public UserRepository(DbContext dbContext, IQueryBuilder queryBuilder) : base(dbContext, queryBuilder) { }
    }
}