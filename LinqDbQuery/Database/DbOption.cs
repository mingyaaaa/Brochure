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

        protected DbOption (IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        private readonly IDbProvider dbProvider;

        public IDbConnection GetDbConnection ()
        {
            var connection = dbProvider.GetDbConnection ();
            if (string.IsNullOrWhiteSpace (ConnectionString))
                throw new Exception ("请设置数据库连接字符串");
            connection.ConnectionString = ConnectionString;
            DatabaseName = connection.Database;
            return connection;
        }
    }
}