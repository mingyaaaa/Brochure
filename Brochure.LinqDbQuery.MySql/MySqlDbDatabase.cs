using LinqDbQuery;
using LinqDbQuery.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbDatabase : DbDatabase
    {
        public MySqlDbDatabase(DbOption dbOption, DbSql dbSql) : base(dbOption, dbSql) { }
    }
}