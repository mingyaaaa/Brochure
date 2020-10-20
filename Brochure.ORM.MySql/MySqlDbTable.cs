using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbTable : DbTable
    {
        public MySqlDbTable (DbOption option, DbSql dbSql, IConnectFactory connectFactory) : base (option, dbSql, connectFactory) { }
    }
}