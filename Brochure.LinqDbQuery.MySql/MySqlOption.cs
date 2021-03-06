using System;
using LinqDbQuery;
using LinqDbQuery.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlOption : DbOption
    {
        public MySqlOption (IDbProvider dbProvider, TransactionManager manager) : base (dbProvider, manager) { }
    }
}