using System;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.Visitors;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbSql : DbSql
    {
        public MySqlDbSql (IDbProvider dbProvider, DbOption dbOption, IVisitProvider visitProvider) : base (dbProvider, dbOption, visitProvider) { }
    }
}