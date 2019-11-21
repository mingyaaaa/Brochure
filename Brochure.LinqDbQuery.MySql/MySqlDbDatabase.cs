using LinqDbQuery;
using LinqDbQuery.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbDatabase : DbDatabase
    {
        public MySqlDbDatabase (IDbProvider dbProvider, DbOption dbOption) : base (dbOption, new MySqlDbSql (dbProvider)) { }
    }
}