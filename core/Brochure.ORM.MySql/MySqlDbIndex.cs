using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    public class MySqlDbIndex : DbIndex
    {
        public MySqlDbIndex(DbOption option, DbSql dbSql, IConnectFactory connectFactory) : base(option, dbSql, connectFactory)
        {
        }
    }
}