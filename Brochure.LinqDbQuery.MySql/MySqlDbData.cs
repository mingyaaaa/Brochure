using System;
using LinqDbQuery;
using LinqDbQuery.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbData : DbData
    {
        public MySqlDbData (DbSql dbSql, DbOption dbOption, TransactionManager transactionManager) : base (dbOption, dbSql, transactionManager) { }
    }
}