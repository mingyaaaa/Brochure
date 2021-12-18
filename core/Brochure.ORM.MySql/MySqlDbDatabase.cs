using AspectCore.Abstractions.DependencyInjection;
using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db database.
    /// </summary>
    public class MySqlDbDatabase : DbDatabase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbDatabase"/> class.
        /// </summary>
        /// <param name="dbOption">The db option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="connectFactory">The connect factory.</param>
        [InjectConstructor]
        public MySqlDbDatabase(DbContext dbContext) : base(dbContext) { }

        public MySqlDbDatabase(bool isBeginTransaction = false) : base(new MySqlDbContext(isBeginTransaction))
        {
        }
    }
}