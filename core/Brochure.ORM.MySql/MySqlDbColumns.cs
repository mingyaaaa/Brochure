using AspectCore.Abstractions.DependencyInjection;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db columns.
    /// </summary>
    public class MySqlDbColumns : DbColumns
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbColumns"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        [InjectConstructor]
        public MySqlDbColumns(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbColumns"/> class.
        /// </summary>
        /// <param name="isBeginTransaction">If true, is begin transaction.</param>
        public MySqlDbColumns(bool isBeginTransaction = false) : base(new MySqlDbContext(isBeginTransaction))
        {
        }
    }
}