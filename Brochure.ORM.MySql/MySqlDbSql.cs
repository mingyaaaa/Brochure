using System;
using LinqDbQuery;
using LinqDbQuery.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbSql : DbSql
    {
        public MySqlDbSql (IDbProvider dbProvider, DbOption dbOption) : base (dbProvider, dbOption) { }
    }
}