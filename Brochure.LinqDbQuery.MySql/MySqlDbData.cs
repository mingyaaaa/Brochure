using System;
using LinqDbQuery;
using LinqDbQuery.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbData : DbData
    {
        public MySqlDbData (IDbProvider dbProvider, DbOption dbOption) : base (dbOption, new MySqlDbSql (dbProvider)) { }
    }
}