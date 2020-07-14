using System;
using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbSql : DbSql
    {
        public MySqlDbSql (IDbProvider dbProvider, DbOption dbOption) : base (dbProvider, dbOption) { }
    }
}