using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    public class MySqlDbColumns : DbColumns
    {
        public MySqlDbColumns(DbOption option, DbSql dbSql, IConnectFactory connectFactory) : base(option, dbSql, connectFactory)
        {
        }
    }
}