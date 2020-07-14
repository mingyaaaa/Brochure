using System;
using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlOption : DbOption
    {
        public MySqlOption (IDbProvider dbProvider, TransactionManager manager) : base (dbProvider, manager) { }
    }
}