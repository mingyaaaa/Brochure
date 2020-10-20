using System;
using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbData : DbData
    {
        public MySqlDbData (DbSql dbSql, DbOption dbOption, TransactionManager transactionManager, IConnectFactory connectFactory) : base (dbOption, dbSql, transactionManager, connectFactory) { }
    }
}