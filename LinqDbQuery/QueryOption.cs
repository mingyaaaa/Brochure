using System;
using System.Data;
using System.Data.Common;
using AspectCore.Injector;

namespace LinqDbQuery
{
    public abstract class DbQueryOption
    {
        public bool IsUseParamers { get; set; }
        public string ConnectionString { get; set; }
        public int Timeout { get; set; }
        public DbQueryOption ()
        {
            DbProvider = DI.Ins.ServiceProvider.ResolveRequired<IDbProvider> ();
        }

        public IDbProvider DbProvider { get; private set; }
        public IDbConnection GetDbConnection ()
        {
            var connection = DbProvider.GetDbConnection ();
            if (string.IsNullOrWhiteSpace (ConnectionString))
                throw new Exception ("请设置数据库连接字符串");
            connection.ConnectionString = ConnectionString;
            return connection;
        }
    }
}