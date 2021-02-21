using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbDatabase : DbDatabase
    {
        public MySqlDbDatabase (DbOption dbOption, DbSql dbSql, IConnectFactory connectFactory) : base (dbOption, dbSql, connectFactory) { }
    }
}