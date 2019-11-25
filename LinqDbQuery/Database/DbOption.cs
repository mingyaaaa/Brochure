using System;
using System.Data;
using System.Data.Common;
using AspectCore.Injector;
using LinqDbQuery.Database;

namespace LinqDbQuery
{
    public abstract class DbOption
    {
        private IDbConnection dbConnection;
        public IsolationLevel TransactionLevel { get; set; } = IsolationLevel.ReadUncommitted;
        protected string ConnectionString { get; set; }
        public int Timeout { get; set; }

        public string DatabaseName { get; set; }

        protected DbOption (IDbProvider dbProvider, TransactionManager transactionManager)
        {
            this.dbProvider = dbProvider;
            this.transactionManager = transactionManager;
        }

        private readonly IDbProvider dbProvider;
        private readonly TransactionManager transactionManager;

        public IDbConnection GetDbConnection ()
        {
            if (dbConnection != null)
                return dbConnection;
            dbConnection = dbProvider.GetDbConnection ();
            if (string.IsNullOrWhiteSpace (ConnectionString))
                throw new Exception ("请设置数据库连接字符串");
            dbConnection.ConnectionString = ConnectionString;
            DatabaseName = dbConnection.Database;
            return dbConnection;
        }
    }
}