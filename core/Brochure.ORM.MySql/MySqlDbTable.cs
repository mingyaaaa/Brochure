using AspectCore.Abstractions.DependencyInjection;
using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db table.
    /// </summary>
    public class MySqlDbTable : DbTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbTable"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        [InjectConstructor]
        public MySqlDbTable(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbTable"/> class.
        /// </summary>
        /// <param name="isBeginTransaction">If true, is begin transaction.</param>
        public MySqlDbTable(bool isBeginTransaction = false) : base(new MySqlDbContext(isBeginTransaction))
        {
        }
    }
}