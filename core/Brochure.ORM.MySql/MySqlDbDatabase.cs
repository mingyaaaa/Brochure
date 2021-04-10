using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.LinqDbQuery.MySql
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
        public MySqlDbDatabase(DbOption dbOption, DbSql dbSql, IConnectFactory connectFactory) : base(dbOption, dbSql, connectFactory) { }
    }
}