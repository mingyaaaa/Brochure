using Brochure.ORM.Database;
using Brochure.ORM.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db context.
    /// </summary>
    public class MySqlDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbContext"/> class.
        /// </summary>
        /// <param name="dbDatabase">The db database.</param>
        /// <param name="dbTable">The db table.</param>
        /// <param name="dbColumns">The db columns.</param>
        /// <param name="dbIndex">The db index.</param>
        /// <param name="dbData">The db data.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="dbProvider">The db provider.</param>
        /// <param name="visitProvider">The visit provider.</param>
        public MySqlDbContext(DbDatabase dbDatabase, DbTable dbTable,
                DbColumns dbColumns, DbIndex dbIndex, DbData dbData, DbOption dbOption, IDbProvider dbProvider, IVisitProvider visitProvider) :
            base(dbDatabase, dbTable, dbColumns, dbIndex, dbData, dbOption, dbProvider, visitProvider)
        { }
    }
}