using AspectCore.Abstractions.DependencyInjection;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db data.
    /// </summary>
    public class MySqlDbData : DbData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbData"/> class.
        /// </summary>
        /// <param name="dbContext"></param>
        [InjectConstructor]
        public MySqlDbData(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbData"/> class.
        /// </summary>
        /// <param name="isBeginTransaction">If true, is begin transaction.</param>
        public MySqlDbData(bool isBeginTransaction = false) : base(new MySqlDbContext(isBeginTransaction))
        {
        }
    }
}