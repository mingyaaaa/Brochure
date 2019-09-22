using System;
using System.Data;
using System.Data.Common;
using AspectCore.Injector;

namespace LinqDbQuery
{
    public class QueryOption
    {
        private string connectionStr;
        public QueryOption ()
        {
            DbProvider = DI.Ins.ServiceProvider.ResolveRequired<IDbProvider> ();
        }
        public int Timeout { get; set; }

        public IDbProvider DbProvider { get; private set; }
        public IDbConnection GetDbConnection ()
        {
            var connection = DbProvider.GetDbConnection ();
            connection.ConnectionString = connectionStr;
            return connection;
        }
    }
}