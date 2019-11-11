using System;
using LinqDbQuery;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlOption : DbOption
    {
        public MySqlOption (IDbProvider dbProvider) : base (dbProvider) { }
    }
}