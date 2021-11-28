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
        /// <param name="dbSql">The db sql.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="connectFactory">The connect factory.</param>
        /// <param name="objectFactory">The object factory.</param>
        public MySqlDbData(DbContext dbContext) : base(dbContext) { }
    }
}