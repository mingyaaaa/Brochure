using System;
using LinqDbQuery.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbData : DbData
    {
        public MySqlDbData (global::LinqDbQuery.DbOption dbOption) : base (dbOption, new MySqlDbSql (dbOption)) { }
    }
}