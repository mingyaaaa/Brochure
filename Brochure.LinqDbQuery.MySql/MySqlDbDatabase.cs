using LinqDbQuery.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbDatabase : DbDatabase
    {
        public MySqlDbDatabase (global::LinqDbQuery.DbOption option) : base (option, new MySqlDbSql (option)) { }
    }
}