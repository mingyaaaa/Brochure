using AspectCore.Abstractions.DependencyInjection;
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
        /// <param name="dbContext"></param>
        [InjectConstructor]
        public MySqlDbDatabase(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbDatabase"/> class.
        /// </summary>
        /// <param name="isBeginTransaction">If true, is begin transaction.</param>
        public MySqlDbDatabase(bool isBeginTransaction = false) : base(new MySqlDbContext(isBeginTransaction))
        {
        }
    }
}