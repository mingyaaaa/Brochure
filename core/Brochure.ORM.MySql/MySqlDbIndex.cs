using AspectCore.Abstractions.DependencyInjection;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db index.
    /// </summary>
    public class MySqlDbIndex : DbIndex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbIndex"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        [InjectConstructor]
        public MySqlDbIndex(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbIndex"/> class.
        /// </summary>
        /// <param name="isBeginTransaction">If true, is begin transaction.</param>
        public MySqlDbIndex(bool isBeginTransaction = false) : base(new MySqlDbContext(isBeginTransaction))
        {
        }
    }
}