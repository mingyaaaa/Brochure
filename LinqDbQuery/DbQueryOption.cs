using System;
using System.Data;
using System.Data.Common;
using AspectCore.Injector;

namespace LinqDbQuery
{
    public abstract class DbOption
    {
        protected string ConnectionString { get; set; }
        public int Timeout { get; set; }

        public string DatabaseName { get; set; }

        protected DbOption ()
        {
            DbProvider = DI.Ins.ServiceProvider.Resolve<IDbProvider> ();
        }

        protected DbOption (IDbProvider dbProvider)
        {
            DbProvider = dbProvider;
        }

        public IDbProvider DbProvider { get; }

        public IDbConnection GetDbConnection ()
        {
            var connection = DbProvider.GetDbConnection ();
            if (string.IsNullOrWhiteSpace (ConnectionString))
                throw new Exception ("请设置数据库连接字符串");
            connection.ConnectionString = ConnectionString;
            DatabaseName = connection.Database;
            return connection;
        }
    }
}