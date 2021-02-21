using System;
using System.Data;
namespace Brochure.ORM.Database
{
    public interface IConnectFactory
    {
        IDbConnection CreaConnection ();
    }
    public class ConnectFactory : IConnectFactory
    {
        private readonly IDbProvider provider;
        private IDbConnection dbConnection;
        private readonly DbOption dbOption;

        public ConnectFactory (IDbProvider provider , DbOption dbOption)
        {
            this.provider = provider;
            this.dbOption = dbOption;
        }
        public IDbConnection CreaConnection ()
        {
            if (dbConnection != null)
                return dbConnection;
            dbConnection = provider.GetDbConnection ();
            if (string.IsNullOrWhiteSpace (dbOption.ConnectionString))
                throw new Exception ("请设置数据库连接字符串");
            dbConnection.ConnectionString = dbOption.ConnectionString;
            dbOption.DatabaseName = dbConnection.Database;
            return dbConnection;
        }
    }
}