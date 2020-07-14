using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbIndex : DbIndex
    {
        public MySqlDbIndex (DbOption option, DbSql dbSql) : base (option, dbSql) { }
    }
}